using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace AccomodationWebApi
{
    public static class EmailNotification
    {

        private static string username = "quickaccommodationnoreply";
        private static string password = "123QANR123"; //to chyba jest słabe, żeby tu trzymać hasło 
        private static string subject = "Powiadomienie o rezerwacji oferty";
        private static string body1 = "Pozdrawiamy, zespół QuickAccommodation";
        private static string body2 = "Powyższa wiadomośc została wygenerowana autmatycznie, prosimy nie odpowiadać";


        public static bool SendReservationNotification(OfferInfo offerInfo,Place place, UserData userTo, UserData customer)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("quickaccommodationnoreply@gmail.com");
                mail.To.Add(userTo.Email);
                mail.Subject = subject;

                var sb = new StringBuilder();
                sb.AppendFormat("Witaj, {0}",userTo.FirstName);
                sb.AppendLine();
                sb.AppendFormat("Użytkownik {1} {2} zarezerwował ofertę:", customer.FirstName, customer.LastName);
                sb.AppendLine();
                sb.AppendFormat("{0}, od {1} do {2}", place.PlaceName, offerInfo.OfferStartTime, offerInfo.OfferEndTime);
                sb.AppendLine();
                sb.Append(body1);
                sb.AppendLine();
                sb.Append(body2);

                mail.Body = sb.ToString();

                SmtpServer.Port = 587;

                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;

                SmtpServer.Credentials = new System.Net.NetworkCredential(username,password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
            return true;
        }
    }
}