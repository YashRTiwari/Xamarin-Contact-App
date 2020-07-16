using ContactApp.ViewModels;
using ContactsLib;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Input;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContactApp.Pages

{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddContact : ContentPage
    {

        private AddContactViewModel MyViewModel { get { return (AddContactViewModel) BindingContext;  } }

        public interface IContact
        {
            void ContactSaved(Contact contact);
        }

        private IContact i_ContactSaved;

        public AddContact(IContact iContactSaved)
        {
            InitializeComponent();
            i_ContactSaved = iContactSaved;
            MyViewModel.OriginalContact = MyViewModel.CurrentContact.GetCopy();
        }

        public AddContact(IContact iContactSaved, Contact contact)
        {
            InitializeComponent();
            i_ContactSaved = iContactSaved;
            MyViewModel.OriginalContact = contact;
            MyViewModel.CurrentContact = contact.GetCopy();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


        public async void CheckPermission(object sender, EventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await DisplayAlert("Allow Permission", "If you wish to and an image to this Contact please allow the permission.", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    //Query permission
                    OnPickPhotoButtonClicked();
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //Storage denied
                }
            }
            catch (Exception ex)
            {
                //Something went wrong
            }

        }

        async void OnPickPhotoButtonClicked()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "Not supported", "Ok");
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImageFile == null)
            {
                await DisplayAlert("Error", "No image selected", "Ok");
                return;
            }

            MyViewModel.AddImageToContact(selectedImageFile.Path); 
        }

        private void AddPhoneButton_Clicked(object sender, EventArgs e)
        {
            MyViewModel.AddPhone();
        }

        private void AddEmailButton_Clicked(object sender, EventArgs e)
        {
            MyViewModel.AddEmail();
        }

        private void AddAddressButton_Clicked(object sender, EventArgs e)
        {
            MyViewModel.AddAddress();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            bool wereChangesMade = MyViewModel.DismissPage();

            if (wereChangesMade) 
            {
                bool answer = await DisplayAlert("Discard changes?", "Would you like to disregard the changes made?", "Yes", "No");
                Debug.WriteLine(answer);
                wereChangesMade = !answer; // answer = Yes -> true => Discard changes
            }

            if(!wereChangesMade) _= Navigation.PopAsync();

        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (MyViewModel.UpdateContact())
            {
                i_ContactSaved.ContactSaved(MyViewModel.OriginalContact);
                await Navigation.PopAsync();
            }
            else
                await DisplayAlert("Alert", "You have been alerted", "OK");
        }

        private void TypeSelectorButton_Clicked(object sender, EventArgs e)
        {
            ImageButton phoneType = (ImageButton)sender;
            RadPopup popup = phoneType.FindByName<RadPopup>("PhoneTypePopup");
            popup.IsOpen = true;
        }

        private void WorkTypeButton_Clicked(object sender, EventArgs e)
        {
            ImageButton ib = (ImageButton)sender;
            ImageButton typeSelectorButton = (ImageButton)ib.Parent.Parent.Parent.Parent;
            RadPopup popup = typeSelectorButton.FindByName<RadPopup>("PhoneTypePopup");
            popup.IsOpen = false;

            Phone phone = typeSelectorButton.BindingContext as Phone;
            if (phone != null)
            {
                UpdateViewTypeHelper(typeSelectorButton, phone, Phone.Type.WORK); return;
            }
            Email email = typeSelectorButton.BindingContext as Email;
            if (email != null)
            {
                UpdateViewTypeHelper(typeSelectorButton, email, Email.Type.WORK); return;
            }

            Address address = typeSelectorButton.BindingContext as Address;
            if (address != null)
            {
                UpdateViewTypeHelper(typeSelectorButton, address, Address.Type.WORK); return;
            }
        }

        private void PersonalTypeButton_Clicked(object sender, EventArgs e)
        {
            ImageButton ib = (ImageButton)sender;
            ImageButton typeSelectorButton = (ImageButton)ib.Parent.Parent.Parent.Parent;
            Phone phone = typeSelectorButton.BindingContext as Phone;
            RadPopup popup = typeSelectorButton.FindByName<RadPopup>("PhoneTypePopup");
            popup.IsOpen = false;

            if (phone != null)
            {
                UpdateViewTypeHelper(typeSelectorButton, phone, Phone.Type.PERSONAL); return;
            }
            Email email = typeSelectorButton.BindingContext as Email;
            if (email != null)
            {
                UpdateViewTypeHelper(typeSelectorButton, email, Email.Type.PERSONAL); return;
            }

            Address address = typeSelectorButton.BindingContext as Address;
            if (address != null)
            {
                UpdateViewTypeHelper(typeSelectorButton, address, Address.Type.PERSONAL); return;
            }
        }

        private void HomeTypeButton_Clicked(object sender, EventArgs e)
        {

            ImageButton ib = (ImageButton)sender;
            ImageButton typeSelectorButton = (ImageButton)ib.Parent.Parent.Parent.Parent;
            Phone phone = typeSelectorButton.BindingContext as Phone;
            RadPopup popup = typeSelectorButton.FindByName<RadPopup>("PhoneTypePopup");
            popup.IsOpen = false;

            if (phone != null)
            {
                UpdateViewTypeHelper(typeSelectorButton, phone, Phone.Type.HOME); return;
            }

            Email email =  typeSelectorButton.BindingContext as Email;
            if (email != null)
            {
                UpdateViewTypeHelper(typeSelectorButton, email, Email.Type.PERSONAL); return;
            }

            Address address = typeSelectorButton.BindingContext as Address;
            if (address != null)
            {
                UpdateViewTypeHelper(typeSelectorButton, address, Address.Type.PERSONAL); return;
            }

        }

        private void UpdateViewTypeHelper(ImageButton typeSelectorButton, Phone phone, Phone.Type type)
        {
                if(type == Phone.Type.HOME)
                {
                    typeSelectorButton.Source = "home.png";
                } else if (type == Phone.Type.PERSONAL)
                {
                    typeSelectorButton.Source = "mobile.png";
                }
                else if (type == Phone.Type.WORK)
                {
                    typeSelectorButton.Source = "work.png";
                }
                MyViewModel.UpdatePhoneType(phone, type);
        }

        private void UpdateViewTypeHelper(ImageButton typeSelectorButton, Email email, Email.Type type)
        {
                if (type == Email.Type.PERSONAL)
                {
                    typeSelectorButton.Source = "home.png";
                }
                else if (type == Email.Type.WORK)
                {
                    typeSelectorButton.Source = "work.png";
                }
            MyViewModel.UpdateEmailType(email, type);
        }


        private void UpdateViewTypeHelper(ImageButton typeSelectorButton, Address address, Address.Type type)
        {
            if (type == Address.Type.PERSONAL)
            {
                typeSelectorButton.Source = "home.png";
            }
            else if (type == Address.Type.WORK)
            {
                typeSelectorButton.Source = "work.png";
            }
            MyViewModel.UpdateAddressType(address, type);
        }

    }
}