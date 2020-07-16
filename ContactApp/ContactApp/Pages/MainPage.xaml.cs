using ContactsLib;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Telerik.XamarinForms.DataControls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Telerik.XamarinForms.DataControls.ListView;
using ContactApp.ViewModels;
using ContactApp.utils;

namespace ContactApp.Pages
{
    public partial class MainPage : ContentPage, AddContact.IContact, ContactProfilePage.IContactProfile
    {

        private MainViewModel MyViewModel { get { return (MainViewModel)BindingContext; } }

        private DelegateFilterDescriptor filterDescriptor;
        private PropertySortDescriptor sortDescriptor;

        public MainPage()
        {
            InitializeComponent();
            filterDescriptor = new DelegateFilterDescriptor { Filter = MyViewModel.GetNameFilter };
            sortDescriptor = new PropertySortDescriptor { PropertyName = "FullName", SortOrder = SortOrder.Ascending };
            contactListView.SortDescriptors.Add(sortDescriptor);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (MyViewModel.Contacts == null)
            {
                MyViewModel.InitializeData();
            }
        }

        private void listView_ItemTapped(object sender, Telerik.XamarinForms.DataControls.ListView.ItemTapEventArgs e)
        {
            Contact contact = (Contact) e.Item;
            MyViewModel.SelectedContact = contact;
            Navigation.PushAsync(new ContactProfilePage(contact, this));
        }

        private void filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            contactListView.FilterDescriptors.Add(filterDescriptor);
        }

        private void ClearEntry_Clicked(object sender, EventArgs e)
        {
            filter.Text = "";
        }

        private void contactListView_ItemHold(object sender, ItemHoldEventArgs e)
        {
            Console.WriteLine("Hold");
        }

        
        public void ContactSaved(Contact contact) 
        {
            MyViewModel.AddNewContact(contact);
        }

        private void AddContact_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddContact(this));
        }

        private async void contactListView_ItemSwipeCompleted(object sender, ItemSwipeCompletedEventArgs e)
        {
            var listView = sender as RadListView;
            var item = (Contact) e.Item ;

            listView.EndItemSwipe();

            if (e.Offset >= 70)
            {
                // TODO : Check if removed.
                if(item.PhonePreferred != null)Utility.PlacePhoneCall(item.PhonePreferred.ToString());
            }
            else if (e.Offset <= -70)
            {
                // TODO : Check if removed.
                if(item.EmailPreferred != null)await Utility.SendEmail("", "", new List<string>() { item.EmailPreferred.Address });
            }

        }

        public void DeleteContact(Contact contact)
        {
            MyViewModel.DeleteContact(contact);
        }

        public void ContactUpdated(Contact contact)
        {
            MyViewModel.ContactUpdated(contact);
        }
    }
}
