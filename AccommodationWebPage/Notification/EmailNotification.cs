using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace AccomodationWebPage
{
    /// <summary>
    /// Klasa do powiadomień mailowych
    /// </summary>
    public static class EmailNotification
    {

        private static string username = "quickaccommodationnoreply";
        private static string password = "123QANR123";
        private static string subject1 = "Powiadomienie o rezerwacji oferty";
        private static string subject2 = "Powiadomienie o rezygnacji z oferty";
        private static string body1 = "Pozdrawiamy, zespół QuickAccommodation";
        private static string body2 = "Powyższa wiadomość została wygenerowana autmatycznie, prosimy nie odpowiadać";


        public static bool SendNotification(OfferInfo offerInfo, Place place, UserData vendor, UserData customer, Room room, bool rezervation)
        {

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("quickaccommodationnoreply@gmail.com");
            mail.To.Add(vendor.Email);


            var sb = new StringBuilder();
            sb.AppendFormat("Witaj, {0}", vendor.FirstName);
            sb.AppendLine();
            if (rezervation)
            {
                mail.Subject = subject1;
                sb.AppendFormat("Użytkownik {0} {1} zarezerwował ofertę:", customer.FirstName, customer.LastName);
            }
            else
            {
                sb.AppendFormat("Użytkownik {0} {1} zrezygnował z  oferty:", customer.FirstName, customer.LastName);
                mail.Subject = subject2;
            }
            sb.AppendLine();
            sb.AppendFormat("{0}, pokój nr {1}, od {2} do {3}", place.PlaceName, room.Number, offerInfo.OfferStartTime.Date.ToString("dd/MM/yyyy"), offerInfo.OfferEndTime.Date.ToString("dd/MM/yyyy"));
            sb.AppendLine();
            sb.AppendLine();
            sb.Append(body1);
            sb.AppendLine();
            sb.Append(body2);

            mail.Body = sb.ToString();

            SmtpServer.Port = 587;

            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;

            SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            return true;
        }
    }
}