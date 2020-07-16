using ContactsLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ContactApp.utils
{
    class TypeImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string objectType = parameter as string;
            if(objectType != null)
            {
                if (objectType.Equals("phone"))
                {
                    return GetPhoneTypeImage(value);
                } 
                else  if (objectType.Equals("email"))
                {
                    return GetEmailTypeImage(value);
                } else if (objectType.Equals("address"))
                {
                    return GetAddressTypeImage(value);
                }
            }
            
            return null;
        }

        private object GetPhoneTypeImage(object value)
        {
            Phone.Type type = (Phone.Type) value;
            switch (type)
            {
                case Phone.Type.WORK:
                    return "work.png";
                case Phone.Type.HOME:
                    return "home.png";
                case Phone.Type.PERSONAL:
                    return "mobile.png";
            }
            return null;
        }

        private object GetEmailTypeImage(object value)
        {
            Email.Type type = (Email.Type) value;
            switch (type)
            {
                case Email.Type.WORK:
                    return "work.png";
                case Email.Type.PERSONAL:
                    return "home.png";
            }
            return null;
        }

        private object GetAddressTypeImage(object value)
        {
            Address.Type type = (Address.Type) value;
            switch (type)
            {
                case Address.Type.WORK:
                    return "work.png";
                case Address.Type.PERSONAL:
                    return "home.png";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
