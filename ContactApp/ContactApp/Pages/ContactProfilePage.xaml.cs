using ContactApp.ViewModels;
using ContactsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContactApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactProfilePage : ContentPage, AddContact.IContact
    {

        public interface IContactProfile
        {
            void DeleteContact(Contact contact);
            void ContactUpdated(Contact contact);
        }

        public ContactProfileViewModel MyViewModel { get { return (ContactProfileViewModel)BindingContext; } }

        public IContactProfile i_ContactProfile;

        public ContactProfilePage(ContactsLib.Contact contact, IContactProfile iContactProfile)
        {
            InitializeComponent();
            MyViewModel.CurrentContact = contact;
            i_ContactProfile = iContactProfile;
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            //Animate Image Size
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void EmailCurrentContact_Clicked(object sender, EventArgs e)
        {
            MyViewModel.SendEmail();
        }

        private void CallCurrentContact_Clicked(object sender, EventArgs e)
        {
            MyViewModel.MakePhoneCall();
        }


        public void ContactSaved(Contact contact)
        {
            // Contact Updated
            MyViewModel.ContactUpdated(contact);
            i_ContactProfile.ContactUpdated(contact);
        }

        private void MenuOptionButton_Clicked(object sender, EventArgs e)
        {
            popup.IsOpen = true;
        }

        private void ClosePopup(object sender, EventArgs e)
        {
            popup.IsOpen = false;
        }

        private void EditFrame_Tapped(object sender, EventArgs e)
        {
            popup.IsOpen = false;
            Navigation.PushAsync(new AddContact(this, MyViewModel.CurrentContact));

        }

        private void ShareFrame_Tapped(object sender, EventArgs e)
        {
            popup.IsOpen = false;
            MyViewModel.ShareContact();
        }

        private async void DeleteFrame_Tapped(object sender, EventArgs e)
        {
            popup.IsOpen = false;
            i_ContactProfile.DeleteContact(MyViewModel.CurrentContact);
            await Navigation.PopAsync();
        }

        private async void AddressList_ItemTapped(object sender, Telerik.XamarinForms.DataControls.ListView.ItemTapEventArgs e)
        {
            Address address = (Address)e.Item;
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Launcher.OpenAsync("http://maps.apple.com/?q="+address.ToString());
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                await Launcher.OpenAsync("geo:0,0?q=" + address.ToString());
            }

        }

        private void PhoneList_ItemTapped(object sender, Telerik.XamarinForms.DataControls.ListView.ItemTapEventArgs e)
        {
            Phone phone = (Phone)e.Item;
            utils.Utility.PlacePhoneCall(phone.ToString());
        }

        private async void EmailList_ItemTapped(object sender, Telerik.XamarinForms.DataControls.ListView.ItemTapEventArgs e)
        {
            ContactsLib.Email email = (ContactsLib.Email) e.Item;
            await utils.Utility.SendEmail("", "", new List<String>() { email.ToString() });
        }

       
    }
}