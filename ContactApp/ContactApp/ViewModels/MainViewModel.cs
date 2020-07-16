using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;
using ContactsLib;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using System.Data;
using System.IO;
using Xamarin.Essentials;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace ContactApp.ViewModels
{
    class MainViewModel : ViewModelBase
    {

        private ObservableCollection<Contact> m_Contacts;
        private bool m_IsFilterApplied = false;
        private Contact m_SelectedContact;
        private string m_Filter = "";

        public bool IsFilterApplied
        {
            get { return m_IsFilterApplied; }
            set
            {
                Set(ref m_IsFilterApplied, value, nameof(IsFilterApplied));
            }
        }

        public ObservableCollection<Contact> Contacts
        {
            get
            {
                return m_Contacts;
            }
            set { Set(ref m_Contacts, value); }
        }

        public string Filter
        {
            get { return m_Filter; }
            set
            {
                if (value != null)
                {
                    Set(ref m_Filter, value);
                    if(value.Length > 0)
                    {
                        m_IsFilterApplied = true;
                    } else
                    {
                        m_IsFilterApplied = false;
                    }
                    RaisePropertyChanged(nameof(IsFilterApplied));

                }
            }
        }

        public bool GetNameFilter(object args)
        {
            if (String.IsNullOrEmpty(Filter)) return true;
            return ((Contact)args).FullName.ToUpper().Contains(Filter.ToUpper());
        }

        public Contact SelectedContact
        {
            get { return m_SelectedContact; }
            set
            {
                if (value != null)
                {
                    Set(ref m_SelectedContact, value);
                }

            }
        }

        public void InitializeData()
        {
            DataService.Instance.ContactList = ContactManager.CreateContactList();
            DataService.Instance.ContactList.LoadDataFromDB(Path.Combine(FileSystem.AppDataDirectory, "temp.txt"));
            Contacts = DataService.Instance.ContactList.Contacts;
        }

        internal void AddNewContact(Contact contact)
        {
            if(contact != null)
            {
                Contacts.Add(contact);
                CheckPermission();
            }
            
        }

        internal void DeleteContact(Contact contact)
        {
            if (Contacts != null)
            {
                Contacts.Remove(contact);
                CheckPermission();
                
            } 
        }

        public async void CheckPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        // await DisplayAlert("Allow Permission", "If you wish to and an image to this Contact please allow the permission.", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    //Query permission
                    DataService.Instance.UpdateDb();
                }
                else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                {
                    //Storage denied
                }
            }
            catch (Exception ex)
            {
                //Something went wrong
            }

        }

        internal void ContactUpdated(Contact contact)
        {
            if(contact != null)
            {
                CheckPermission();
            }
        }
    }
}
