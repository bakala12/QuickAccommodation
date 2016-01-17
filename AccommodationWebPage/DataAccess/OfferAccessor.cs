using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationWebPage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AccommodationWebPage.DataAccess
{
    public static class OfferAccessor
    {

        public static bool SaveOffer(IAccommodationContext context, AddNewOfferViewModel model, string username)
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



        public static async Task<bool> SaveOfferAsync(IAccommodationContext context, AddNewOfferViewModel model, string username)
        {
            return await Task.Run(() => SaveOffer(context, model, username));
        }

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

        public static async Task<List<OfferViewModel>> GetUserOffersAsync(IAccommodationContext context, string username)
        {
            return await Task.Run(() => GetUserOffers(context, username));
        }



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

        public static async Task<List<OfferViewModel>> GetUserReservedOffersAsync(IAccommodationContext context, string username)
        {
            return await Task.Run(() => GetUserReservedOffers(context, username));
        }


        public static async Task<AddNewOfferViewModel> GetOfferByIdAsync(IAccommodationContext context, int id)
        {
            return await Task.Run(() => GetOfferById(context, id));
        }

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

        public static async Task<bool> EditOfferAsync(IAccommodationContext context, AddNewOfferViewModel model)
        {
            return await Task.Run(() => EditOffer(context, model));
        }

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
    }
}