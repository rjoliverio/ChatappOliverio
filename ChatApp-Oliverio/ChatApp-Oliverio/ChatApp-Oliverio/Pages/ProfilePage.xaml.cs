using Plugin.FacebookClient;
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
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        async private void CustomButton_Clicked(object sender, EventArgs e)
        {
            var res = DependencyService.Get<iFirebaseAuth>().SignOut();
            if (res.Status == true)
            {
                App.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                await DisplayAlert("Error", res.Response, "Okay");
            }
        }
    }
}