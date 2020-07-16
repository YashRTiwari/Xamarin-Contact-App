using ContactsLib;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace ContactApp 
{ 
    class DataService : ObservableObject
    {

        private static DataService s_Instance;
        private ContactList m_ContactList;

        static DataService()
        {
            s_Instance = new DataService();
        }

        public static DataService Instance { get { return s_Instance; } }

        public ContactList ContactList
        {
            get { return m_ContactList; }
            set 
            { 
                Set(ref m_ContactList, value);
            }
        }

        public void UpdateDb()
        {
            string vl = JsonConvert.SerializeObject(ContactList.Contacts);
            string fileName = Path.Combine(FileSystem.AppDataDirectory, "temp.txt");
            File.WriteAllText(fileName, vl);
        }


    }
}
