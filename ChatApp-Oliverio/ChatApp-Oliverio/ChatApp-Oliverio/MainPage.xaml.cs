using Newtonsoft.Json;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace ChatApp_Oliverio
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }
        async private void CustomButton_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new SignUp());
        }
        

        async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //FacebookResponse<bool> response = await CrossFacebookClient.Current.LoginAsync(new string[] { "email", "public_profile" });
            //await DisplayAlert("SAmple", "Data: " + response.Data + "Status: " + response.Status + "Message: " + response.Message, "Ok");
            isLoading();
            var src = (Frame)sender;
            if (src == googlesignin)
            {
                var res = await DependencyService.Get<iFirebaseAuth>().SigninWithGoogle();
                if (res.Status == true)
                {
                    Application.Current.MainPage = new UserTabbedPage();
                }
                else
                {
                    await DisplayAlert("Error", res.Response, "Okay");
                }
            }
            else
            {
                var res = await DependencyService.Get<iFirebaseAuth>().LoginAsyncWithFacebook();
                if (res.Status == true)
                {
                    var respond = await DependencyService.Get<iFirebaseAuth>().SigninWithFacebook();
                    if (respond.Status == true)
                    {
                        Application.Current.MainPage = new UserTabbedPage();
                    }
                    else
                    {
                        await DisplayAlert("Error", respond.Response, "Okay");
                    }
                }
                else
                {
                    await DisplayAlert("Error", res.Response, "Okay");
                }
            }
            stopLoading();
        }

        private void passShow_Clicked(object sender, EventArgs e)
        {
            if (passShow.Text == "Show")
            {
                passShow.Text = "Hide";
                passEntry.IsPassword = false;
            }
            else
            {
                passShow.Text = "Show";
                passEntry.IsPassword = true;
            }
        }

        async private void btnSignIn_Clicked(object sender, EventArgs e)
        {
            if (emailEntry.Text=="" || passEntry.Text=="")
            {
                if (emailEntry.Text == "")
                {
                    emailEntry.BorderColor = Color.Red;
                }
                if (passEntry.Text == "")
                {
                    passEntry.BorderColor = Color.Red;
                }
               await DisplayAlert("Error", "Missing Fields", "Okay");
            }
            else
            {
                this.isLoading();
                var res = await DependencyService.Get<iFirebaseAuth>().LoginWithEmailPassword(emailEntry.Text, passEntry.Text);
                if (res.Status == true)
                {
                    Application.Current.MainPage = new UserTabbedPage();
                }
                else
                {
                    await DisplayAlert("Error", res.Response, "Okay");
                }
                this.stopLoading();
            }
            
        }
        private void passEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            passEntry.BorderColor = Color.Black;
        }

        private void emailEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            emailEntry.BorderColor = Color.Black;
        }
        private void emailEntry_Completed(object sender, EventArgs e)
        {
            passEntry.Focus();
        }

        private void passEntry_Completed(object sender, EventArgs e)
        {
            this.btnSignIn_Clicked(sender, e);
        }
        private void isLoading()
        {
            ai.IsRunning = true;
            aiLayout.BackgroundColor = Color.FromRgba(0, 0, 0, 0.50);
            aiLayout.IsVisible = true;
        }
        private void stopLoading()
        {
            aiLayout.IsVisible = false;
            ai.IsRunning = false;
        }

        async private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ResetPage());
        }

        
    }
    
}
