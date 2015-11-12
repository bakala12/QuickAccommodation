using AccommodationApplication.Commands;
using AccommodationApplication.Interfaces;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Transactions;
using System.Threading;

namespace AccommodationApplication.ViewModels
{
    public class AddNewOfferViewModel : ViewModelBase, IPageViewModel
    {

        private string _accommodationName;
        private string _street;
        private string _localNumber;
        private string _postalCode;
        private string _city;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _price;
        private int _availiableVacanciesNumber;
        private string _description;

        public AddNewOfferViewModel()
        {
            _startDate = DateTime.Now;
            _endDate = DateTime.Now;

            AddCommand = new DelegateCommand(async x => await AddAsync());
            
        }

        public async virtual Task AddAsync()
        {
            await Task.Run(() => Add());
        }

        public ICommand AddCommand { get; set; }
        

     
        public void Add()
        {
            OfferValidator ov = new OfferValidator();
            if (ov.ValidateDate(this.StartDate, this.EndDate) || ov.ValidateLocalNumber(this.LocalNumber) ||
                 ov.ValidateName(this.AccommodationName) || ov.ValidatePostalCode(this.PostalCode) || ov.ValidatePrice(this.Price))
            {
                Address address = new Address()
                {
                    Name = this.AccommodationName,
                    City = this.City,
                    Street = this.Street,
                    LocalNumber = this.LocalNumber,
                    PostalCode = this.PostalCode
                };
                OfferInfo offer = new OfferInfo()
                {
                    Address = address,
                    OfferStartTime = TimeZoneInfo.ConvertTimeToUtc(this.StartDate),
                    OfferEndTime = TimeZoneInfo.ConvertTimeToUtc(this.EndDate),
                    Description = this.Description,
                    Price = double.Parse(this.Price),
                    AvailableVacanciesNumber = this.AvailiableVacanciesNumber,
                    OfferPublishTime = DateTime.UtcNow
                };

                AvailableOffer ao = new AvailableOffer();
              

                string currentUser = Thread.CurrentPrincipal.Identity.Name;

                using (var context = new AccommodationContext())
                {
                    using (var scope = new TransactionScope())
                    {
                        User user = context.Users.FirstOrDefault(x => x.Username.Equals(currentUser));
                        ao.OfferInfo = offer;
                        ao.Vendor = user;
                        user.MyOffers.Add(ao);
                        context.AvailableOffers.Add(ao);
                        context.SaveChanges();
                        scope.Complete();
                    }
                }




            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }


        public int AvailiableVacanciesNumber
        {
            get
            {
                return _availiableVacanciesNumber;
            }
            set
            {
                _availiableVacanciesNumber = value;
                OnPropertyChanged();
            }
        }

        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }


        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        public string AccommodationName
        {
            get
            {
                return _accommodationName;
            }
            set
            {
                _accommodationName = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return "Add new offer";
            }
        }

        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                OnPropertyChanged();
            }
        }

        public string LocalNumber
        {
            get { return _localNumber; }
            set
            {
                _localNumber = value;
                OnPropertyChanged();
            }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

    }
}
