using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public class EmailService
{
    private string key = "";
    private  char[] sybChars;

    public async Task SendMessageAsync(string mail)
    {
        sybChars = mail.ToCharArray();
        string mailWithoutAt = new string(sybChars);
        int atIndex = mailWithoutAt.IndexOf('@');

        if (atIndex >= 0)
        {
            mailWithoutAt = mailWithoutAt.Substring(0, atIndex); // Видаляємо все після символа "@"
            mailWithoutAt = Regex.Replace(mailWithoutAt, "[^a-zA-Z]", "");
        }
            sybChars = mailWithoutAt.ToCharArray();

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
        Random random = new Random();
        int digitCount = 0;
        for (int i = 0; i < 7; i++)
        {
            key += sybChars[new Random().Next(0, sybChars.Length)];
        }
        while (digitCount < 4)
        {
            char randomChar = (char)random.Next(48, 58);
            int insertIndex = random.Next(0, key.Length + 1);

            if (insertIndex == key.Length || !char.IsDigit(key[insertIndex]))
            {
                key = key.Insert(insertIndex, randomChar.ToString());
                digitCount++;
            }
        }
        key = key.ToUpper();
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