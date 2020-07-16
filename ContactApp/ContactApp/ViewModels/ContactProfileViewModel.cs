using ContactApp.utils;
using ContactsLib;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using static ContactApp.Pages.AddContact;

namespace ContactApp.ViewModels
{
    public class ContactProfileViewModel : ViewModelBase
    {

        private Contact m_CurrentContact;
        public bool PhoneListVisible 
        {
            get 
            {
                if (CurrentContact == null || CurrentContact.Phones == null || CurrentContact.Phones.Count == 0) 
                    return false;
                return true;
            }
        }

        public bool IsPerferredPhoneVisible
        {
            get { 
                if (CurrentContact == null || CurrentContact.PhonePreferred == null) 
                    return false;
                return true;
            }
        }
        
        public bool IsPerferredEmailVisible
        {
            get
            {
                if (CurrentContact == null || CurrentContact.EmailPreferred == null) 
                    return false;
                return true;
            }
        }

        public bool EmailListVisible
        {
            get
            {
                if (CurrentContact == null || CurrentContact.Emails == null || CurrentContact.Emails.Count == 0) 
                    return false;
                return true;
            }
        }

        public bool AddressListVisible
        {
            get
            {
                if (CurrentContact == null || CurrentContact.Addresses == null || CurrentContact.Addresses.Count == 0) 
                    return false;
                return true;
            }
        }


        public Contact CurrentContact
        {
            get { return m_CurrentContact; }
            set
            {
                Set(ref m_CurrentContact, value, nameof(CurrentContact));
                RaisePropertyChanged(nameof(PhoneListVisible));
                RaisePropertyChanged(nameof(EmailListVisible));
                RaisePropertyChanged(nameof(AddressListVisible));
                RaisePropertyChanged(nameof(IsPerferredEmailVisible));
                RaisePropertyChanged(nameof(IsPerferredPhoneVisible));
            }
        }

        public object Navigation { get; private set; }

        internal void MakePhoneCall()
        {
            Utility.PlacePhoneCall(CurrentContact.PhonePreferred.ToString());
        }

        internal async void SendEmail()
        {
            await Utility.SendEmail("", "", new List<string>() { CurrentContact.EmailPreferred.ToString()});
        }

        internal async void ShareContact()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = CurrentContact.GetShareContactText(),
                Title = "Share Contact - " + CurrentContact.FullName
            });
        }

        internal bool ContactUpdated(Contact contact)
        {
            CurrentContact = contact;
            RaisePropertyChanged(nameof(CurrentContact));
            return true;
        }
    }
}
