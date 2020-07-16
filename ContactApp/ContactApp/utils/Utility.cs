using ContactsLib;
using Newtonsoft.Json;
using Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ContactApp.utils
{
    class Utility
    {

        static Utility m_Instance;

        static Utility()
        {
            m_Instance = new Utility();
        }

        public Utility Instance 
        {
            get { return m_Instance;  }

        }

        public static String PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
                return null;
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                return "Invalid Number";
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                return "Phone Dialer is not supported on this device.";
            }
            catch (Exception ex)
            {
                return "Something went wrong";
            }
        }

        public static async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                };
                await Xamarin.Essentials.Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

    }
}
