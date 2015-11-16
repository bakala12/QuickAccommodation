﻿using AccommodationApplication.Commands;
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

namespace AccommodationApplication.ViewModels
{
    /// <summary>
    /// ViewModel dla dodawania nowych ofert
    /// </summary>
    public class AddNewOfferViewModel : ViewModelBase, IPageViewModel, IDataErrorInfo
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


        public AddNewOfferViewModel()
        {
            //ustawianie początkowych wartości dla dat
            _startDate = DateTime.Now;
            _endDate = DateTime.Now;

            AddCommand = new DelegateCommand(async x => await AddAsync());
        }

        /// <summary>
        /// Asynchroniczne dodawanie ofert
        /// </summary>
        /// <returns></returns>
        public async virtual Task AddAsync()
        {
            await Task.Run(() => Add());
        }

        /// <summary>
        /// komenda do dodawania nowych ofert
        /// </summary>
        public ICommand AddCommand { get; set; }

        /// <summary>
        /// Funkcja dodająca nową ofertę
        /// </summary>
        public void Add()
        {
            Address address = new Address()
            {
                City = this.City,
                Street = this.Street,
                LocalNumber = this.LocalNumber,
                PostalCode = this.PostalCode
            };

            OfferInfo offer = new OfferInfo()
            {
                //przekształcanie dat do odpowiedniej postaci (zgodnej z bazą danych)
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

            Offer offerToAdd = new Offer();

            //pobierz login aktualnego usera
            string currentUser = Thread.CurrentPrincipal.Identity.Name;

            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    //Wyciągnij z bazy aktualnego usera
                    User user = context.Users.FirstOrDefault(x => x.Username.Equals(currentUser));
                    if (user == null) throw new InvalidOperationException();

                    offerToAdd.OfferInfo = offer;
                    offerToAdd.Vendor = user;
                    offerToAdd.Place = place;

                    //dodaj do listy ofert nową ofertę
                    user.MyOffers.Add(offerToAdd);


                    context.SaveChanges();
                    transaction.Commit();
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
                OnPropertyChanged("StartDate");
                OnPropertyChanged("EndDate");
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
                OnPropertyChanged("StartDate");
                OnPropertyChanged("EndDate");
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
                return "Dodaj nową ofertę";
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


        //indekser, potrzebny do walidacji
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

                //zwracana jest przyczyna błędu walidacji wprowadzanych danych
                return errorMessage;
            }
        }
    }
}
