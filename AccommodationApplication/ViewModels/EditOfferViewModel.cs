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
using System.Windows;
using AccommodationApplication.Services;

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// ViewModel dla edycji ofert
    /// </summary>
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
        private OffersProxy offersProxy;
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
            Id = displayableOffer.Id;
            RoomNumber = displayableOffer.RoomNumber;
            Ovm = ovm;
            UpDateCommand = new DelegateCommand(x => UpDate());
            offersProxy = new OffersProxy();
        }

        /// <summary>
        /// Asychronicznie uaktualnia ofertę w bazie
        /// </summary>
        /// <returns></returns>
        public async virtual Task UpDateAsync()
        {
            await Task.Run(() => UpDate());
        }

        /// <summary>
        /// Komenda do uaktualniania ofert
        /// </summary>
        public ICommand UpDateCommand { get; set; }

        /// <summary>
        /// Aktualny ViewModel dla ofert, służy do uaktualniania bieżącej listy ofert
        /// </summary>
        OffersViewModel Ovm;

        /// <summary>
        /// Funkcja aktualizująca ofertę w bazie
        /// </summary>
        public async void UpDate()
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
                OfferPublishTime = DateTime.UtcNow
            };

            Place place = new Place()
            {
                PlaceName = this.AccommodationName,
                Address = address
            };

            Room room = new Room()
            {
                Capacity = int.Parse(AvailiableVacanciesNumber),
                Number = RoomNumber
            };

            string currentUser = Thread.CurrentPrincipal.Identity.Name;
            try
            {
                await offersProxy.EditOfferAsync(currentUser, this.Id, offerInfo, place, room);
            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpił błąd przy edycji oferty");
                return;
            }
            Close();

            //uaktualnij bieżące oferty
            Ovm.Load();
           
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

        private string _roomNumber;

        public string RoomNumber
        {
            get { return _roomNumber; }
            set
            {
                _roomNumber = value;
                OnPropertyChanged();
            }    
        }

        public string Error => String.Empty;

        public int Id { get; set; }

        /// <summary>
        /// indekser potzrebny do walidacji
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                ///przyczyna błędu walidacji
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
                    case "RoomNumber":
                        if (string.IsNullOrWhiteSpace(RoomNumber) || !ov.ValidateLocalNumber(RoomNumber))
                        {
                            errorMessage = "Nieprawidłowy numer pokoju";
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
