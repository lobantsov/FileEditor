using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

public class EmailService
{
    private string key = "";
    private readonly char[] sybChars =
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
        'v', 'w', 'x', 'y', 'z'
    };

    public async Task SendMessageAsync(string mail)
    {
        var message = new MailMessage
        {
            From = new MailAddress("lobanszov.aleksei2@gmail.com", "Sender"),
            Subject = "Confirm your email!",
            Body = $"Your password for confirmation:\n{CreateKey()}"
        };

        message.To.Add(new MailAddress(mail));

        using (var smtpClient = new SmtpClient("smtp.gmail.com"))
        {
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("lobanszov.aleksei2@gmail.com", "mijz ljhc xpvt zilw");
            smtpClient.EnableSsl = true;

            try
            {
                await smtpClient.SendMailAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private string CreateKey()
    {
        for (int i = 0; i < 10; i++)
        {
            key += sybChars[new Random().Next(0, sybChars.Length)];
        }
        return key;
    }

    public bool CheckPasswords(string password)
    {
        return password == key ? true : false;
    }

    public byte[] HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            return hashBytes;
        }
    }
}