using FirstFiorellaMVC.Models;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace FirstFiorellaMVC.Data
{
    public static class MyEmailUtil
    {
        public static bool SendEmail(User reciever, string link)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("codep320@gmail.com", "Fiorello");
                msg.To.Add(reciever.Email);
                string body = string.Empty;
                using (StreamReader reader = new StreamReader("wwwroot/template/verifyemail.html"))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{link}}", link);
                body = body.Replace("{{name}}", $"Welcome, {reciever.UserName.ToUpper()}");
                msg.Body = body;
                msg.Subject = "Verify";
                msg.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("codep320@gmail.com", "codeacademyp320");
                smtp.Send(msg);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
