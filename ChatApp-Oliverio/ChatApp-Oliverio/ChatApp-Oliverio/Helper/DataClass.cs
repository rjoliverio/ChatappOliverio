using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace ChatApp_Oliverio
{
    public class DataClass : INotifyPropertyChanged
    {
        static DataClass dataClass;
        public static DataClass GetInstance
        {
            get
            {
                if (dataClass == null)
                {
                    dataClass = new DataClass();
                }
                return dataClass;
            }
        }
        bool isSignedIn { get; set; }
        public bool SignedIn
        {
            set
            {
                isSignedIn = value;
                Application.Current.Properties["SignedIn"] = isSignedIn;
                Application.Current.SavePropertiesAsync();
                OnPropertyChanged();
            }
            get
            {
                if (Application.Current.Properties.ContainsKey("SignedIn"))
                {
                    isSignedIn = Convert.ToBoolean(Application.Current.Properties["SignedIn"]);
                }
                return isSignedIn;
            }
        }
        Account isLoggedInUser { get; set; }
        public Account LoggedInUser
        {
            set
            {
                isLoggedInUser = value;
                Application.Current.Properties["LoggedInUser"] = JsonConvert.SerializeObject(isLoggedInUser);
                Application.Current.SavePropertiesAsync();
                OnPropertyChanged();
            }
            get
            {
                if(isLoggedInUser==null && Application.Current.Properties.ContainsKey("LoggedInUser"))
                {
                    isLoggedInUser = JsonConvert.DeserializeObject<Account>(Application.Current.Properties["LoggedInUser"].ToString());
                }
                return isLoggedInUser;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
