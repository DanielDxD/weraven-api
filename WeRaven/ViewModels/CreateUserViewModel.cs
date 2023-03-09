namespace WeRaven.ViewModels;

public class CreateUserViewModel : DefaultUserViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public string Birthdate { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}