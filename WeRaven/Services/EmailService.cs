using WeRaven.Tools;
using System.Net;
using System.Net.Mail;
using Amethyst;


namespace WeRaven.Services;

public class EmailService
{
    private string _body = "";
    public virtual async Task CompileAsync<T>(string templateName, T model)
    {
        var appRoot = EnvTool.IsDebug() ? Directory.GetCurrentDirectory() : AppDomain.CurrentDomain.BaseDirectory;
        var fileTemplate = await File.ReadAllTextAsync(Path.Combine(appRoot, "Templates", $"{templateName}.html"));
        var mailCompiler = new MailCompiler<T>(fileTemplate);
        _body = mailCompiler.Compile(model);
    }
    public virtual bool Send(
        string toName,
        string toEmail,
        string subject,
        string fromName = "Equipe WeRaven",
        string fromEmail = "no-reply@weraven.net")
    {
        var smtpClient = new SmtpClient(EnvTool.Smtp.Host, EnvTool.Smtp.Port);

        smtpClient.Credentials = new NetworkCredential(EnvTool.Smtp.UserName, EnvTool.Smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;
        var mail = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName)
        };
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = _body;
        mail.IsBodyHtml = true;
        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}