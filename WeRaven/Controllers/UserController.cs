using System.Globalization;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeRaven.Data;
using WeRaven.Models;
using WeRaven.Models.Mongo;
using WeRaven.Services;
using WeRaven.ViewModels;

namespace WeRaven.Controllers;

[ApiController]
[Route("v1/users")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserViewModel model,
        [FromServices] MongoService mongoService, [FromServices] EmailService emailService)
    {
        var existEmail = await _context.Users
            .AsNoTracking()
            .Select(x => x.Email)
            .FirstOrDefaultAsync(x => x == model.Email);
        if (existEmail != null)
        {
            return BadRequest(new
            {
                message = "This email is already in use"
            });
        }

        var existUsername = await _context.Users
            .AsNoTracking()
            .Select(x => x.Username)
            .FirstOrDefaultAsync(x => x == model.Username);
        if (existUsername != null)
        {
            return BadRequest(new
            {
                message = "This username is already in use"
            });
        }

        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Username = model.Username,
            Birthdate = DateTime.ParseExact(model.Birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToUniversalTime(),
            Password = Argon2.Hash(model.Password)
        };
        var auth = new Auth
        {
            UserId = user.Id
        };
        await _context.Users.AddAsync(user);
        await _context.Auths.AddAsync(auth);
        await _context.SaveChangesAsync();

        var account = new Account
        {
            UserId = user.Id
        };
        await mongoService.Account.CreateAsync(account);

        await emailService.CompileAsync<VerifyTemplateViewModel>("Verify", new VerifyTemplateViewModel
        {
            Code = auth.Code,
            Name = user.FirstName
        });
        emailService.Send($"{user.FirstName} {user.LastName}", user.Email, "Verfique seu e-mail");

        return Ok();
    }

    [HttpPost("re-send")]
    public async Task<IActionResult> Resend([FromBody] DefaultUserViewModel model,
        [FromServices] EmailService emailService)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Select(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.Email
            })
            .FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null)
        {
            return NotFound(new
            {
                message = "User not found."
            });
        }

        var auth = await _context.Auths
            .FirstOrDefaultAsync(x => x.UserId == user.Id);
        if (auth == null)
        {
            auth = new Auth
            {
                UserId = user.Id
            };
            await _context.Auths.AddAsync(auth);
            await _context.SaveChangesAsync();
        }
        else
        {
            var currentTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            var expiryTimestamp = new DateTimeOffset(auth.ExpiryAt).ToUnixTimeMilliseconds();

            if (currentTimestamp <= expiryTimestamp)
            {
                auth.ExpiryAt = DateTime.UtcNow.AddHours(1);
                _context.Auths.Update(auth);
                await _context.SaveChangesAsync();
            }
        }
        
        await emailService.CompileAsync<VerifyTemplateViewModel>("Verify", new VerifyTemplateViewModel
        {
            Code = auth.Code,
            Name = user.FirstName
        });
        emailService.Send($"{user.FirstName} {user.LastName}", user.Email, "Verfique seu e-mail");

        return Ok();
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyViewModel model, [FromServices] TokenService tokenService)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null)
        {
            return NotFound(new
            {
                message = "User not found."
            });
        }

        var auth = await _context.Auths
            .FirstOrDefaultAsync(x => x.UserId == user.Id);
        if (auth == null)
        {
            return BadRequest(new
            {
                message = "Can't authenticate your verification request."
            });
        }

        if (auth.Code != model.Code)
        {
            return BadRequest(new
            {
                message = "Invalid code"
            });
        }
        var currentTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        var expiryTimestamp = new DateTimeOffset(auth.ExpiryAt).ToUnixTimeMilliseconds();

        if (currentTimestamp <= expiryTimestamp)
        {
            return BadRequest(new
            {
                message = "Your request is expired."
            });
        }

        user.Confirmed = true;
        _context.Users.Update(user);
        _context.Auths.Remove(auth);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            token = tokenService.GenerateToken(user)
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model, [FromServices] TokenService tokenService)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => model.IsEmail ? x.Email == model.Username : x.Username == model.Username);
        if (user == null)
        {
            return BadRequest(new
            {
                message = "Incorrect email or password."
            });
        }

        if (!Argon2.Verify(user.Password, model.Password))
        {
            return BadRequest(new
            {
                message = "Incorrect email or password."
            });
        }

        return Ok(new
        {
            token = tokenService.GenerateToken(user)
        });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get([FromServices] MongoService mongoService)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Select(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.Email,
                x.Confirmed,
                x.Verified,
                x.Username,
                x.Birthdate,
                x.Private,
                x.CanvaPhoto,
                x.ProfilePhoto
            })
            .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
        if(user == null)
        {
            return Unauthorized(new
            {
                message = "Invalid token."
            });
        }

        var account = await mongoService.Account.GetAsync(user.Id);
        if (account == null)
        {
            account = new Account
            {
                UserId = user.Id
            };
            await mongoService.Account.CreateAsync(account);
        }

        return Ok(new
        {
            user,
            account
        });
    }
}