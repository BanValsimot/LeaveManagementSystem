using System.Net.Mail;
// Provides classes for working with email (MailMessage, SmtpClient, etc.)

namespace LeaveManagementSystem.Web.Services;

// EmailSender implements IEmailSender (used by ASP.NET Identity for sending emails)
public class EmailSender(IConfiguration _configuration) : IEmailSender
{
    // Method used by Identity (e.g., email confirmation, password reset)
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Read values from appsettings.json
        // IConfiguration allows access to configuration keys

        var fromAddress = _configuration["EmailSettings:DefaultEmailAddress"];
        // Email address that appears as the sender (e.g., no-reply@localhost.com)

        var smtpServer = _configuration["EmailSettings:Server"];
        // SMTP server (e.g., localhost for PaperCut, or smtp.gmail.com)

        var smtpPort = Convert.ToInt32(_configuration["EmailSettings:Port"]);
        // Port number (e.g., 25 for local SMTP, 587 for TLS)

        // Create email message object
        var message = new MailMessage
        {
            // From must be a MailAddress object (not a string)
            // That’s why we wrap fromAddress inside new MailAddress(...)
            From = new MailAddress(fromAddress),

            // Subject of the email
            Subject = subject,

            // Body content (HTML string)
            Body = htmlMessage,

            // Indicates that Body contains HTML (not plain text)
            IsBodyHtml = true
        };

        // Add recipient email address
        message.To.Add(new MailAddress(email));

        // Create SMTP client (used to send email)
        using var client = new SmtpClient(smtpServer, smtpPort);
        // using → ensures resources are disposed after sending

        // Send email asynchronously (non-blocking)
        await client.SendMailAsync(message);
    }
}