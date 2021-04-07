using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_Oliverio
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserTabbedPage : TabbedPage
    {
        DataClass dataClass = DataClass.GetInstance;
        public UserTabbedPage()
        {
            InitializeComponent();
            profilePage.BindingContext = dataClass.LoggedInUser;
        }
    }
}