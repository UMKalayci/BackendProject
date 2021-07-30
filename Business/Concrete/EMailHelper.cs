using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class EMailHelper:IEmailHelper
    {
        public IDataResult<string> MailConfirmation(string mail, string link)
        {
            var fromAddress = new MailAddress("bmugurmutlukalayci@gmail.com", "Uğur Mutlu KALAYCI");
            var toAddress = new MailAddress(mail, "E Gönüllü");
            const string fromPassword = "Qazwsx112++";
            const string subject = "E Gönüllü Kayıt Onaylama";
            string body = "Onay adresiniz : " + link;

            try
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
                return new SuccessDataResult<string>(Messages.MailSuccess);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<string>(Messages.MailError);
            }
        }

        public async Task AdvertisementMail(string mail, string title)
        {
            var fromAddress = new MailAddress("bmugurmutlukalayci@gmail.com", "Uğur Mutlu KALAYCI");
            var toAddress = new MailAddress(mail, "E Gönüllü");
            const string fromPassword = "Qazwsx112++";
            const string subject = "E Gönüllü Yeni Proje İlanı";
            string body = "Merhabalar, yeni yayınlanan ilanımıza bakmak ister misiniz ? İlan Başlığı :"+title;

            try
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
              
            }
        }
    }
}
