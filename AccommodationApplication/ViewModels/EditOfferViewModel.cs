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
using System.ComponentModel;
using AccommodationApplication.Model;
using System.Collections.ObjectModel;

namespace AccommodationApplication.ViewModels
{
    public class EditOfferViewModel : CloseableViewModel, IDataErrorInfo
    {

        private string _accommodationName;
        private string _street;
        private string _localNumber;
        private string _postalCode;
        private string _city;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _price;
        private string _availiableVacanciesNumber;
        private string _description;
        private OfferValidator ov = new OfferValidator();

        public EditOfferViewModel(DisplayableOffer displayableOffer, OffersViewModel ovm)
        {
            Description = displayableOffer.Description;
            AvailiableVacanciesNumber = displayableOffer.AvailableVacanciesNumber.ToString();
            EndDate = displayableOffer.OfferEndTimeDate;
            StartDate = displayableOffer.OfferStartTimeDate;
            AccommodationName = displayableOffer.PlaceName;
            Price = displayableOffer.Price.ToString();
            Street = displayableOffer.Address.Street;
            LocalNumber = displayableOffer.Address.LocalNumber;
            City = displayableOffer.Address.City;
            PostalCode = displayableOffer.Address.PostalCode;
            UpDateCommand = new DelegateCommand(async x => await UpDateAsync());
            Id = displayableOffer.Id;
            Ovm = ovm;
        }

        public async virtual Task UpDateAsync()
        {
            await Task.Run(() => UpDate());
        }

        public ICommand UpDateCommand { get; set; }

        OffersViewModel Ovm;

        public void UpDate()
        {

            Address address = new Address()
            {
                City = this.City,
                Street = this.Street,
                LocalNumber = this.LocalNumber,
                PostalCode = this.PostalCode
            };

            OfferInfo offerInfo = new OfferInfo()
            {
                OfferStartTime = TimeZoneInfo.ConvertTimeToUtc(this.StartDate),
                OfferEndTime = TimeZoneInfo.ConvertTimeToUtc(this.EndDate),
                Description = this.Description,
                Price = double.Parse(this.Price),
                AvailableVacanciesNumber = int.Parse(this.AvailiableVacanciesNumber),
                OfferPublishTime = DateTime.UtcNow
            };

            Place place = new Place()
            {
                PlaceName = this.AccommodationName,
                Address = address
            };

            Offer ao = new Offer();

            using (var context = new AccommodationContext())
            {
                using (var scope = new TransactionScope())
                {

                    string currentUser = Thread.CurrentPrincipal.Identity.Name;
                    if (currentUser == null) return;
                    User user = context.Users.FirstOrDefault(x => x.Username.Equals(currentUser));
                    Offer offer = context.Offers.FirstOrDefault(x => x.Id == this.Id);

                    if (offer == null) return;

                    offer.OfferInfo = offerInfo;
                    place.Address = address;
                    offer.Place = place;


                    context.SaveChanges();
                    scope.Complete();
                }
            }

            Ovm.Load();
            Close();
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


        public string AvailiableVacanciesNumber
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
                return _startDate.Date;
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
                return _endDate.Date;
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
                return "Dodaj nową ofertę ";
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

        public string Error
        {
            get
            {
                return String.Empty;
            }
        }

        public int Id { get; set; }



        public string this[string columnName]
        {
            get
            {
                String errorMessage = String.Empty;
                OfferValidator ov = new OfferValidator();
                switch (columnName)
                {
                    case "AccommodationName":
                        if (string.IsNullOrWhiteSpace(this.AccommodationName))
                        {
                            errorMessage = "Nazwa nie może być pusta";
                        }
                        break;
                    case "Street":
                        if (string.IsNullOrWhiteSpace(this.Street))
                        {
                            errorMessage = "Ulica nie może być pusta";
                        }
                        break;
                    case "LocalNumber":
                        if (string.IsNullOrWhiteSpace(this.LocalNumber))
                        {
                            errorMessage = "Numer Lokalu nie może być pusty";
                        }
                        if (!ov.ValidateLocalNumber(this.LocalNumber))
                        {
                            errorMessage = "Nieprawidłowy numer lokalu";
                        }
                        break;
                    case "PostalCode":
                        if (string.IsNullOrWhiteSpace(this.PostalCode))
                        {
                            errorMessage = "Kod pocztowy nie może być pusty";
                        }
                        if (!ov.ValidatePostalCode(this.PostalCode))
                        {
                            errorMessage = "Nieprawidłowy kod pocztowy";
                        }
                        break;
                    case "City":
                        if (string.IsNullOrEmpty(this.City))
                        {
                            errorMessage = "Nieprawidłowa nazwa miasta";
                        }
                        break;
                    case "StartDate":
                        if (!ov.ValidateDate(this.StartDate, this.EndDate))
                        {
                            errorMessage = "Nieprawidłowe daty";
                        }
                        break;
                    case "EndDate":
                        if (!ov.ValidateDate(this.StartDate, this.EndDate))
                        {
                            errorMessage = "Nieprawidłowe daty";
                        }
                        break;
                    case "Price":
                        if (!ov.ValidatePrice(this.Price))
                        {
                            errorMessage = "Nieprawidłowa cena";
                        }
                        break;
                    case "AvailiableVacanciesNumber":
                        if (!ov.ValidateNumber(this.AvailiableVacanciesNumber))
                        {
                            errorMessage = "Nieprawidłowa liczba wolnych miejsc";
                        }
                        break;
                    case "Description":
                        if (string.IsNullOrEmpty(this.Description))
                        {
                            errorMessage = "Dodaj opis";
                        }
                        break;
                }
                return errorMessage;
            }
        }
    }
}
