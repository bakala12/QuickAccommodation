﻿using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationWebPage.Models;
using AccommodationWebPage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AccomodationWebPage;

namespace AccommodationWebPage.DataAccess
{

    /// <summary>
    /// Odpowiada za operacje związane z ofertami w bazie 
    /// </summary>
    public static class OfferAccessor
    {
        /// <summary>
        /// Zapisuje daną ofertę w bazie
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="model">Model z danymi oferty</param>
        /// <param name="username">Aktualnie zalogowany użytkownik</param>
        /// <param name="image">Opcjonalny obrazek</param>
        /// <returns></returns>
        public static bool SaveOffer(IAccommodationContext context, AddNewOfferViewModel model, string username, HttpPostedFileBase image = null)
        {
            try
            {

                Offer offerToAdd = new Offer();

                User user = context.Users.FirstOrDefault(x => x.Username == username);
                if (user == null) return false;

                offerToAdd.Vendor = user;
                offerToAdd.OfferInfo = new OfferInfo
                {
                    OfferStartTime = model.StartDate,
                    OfferEndTime = model.EndDate,
                    OfferPublishTime = DateTime.Now,
                    Price = double.Parse(model.Price),
                    Description = model.Description
                };

                if (image != null)
                {
                    offerToAdd.OfferInfo.OfferImage = new byte[image.ContentLength];
                    image.InputStream.Read(offerToAdd.OfferInfo.OfferImage, 0, image.ContentLength);
                }

                Place place = context.Places.FirstOrDefault(p => p.PlaceName.Equals(model.AccommodationName) &&
                                                                 p.Address.City.Equals(model.City) &&
                                                                 p.Address.Street.Equals(model.Street) &&
                                                                 p.Address.LocalNumber == model.LocalNumber);
                if (place != null)
                {
                    //istnieje to miejsce w bazie danych
                    Room room = context.Rooms.FirstOrDefault(r => r.PlaceId == place.Id && r.Number == model.RoomNumber);
                    if (room != null)
                    {
                        //istnieje oferta na ten pokój
                        List<Offer> off =
                            context.Offers.Where(offer => offer.RoomId == room.Id)
                                .Include(o => o.OfferInfo)
                                .ToList();
                        if (off.Any(offer => (offer.OfferInfo.OfferStartTime <= model.StartDate &&
                                              offer.OfferInfo.OfferEndTime >= model.EndDate) ||
                                             (offer.OfferInfo.OfferStartTime >= model.StartDate &&
                                              offer.OfferInfo.OfferEndTime <= model.EndDate)))
                        {
                            return false;
                        }
                        //żadna oferta nie koliduje
                        offerToAdd.Room = room;
                    }
                    else
                    {
                        //nowy pokój
                        Room newRoom = new Room
                        {
                            Capacity = int.Parse(model.AvailiableVacanciesNumber),
                            Number = model.RoomNumber,
                            Place = place,
                        };
                        offerToAdd.Room = newRoom;
                        offerToAdd.Room.Place = place;
                    }
                }
                else
                {
                    Address newAddress = new Address
                    {
                        City = model.City,
                        Street = model.Street,
                        LocalNumber = model.LocalNumber,
                        PostalCode = model.PostalCode,
                    };

                    Place newPlace = new Place
                    {
                        Address = newAddress,
                        PlaceName = model.AccommodationName,
                    };

                    Room newRoom = new Room
                    {
                        Capacity = int.Parse(model.AvailiableVacanciesNumber),
                        Number = model.RoomNumber,
                        Place = newPlace,
                    };

                    offerToAdd.Room = newRoom;
                }
                user.MyOffers.Add(offerToAdd);

                //zapisanie do historii
                HistoricalOffer historicalOffer = new HistoricalOffer();
                historicalOffer.OfferInfo = offerToAdd.OfferInfo;
                historicalOffer.Vendor = offerToAdd.Vendor;
                historicalOffer.Room = offerToAdd.Room;
                historicalOffer.Room.Place = offerToAdd.Room.Place;
                historicalOffer.Room.Place.Address = offerToAdd.Room.Place.Address;
                historicalOffer.OriginalOffer = offerToAdd;

                user.MyHistoricalOffers.Add(historicalOffer);

                //ewentualna zmiana rangi
                int c = user.MyHistoricalOffers.Count;
                if (c >= 4 && c < 8)
                {
                    user.Rank = context.Ranks.FirstOrDefault(r => r.Name.Equals("Junior"));
                }
                else if (c >= 8 && c < 15)
                {
                    user.Rank = context.Ranks.FirstOrDefault(r => r.Name.Equals("Znawca"));
                }
                else if (c >= 15 && c < 25)
                {
                    user.Rank = context.Ranks.FirstOrDefault(r => r.Name.Equals("Mistrz"));
                }
                else if (c >= 25)
                {
                    user.Rank = context.Ranks.FirstOrDefault(r => r.Name.Equals("Guru"));
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Asynchroniczna metoda wywołująca metodę do 
        /// zapisywania oferty w bazie
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="model">Model z danymi oferty</param>
        /// <param name="username">Aktualnie zalogowany użytkownik</param>
        /// <param name="image">Opcjonalny obrazek</param>
        /// <returns></returns>
        public static async Task<bool> SaveOfferAsync(IAccommodationContext context, AddNewOfferViewModel model, string username,
            HttpPostedFileBase image = null)
        {
            return await Task.Run(() => SaveOffer(context, model, username, image));
        }

        /// <summary>
        /// Umożliwia pobranie ofert użytkownika z bazy
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="username">Nazwa zalogowanego użytkownika</param>
        /// <returns>Zwraca listę ofert</returns>
        public static List<OfferViewModel> GetUserOffers(IAccommodationContext context, string username)
        {
            try
            {
                List<Offer> offerList = new List<Offer>();
                List<OfferViewModel> offerViewModelList = new List<OfferViewModel>();

                User user = context.Users.FirstOrDefault(u => u.Username == username);
                offerList = context.Offers.Where(o => o.VendorId == user.Id).Include(o => o.OfferInfo).Include(o => o.Room).ToList();

                foreach (var offer in offerList)
                {
                    offer.Room.Place = context.Places.FirstOrDefault(p => p.Id == offer.Room.PlaceId);
                    offer.Room.Place.Address = context.Addresses.FirstOrDefault(a => a.Id == offer.Room.Place.AddressId);

                    offerViewModelList.Add(new OfferViewModel(offer));
                }

                return offerViewModelList;
            }
            catch (Exception ex)
            {
                return new List<OfferViewModel>();
            }
        }

        /// <summary>
        /// Asynchroniczna metoda wywołująca metodę
        /// do pobierania listy ofert użytkownika
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="username">Nazwa użytkownika</param>
        /// <returns></returns>
        public static async Task<List<OfferViewModel>> GetUserOffersAsync(IAccommodationContext context, string username)
        {
            return await Task.Run(() => GetUserOffers(context, username));
        }


        /// <summary>
        /// Umożliwia pobranie zarezerwwanych ofert użytkownika z bazy
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="username">Nazwa zalogowanego użytkownika</param>
        /// <returns>Zwraca listę zarezerwowanych ofert</returns>
        public static List<OfferViewModel> GetUserReservedOffers(IAccommodationContext context, string username)
        {
            try
            {
                List<Offer> offerList = new List<Offer>();
                List<OfferViewModel> offerViewModelList = new List<OfferViewModel>();

                User user = context.Users.FirstOrDefault(u => u.Username == username);
                offerList = context.Offers.Where(o => o.CustomerId == user.Id).Include(o => o.OfferInfo).Include(o => o.Room).ToList();

                foreach (var offer in offerList)
                {
                    offer.Room.Place = context.Places.FirstOrDefault(p => p.Id == offer.Room.PlaceId);
                    offer.Room.Place.Address = context.Addresses.FirstOrDefault(a => a.Id == offer.Room.Place.AddressId);


                    offerViewModelList.Add(new OfferViewModel(offer));
                }

                return offerViewModelList;
            }
            catch (Exception ex)
            {
                return new List<OfferViewModel>();
            }
        }

        /// <summary>
        /// Asynchroniczna metoda wywołująca metodę
        /// do pobierania listy zarezerwowanych ofert użytkownika
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static async Task<List<OfferViewModel>> GetUserReservedOffersAsync(IAccommodationContext context, string username)
        {
            return await Task.Run(() => GetUserReservedOffers(context, username));
        }


        /// <summary>
        /// Asynchroniczna metoda wywołująca metodę
        /// do pobierania oferty o danym id
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="id">id oferty</param>
        /// <returns></returns>
        public static async Task<AddNewOfferViewModel> GetOfferByIdAsync(IAccommodationContext context, int id)
        {
            return await Task.Run(() => GetOfferById(context, id));
        }

        /// <summary>
        /// Pobiera ofertę o danym id z bazy
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="offerId">id oferty</param>
        /// <returns></returns>
        public static AddNewOfferViewModel GetOfferById(IAccommodationContext context, int offerId)
        {
            try
            {
                Offer offer = context.Offers.Where(o => o.Id == offerId).Include(o => o.OfferInfo).Include(o => o.Room).First();
                if (offer == null) return null;
                Place place = context.Places.FirstOrDefault(p => p.Id == offer.Room.PlaceId);
                Address address = context.Addresses.FirstOrDefault(a => a.Id == offer.Room.Place.AddressId);
                offer.Room.Place = place;
                offer.Room.Place.Address = address;
                return new AddNewOfferViewModel(offer);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Asynchroniczna metoda wywołująca metodę do
        /// edycji ofert w bazie
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="model">Model z danymi oferty do edycji</param>
        /// <returns></returns>
        public static async Task<bool> EditOfferAsync(IAccommodationContext context, AddNewOfferViewModel model)
        {
            return await Task.Run(() => EditOffer(context, model));
        }

        /// <summary>
        /// Umożliwia edycję oferty w bazie
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="model">Model z danymi oferty</param>
        /// <returns></returns>
        public static bool EditOffer(IAccommodationContext context, AddNewOfferViewModel model)
        {
            try
            {

                Offer offer = context.Offers.FirstOrDefault(x => x.Id == model.Id);
                if (offer == null) return false;

                HistoricalOffer ho = context.HistoricalOffers.FirstOrDefault(x => x.OriginalOfferId == offer.Id);

                OfferInfo newOfferInfo = new OfferInfo
                {
                    Description = model.Description,
                    OfferEndTime = model.EndDate,
                    OfferStartTime = model.StartDate,
                    OfferPublishTime = DateTime.Now,
                    Price = double.Parse(model.Price)
                };

                Address newAddress = new Address
                {
                    City = model.City,
                    Street = model.Street,
                    LocalNumber = model.LocalNumber,
                    PostalCode = model.PostalCode,
                };

                Place newPlace = new Place
                {
                    Address = newAddress,
                    PlaceName = model.AccommodationName,
                };

                Room newRoom = new Room
                {
                    Capacity = int.Parse(model.AvailiableVacanciesNumber),
                    Number = model.RoomNumber,
                    Place = newPlace,
                };

                ho.OfferInfo = offer.OfferInfo = newOfferInfo;
                ho.Room = offer.Room = newRoom;
                ho.Room.Place = offer.Room.Place = newPlace;
                ho.Room.Place.Address = offer.Room.Place.Address = newAddress;

                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Asynchroniczna metoda do usuwania ofert
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="id">Id oferty do usunięcia</param>
        /// <param name="username">Nazwa użytkownika</param>
        /// <returns></returns>
        public static async Task<bool> DeleteOfferByIdAsync(IAccommodationContext context, int id, string username)
        {
            return await Task.Run(() => DeleteOfferById(context, id, username));
        }


        /// <summary>
        /// Usuwa ofertę o danym id z bazy
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="offerId">Id oferty</param>
        /// <param name="username">Nazwa użytkownika</param>
        /// <returns></returns>
        public static bool DeleteOfferById(IAccommodationContext context, int offerId, string username)
        {
            try
            {
                Offer offer = context.Offers.FirstOrDefault(x => x.Id == offerId);
                if (offer == null) return false;
                User user = context.Users.FirstOrDefault(u => u.Username == username);

                OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(x => x.Id == offer.OfferInfoId);
                Room room = context.Rooms.FirstOrDefault(x => x.Id == offer.RoomId);
                Place place = context.Places.FirstOrDefault(x => x.Id == room.PlaceId);
                Address address = context.Addresses.FirstOrDefault(x => x.Id == place.AddressId);

                var ho = context.HistoricalOffers.FirstOrDefault(h => h.OriginalOfferId == offer.Id);
                if (ho != null) ho.OriginalOffer = null;
                //usuń z bazy ofertę oraz jej dane

                context.Offers.Remove(offer);
                user?.MyOffers?.Remove(offer);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Asynchroniczna metoda do rezerwacji oferty
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="id">Id oferty</param>
        /// <param name="username">Nazwa użytkownika</param>
        /// <returns></returns>
        public async static Task<bool> ReserveOfferAsync(IAccommodationContext context, int id, string username)
        {
            return await Task.Run(() => ReserveOffer(context, id, username));
        }

        /// <summary>
        /// Rezerwuje ofertę o danym id aktualnie
        /// zalogowanemu użytkownikowi
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="offerId">Id oferty</param>
        /// <param name="username">Nazwa użytkownika</param>
        /// <returns></returns>
        public static bool ReserveOffer(IAccommodationContext context, int offerId, string username)
        {
            try
            {
                Offer offer = context.Offers.FirstOrDefault(o => o.Id == offerId);
                HistoricalOffer historicalOffer = context.HistoricalOffers.FirstOrDefault(ho => ho.OriginalOfferId == offer.Id);
                User customer = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                User vendor = context.Users.FirstOrDefault(x => x.Id == offer.VendorId);

                OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(o => o.Id == offer.OfferInfoId);
                UserData customerData = context.UserData.FirstOrDefault(x => x.Id == customer.UserDataId);
                UserData vendorData = context.UserData.FirstOrDefault(x => x.Id == vendor.UserDataId);
                Room room = context.Rooms.FirstOrDefault(x => x.Id == offer.RoomId);
                Place place = context.Places.FirstOrDefault(x => x.Id == room.PlaceId);

                if (offer == null || customer == null) return false;
                if (offer.IsBooked) return false;

                offer.IsBooked = true;
                historicalOffer.IsBooked = true;
                historicalOffer.Customer = customer;
                offer.Customer = customer;
                context.SaveChanges();

                //Wysłanie powiadomienia mailowego, ostatni parametr oznacza rezerwację
                EmailNotification.SendNotification(offerInfo, place, vendorData, customerData, room, true);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Asynchroniczna metoda do rezygnacji z oferty
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="id">Id oferty</param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async static Task<bool> ResignOfferAsync(IAccommodationContext context, int id, string username)
        {
            return await Task.Run(() => ResignOffer(context, id, username));
        }



        /// <summary>
        /// Umożliwia rezygnacje z oferty danemu użytkownikowi
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="offerId">Id oferty</param>
        /// <param name="username">Nazwa użytkownika</param>
        /// <returns></returns>
        public static bool ResignOffer(IAccommodationContext context, int offerId, string username)
        {
            try
            {
                Offer offer = context.Offers.FirstOrDefault(o => o.Id == offerId);
                HistoricalOffer historicalOffer = context.HistoricalOffers.FirstOrDefault(ho => ho.OriginalOfferId == offer.Id);
                User customer = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                User vendor = context.Users.FirstOrDefault(x => x.Id == offer.VendorId);

                if (offer == null || customer == null) return false;
                if (!offer.IsBooked) return false;


                OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(o => o.Id == offer.OfferInfoId);
                UserData customerData = context.UserData.FirstOrDefault(x => x.Id == customer.UserDataId);
                UserData vendorData = context.UserData.FirstOrDefault(x => x.Id == vendor.UserDataId);
                Room room = context.Rooms.FirstOrDefault(x => x.Id == offer.RoomId);
                Place place = context.Places.FirstOrDefault(x => x.Id == room.PlaceId);
                offer.IsBooked = false;
                offer.Customer = null;
                historicalOffer.IsBooked = false;
                historicalOffer.Customer = null;
                context.SaveChanges();

                //Wysłanie powiadomienia mailowego, ostatni parametr oznacza rezygnację
                EmailNotification.SendNotification(offerInfo, place, vendorData, customerData, room, false);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async static Task<List<UserMarksViewModel>> GetUsersToMarkAsync(IAccommodationContext context, string username)
        {
            return await Task.Run(() => GetUsersToMark(context, username));
        }


        public static List<UserMarksViewModel> GetUsersToMark(IAccommodationContext context, string username)
        {
            try
            {
                List<UserMarksViewModel> list = new List<UserMarksViewModel>();

                User user = context.Users.FirstOrDefault(u => u.Username == username);
                if (user == null) return new List<UserMarksViewModel>();

                var offersList = context.HistoricalOffers.Where(o => o.CustomerId == user.Id).Include(o => o.OfferInfo).Include(o => o.Room);

                foreach (var offer in offersList)
                {
                    if (!offer.IsMarked)
                    {
                        User vendor = context.Users.FirstOrDefault(u => u.Id == offer.VendorId);
                        Place place = context.Places.FirstOrDefault(x => x.Id == offer.Room.PlaceId);

                        list.Add(new UserMarksViewModel(offer, vendor));
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                return new List<UserMarksViewModel>();
            }
        }

        public async static Task<bool> GiveUserMarkAsync(IAccommodationContext context, MarkViewModel model)
        {
            return await Task.Run(() => GiveUserMark(context, model));
        }

        public static bool GiveUserMark(IAccommodationContext context, MarkViewModel model)
        {
            try
            {
                User user = context.Users.FirstOrDefault(u => u.Username == model.Username);
                HistoricalOffer offer = context.HistoricalOffers.FirstOrDefault(o => o.Id == model.ReservedOfferId);
                int mark = (int)model.mark;

                if(user.AverageMark == null || user.MarkCount == null)
                {
                    user.AverageMark = 0;
                    user.MarkCount = 0;
                }

                user.AverageMark = (user.AverageMark * user.MarkCount + mark) / (user.MarkCount + 1);
                user.MarkCount += 1;

                offer.IsMarked = true;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}