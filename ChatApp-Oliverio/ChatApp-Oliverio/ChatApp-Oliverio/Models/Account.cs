using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChatApp_Oliverio
{
    public class Account : INotifyPropertyChanged
    {
        string MyUID { get; set; }
        string MyUserName { get; set; }
        string MyEmail { get; set; }
        int MyUserType { get; set; }
        DateTime DateCreated { get; set; }
        List<string> _contacts { get; set; }
        public string Uid { get { return MyUID; } set { MyUID = value; OnPropertyChanged(nameof(Uid)); } }
        public string UserName { get {return MyUserName; } set { MyUserName= value; OnPropertyChanged(nameof(UserName)); } }
        public string Email { get { return MyEmail; } set { MyEmail = value; OnPropertyChanged(nameof(Email)); } }
        public int UserType { get { return MyUserType; } set { MyUserType = value; OnPropertyChanged(nameof(UserType)); } }
        public DateTime CreatedAt { get { return DateCreated; } set { DateCreated = value; OnPropertyChanged(nameof(CreatedAt)); } }
        public List<string> contacts { get { return _contacts; } set { _contacts = value; OnPropertyChanged(nameof(contacts)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
