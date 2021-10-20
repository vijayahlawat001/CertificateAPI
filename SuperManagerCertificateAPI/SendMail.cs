using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SuperManagerCertificateAPI
{
    public class SendMail
    {
        public static void SendEmail(string EmailTo, string CC, string BCC, string Subject, string Body, [Optional] FileInfo fileInfo)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("ajaxparmar@gmail.com");
                if (EmailTo != "")
                {
                    foreach (var address in EmailTo.Split(','))
                    {
                        if (address != "")
                        {
                            message.To.Add(new MailAddress(address.Trim(), ""));
                        }
                    }
                }
                if (CC != "")
                {
                    foreach (var address in CC.Split(','))
                    {
                        if (address != "")
                        {
                            message.CC.Add(new MailAddress(address.Trim(), ""));
                        }
                    }
                }
                if (BCC != "")
                {
                    foreach (var address in BCC.Split(','))
                    {
                        if (address != "")
                        {
                            message.CC.Add(new MailAddress(address.Trim(), ""));
                        }
                    }
                }
                Attachment attach = new Attachment(fileInfo.FullName);
                message.Attachments.Add(attach);
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.Body = Body;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("ajaxparmar@gmail.com", "9034402793");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                fileInfo = null;
                attach.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
