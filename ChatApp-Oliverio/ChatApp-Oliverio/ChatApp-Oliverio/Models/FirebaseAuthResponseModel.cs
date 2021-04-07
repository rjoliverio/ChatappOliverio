using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChatApp_Oliverio
{
    public class FirebaseAuthResponseModel : INotifyPropertyChanged
    {
        bool MyStatus { get; set; }
        string MyResponse { get; set; }

        public bool Status { get { return MyStatus; } set { MyStatus = value; OnPropertyChanged(nameof(Status)); } }
        public string Response { get { return MyResponse; } set { MyResponse = value; OnPropertyChanged(nameof(Response)); } }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
