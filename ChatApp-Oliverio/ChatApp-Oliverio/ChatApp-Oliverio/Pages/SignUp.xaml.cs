using Plugin.CloudFirestore;
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
    public partial class SignUp : ContentPage
    {
        DataClass dataClass = DataClass.GetInstance;
        public SignUp()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            this.isLoading();
            var res = await DependencyService.Get<iFirebaseAuth>().SignUpWithGoogle();
            if (res.Status == true)
            {
                try
                {
                    await CrossCloudFirestore.Current
                     .Instance
                     .GetCollection("users")
                     .GetDocument(dataClass.LoggedInUser.Uid)
                     .SetDataAsync(dataClass.LoggedInUser);

                    await DisplayAlert("Success", res.Response, "Okay");
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Okay");
                }
            }
            else
            {
                await DisplayAlert("Error", res.Response, "Okay");
            }
            await Navigation.PopAsync();
            this.stopLoading();
        }

        async private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            this.isLoading();
            var result = await DependencyService.Get<iFirebaseAuth>().LoginAsyncWithFacebook();
            if (result.Status == true)
            {
                var res = await DependencyService.Get<iFirebaseAuth>().SignUpWithFacebook();
                if (res.Status == true)
                {
                    try
                    {
                        await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("users")
                         .GetDocument(dataClass.LoggedInUser.Uid)
                         .SetDataAsync(dataClass.LoggedInUser);

                        await DisplayAlert("Success", res.Response, "Okay");
                        await Navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "Okay");
                    }
                }
                else
                {
                    await DisplayAlert("Error", res.Response, "Okay");
                }
            }
            else
            {
                await DisplayAlert("Error", result.Response, "Okay");
            }

            await Navigation.PopAsync();
            this.stopLoading();
        }

        async private void CustomButton_Clicked(object sender, EventArgs e)
        {
            if (usernameEntry.Text != "" && emailSUEntry.Text != "" && pass1SUEntry.Text != "" && pass2SUEntry.Text != "")
            {
                if(pass1SUEntry.Text== pass2SUEntry.Text)
                {

                    this.isLoading();
                    var res = await DependencyService.Get<iFirebaseAuth>().SignUpWithEmailPassword(usernameEntry.Text, emailSUEntry.Text, pass1SUEntry.Text);
                    if (res.Status == true)
                    {
                        try
                        {
                            await CrossCloudFirestore.Current
                             .Instance
                             .GetCollection("users")
                             .GetDocument(dataClass.LoggedInUser.Uid)
                             .SetDataAsync(dataClass.LoggedInUser);

                            await DisplayAlert("Success", res.Response, "Okay");
                            await Navigation.PopAsync();
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", ex.Message, "Okay");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", res.Response, "Okay");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Password don't match", "Okay");
                }
            }
            else
            {
                if (usernameEntry.Text == "")
                {
                    usernameEntry.BorderColor = Color.Red;
                }
                if (emailSUEntry.Text == "")
                {
                    emailSUEntry.BorderColor = Color.Red;
                }
                if (pass1SUEntry.Text == "")
                {
                    pass1SUEntry.BorderColor = Color.Red;
                }
                if (pass2SUEntry.Text == "")
                {
                    pass2SUEntry.BorderColor = Color.Red;
                }
                await DisplayAlert("Error", "Missing Fields", "Okay");
            }
            this.stopLoading();
        }

        private void SUEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var elem = (CustomEntry)sender;
            elem.BorderColor = Color.Black;
        }

        private void pass1_Clicked(object sender, EventArgs e)
        {
            if(pass1.Text=="Show" && pass2.Text == "Show")
            {
                pass1.Text = "Hide";
                pass1SUEntry.IsPassword = false;
                pass2.Text = "Hide";
                pass2SUEntry.IsPassword = false;
            }
            else
            {
                pass1.Text = "Show";
                pass1SUEntry.IsPassword = true;
                pass2.Text = "Show";
                pass2SUEntry.IsPassword = true;
            }
                
        }
        private void emailSUEntry_Completed(object sender, EventArgs e)
        {
            pass1SUEntry.Focus();
        }

        private void pass1SUEntry_Completed(object sender, EventArgs e)
        {
            pass2SUEntry.Focus();
        }

        private void pass2SUEntry_Completed(object sender, EventArgs e)
        {
            this.CustomButton_Clicked(sender, e);
        }

        private void CustomButton_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
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
    }
}