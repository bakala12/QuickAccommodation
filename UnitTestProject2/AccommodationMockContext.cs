using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace UnitTestProject2
{
    public class AccommodationMockContext : IAccommodationContext
    {
        private readonly MockDbSet<User> _users;
        private readonly MockDbSet<Offer> _offers;
        private readonly MockDbSet<OfferInfo> _offerInfo;
        private readonly MockDbSet<Address> _addreses;
        private readonly MockDbSet<UserData> _userData;
        private readonly MockDbSet<Place> _places;
        private readonly MockDbSet<Room> _rooms;
        private readonly MockDbSet<Rank> _ranks;
        private readonly MockDbSet<HistoricalOffer> _historicalOffers;

        public AccommodationMockContext()
        {
            List<User> users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Username = "bakalam",
                    HashedPassword = @"/TmCAkG+Rl0kJd+GvhgbVYghlOY3RXPFcRXLpx69sPk=",
                    Salt = @"jQcO9f74p4sySJrXISk2XBw689JAPAic"
                },
                new User()
                {
                    Id=2,
                    Username = "jablonskim",
                    HashedPassword = @"PIks6KBVlB0pE5DvlSmKpxKCs5tN9EDebq6s8bqb4HA=",
                    Salt = @"zapt0VgkYldvUEc7K3TJ5S0aRLDavxmN"
                },
                 new User()
                {
                    Id=3,
                    Username = "jablonskim2",
                    HashedPassword = @"PIks6KBVlB0pE5DvlSmKpxKCs5tN9EDebq6s8bqb4HA=",
                    Salt = @"zapt0VgkYldvUEc7K3TJ5S0aRLDavxmN"
                }
            };
            List<UserData> userData = new List<UserData>()
            {
                new UserData()
                {
                    Id=1,
                    Email = "bakala12@o2.pl",
                    FirstName = "Mateusz",
                    LastName = "Bąkała"
                },
                new UserData()
                {
                    Id=2,
                    Email = "jablonskim2@student.mini.pw.edu.pl",
                    FirstName = "Mateusz",
                    LastName = "Jabłoński"
                }
            };
            users[0].UserDataId = 1;
            users[1].UserDataId = 2;
            users[0].UserData = userData[0];
            users[1].UserData = userData[1];

            List<Address> addresses = new List<Address>()
            {
                new Address()
                {
                    City = "Warszawa",
                    Id = 1,
                    PostalCode = "11-111",
                    LocalNumber = "12a",
                    Street = "Puławska"
                }
            };
            List<Place> places = new List<Place>()
            {
                new Place() {PlaceName = "Pensjonat pod różą", Id = 1, Address = addresses[0], AddressId = addresses[0].Id}
            };

            List<Room> rooms = new List<Room>()
            {
                new Room() {Id = 1, Place = places[0], Capacity = 2, Number = "101", PlaceId = 1},
                new Room() {Id = 2, Place = places[0], PlaceId = places[0].Id, Capacity = 3, Number = "102"}
            };

            List<OfferInfo> offerInfos = new List<OfferInfo>()
            {
                new OfferInfo()
                {
                    Id=1,
                    OfferStartTime = new DateTime(2015,12,20),
                    OfferEndTime = new DateTime(2015,12,23),
                    OfferPublishTime = new DateTime(2015,12,13),
                    Price = 400,
                },
                new OfferInfo()
                {
                    Id=2,
                    OfferStartTime = new DateTime(2015,12,20),
                    OfferEndTime = new DateTime(2015,12,23),
                    OfferPublishTime = new DateTime(2015,12,13),
                    Price = 300,
                },
                new OfferInfo()
                {
                    Id=3,
                    OfferStartTime = new DateTime(2015,12,26),
                    OfferEndTime = new DateTime(2015,12,30),
                    OfferPublishTime = new DateTime(2015,12,13),
                    Price = 500,
                }
            };

            List<Offer> offers = new List<Offer>()
            {
                new Offer()
                {
                    Vendor = users[1],
                    VendorId = 2,
                    Id=1,
                    Room = rooms[0],
                    RoomId = rooms[0].Id,
                    OfferInfo = offerInfos[0],
                    OfferInfoId = 1,
                    IsBooked = false
                },
                new Offer()
                {
                    Vendor = users[0],
                    VendorId = 1,
                    Id=2,
                    Room = rooms[1],
                    RoomId = rooms[1].Id,
                    OfferInfo = offerInfos[1],
                    OfferInfoId = 2,
                     IsBooked = false
                },
                new Offer()
                {
                    Vendor = users[1],
                    VendorId = 2,
                    Id=3,
                    Room = rooms[1],
                    RoomId = rooms[1].Id,
                    OfferInfo = offerInfos[2],
                    OfferInfoId = 3,
                     IsBooked = true,
                     CustomerId = 2
                },
                new Offer()
                {
                    Vendor = users[0],
                    VendorId = 1,
                    Id=4,
                    Room = rooms[0],
                    RoomId = rooms[0].Id,
                    OfferInfo = offerInfos[0],
                    OfferInfoId = 1,
                    IsBooked = false
                }
            };
            List<HistoricalOffer> historical = new List<HistoricalOffer>()
            {
                new HistoricalOffer()
                {
                    Vendor = users[0],
                    VendorId = 1,
                    Id=1,
                    Room = rooms[0],
                    RoomId = rooms[0].Id,
                    OfferInfo = offerInfos[0],
                    OfferInfoId = 1,
                    OriginalOffer = offers[0],
                    OriginalOfferId = 1
                },
                new HistoricalOffer()
                {
                    Vendor = users[0],
                    VendorId = 1,
                    Id=2,
                    Room = rooms[1],
                    RoomId = rooms[1].Id,
                    OfferInfo = offerInfos[1],
                    OfferInfoId = 2,
                    OriginalOffer = offers[1],
                    OriginalOfferId = 2
                },
                new HistoricalOffer()
                {
                    Vendor = users[1],
                    VendorId = 2,
                    Id=3,
                    Room = rooms[1],
                    RoomId = rooms[1].Id,
                    OfferInfo = offerInfos[2],
                    OfferInfoId = 3,
                    OriginalOffer = offers[2],
                    OriginalOfferId = 3
                }
            };
            List<Rank> ranks = new List<Rank>()
            {
                new Rank() {Name = "Nowicjusz"},
                new Rank() {Name = "Junior"},
                new Rank() {Name = "Znawca"},
                new Rank() {Name = "Mistrz"},
                new Rank() {Name = "Guru"}
            };
            users[0].Rank = ranks[0];
            users[1].Rank = ranks[0];
            _users = new MockDbSet<User>(users);
            _offerInfo = new MockDbSet<OfferInfo>(offerInfos);
            _addreses = new MockDbSet<Address>(addresses);
            _userData = new MockDbSet<UserData>(userData);
            _offers = new MockDbSet<Offer>(offers);
            _historicalOffers = new MockDbSet<HistoricalOffer>(historical);
            _ranks = new MockDbSet<Rank>(ranks);
            _rooms = new MockDbSet<Room>(rooms);
            _places = new MockDbSet<Place>(places);
        }

        public IDbSet<User> Users => _users.Set.Object;
        public IDbSet<Offer> Offers => _offers.Set.Object;
        public IDbSet<OfferInfo> OfferInfo => _offerInfo.Set.Object;
        public IDbSet<Address> Addresses => _addreses.Set.Object;
        public IDbSet<UserData> UserData => _userData.Set.Object;
        public IDbSet<Place> Places => _places.Set.Object;
        public IDbSet<Rank> Ranks => _ranks.Set.Object;
        public IDbSet<Room> Rooms => _rooms.Set.Object;
        public IDbSet<HistoricalOffer> HistoricalOffers => _historicalOffers.Set.Object;

        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose()
        {

        }
    }
}
