using ContactsLib;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContactApp.ViewModels
{
    public class AddContactViewModel : ViewModelBase
    {

        private Contact m_CurrentContact;
        public Contact CurrentContact
        {
            get { return m_CurrentContact; }
            set 
            {
                Set(ref m_CurrentContact, value, nameof(CurrentContact));
                RaisePropertyChanged(nameof(PhoneListHeight));
                RaisePropertyChanged(nameof(EmailListHeight));
                RaisePropertyChanged(nameof(AddressListHeight));
            }
        }

        public Contact OriginalContact { get; set; }

        private double m_PhoneListHeight = 46;
        private double m_EmailListHeight = 46;
        private double m_AddressListHeight = 250;
        private double m_HeaderOffset = 32;


        public double PhoneListHeight { get { return m_PhoneListHeight * CurrentContact.Phones.Count + m_HeaderOffset; } set { Set(ref m_PhoneListHeight, value, nameof(PhoneListHeight)); } }
        public double EmailListHeight { get { return m_EmailListHeight * CurrentContact.Emails.Count + m_HeaderOffset; } set { Set(ref m_EmailListHeight, value, nameof(EmailListHeight)); } }
        public double AddressListHeight { get { return m_AddressListHeight * CurrentContact.Addresses.Count + m_HeaderOffset; } set { Set(ref m_AddressListHeight, value, nameof(AddressListHeight)); } }

        public ICommand RemovePhoneCommand { get;  }
        public ICommand RemoveEmailCommand { get; }
        public ICommand RemoveAddressCommand { get; }

        public ICommand SetPreferredCommand { get; }
        public AddContactViewModel()
        {
            RemovePhoneCommand = new Command<Phone>(RemovePhone);
            RemoveEmailCommand = new Command<Email>(RemoveEmail);
            RemoveAddressCommand = new Command<Address>(RemoveAddress);
            SetPreferredCommand = new Command<Phone>(SetPreferred);
            m_CurrentContact = new Contact();
        }

        private void SetPreferred(Phone obj)
        {
            if(CurrentContact != null && CurrentContact.Phones != null )
            {
                foreach(Phone p in CurrentContact.Phones)
                {
                    p.IsPreferred = false;
                }
            }
            obj.IsPreferred = !obj.IsPreferred;
            RaisePropertyChanged(nameof(CurrentContact.Phones));
        }

        public void AddPhone()
        {
            Phone phoneToAdd = new Phone();
            CurrentContact.Phones.Add(phoneToAdd);
            RaisePropertyChanged(nameof(PhoneListHeight));
        }

        public void AddEmail()
        {
            Email emailToAdd = new Email();
            CurrentContact.Emails.Add(emailToAdd);
            RaisePropertyChanged(nameof(EmailListHeight));
        }

        private void RemoveEmail(Email obj)
        {
            CurrentContact.Emails.Remove(obj);
            RaisePropertyChanged(nameof(EmailListHeight));
        }

        private void RemovePhone(Phone obj)
        {
            CurrentContact.Phones.Remove(obj);
            RaisePropertyChanged(nameof(PhoneListHeight));
        }

        public bool DismissPage()
        {
            if (OriginalContact.Equals(CurrentContact)) return false;

            return true;
        }

        public void AddAddress()
        {
            Address obj = new Address();
            CurrentContact.Addresses.Add(obj);
            RaisePropertyChanged(nameof(AddressListHeight));
        }

        internal void AddImageToContact(String path)
        {
            CurrentContact.Photo.ImagePath = path;
            RaisePropertyChanged(nameof(CurrentContact));
        }

        private void RemoveAddress(Address obj)
        {
            CurrentContact.Addresses.Remove(obj);
            RaisePropertyChanged(nameof(AddressListHeight));
        }

        internal bool UpdateContact()
        {
            Contact.ERROR error = CurrentContact.IsValid();
            if (error != Contact.ERROR.NONE)
            {
                Console.WriteLine(error.ToString());
                return false;
            } 
            
            OriginalContact.UpdateContact(CurrentContact);
            return true;
        }

        internal void UpdatePhoneType(Phone phone, Phone.Type type)
        {
            phone.PhoneType = type;
        }

        internal void UpdateEmailType(Email email, Email.Type type)
        {
            email.EmailType = type;
        }

        internal void UpdateAddressType(Address address, Address.Type type)
        {
            address.AddressType = type;
        }
    }
}
