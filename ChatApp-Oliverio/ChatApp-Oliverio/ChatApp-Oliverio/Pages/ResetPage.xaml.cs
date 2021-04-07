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
public partial class ResetPage : ContentPage
{
    public ResetPage()
    {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetBackButtonTitle(this, "ChatApp Reset Password");
    }

        async private void CustomButton_Clicked(object sender, EventArgs e)
        {
            if (emailEntry.Text!="")
            {
                isLoading();
                var res = await DependencyService.Get<iFirebaseAuth>().ResetPassword(emailEntry.Text);

                if (res.Status == true)
                {
                    await DisplayAlert("Success", res.Response, "Okay");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", res.Response, "Okay");
                }
            }
            else
            {
                emailEntry.BorderColor = Color.Red;
                await DisplayAlert("Error", "Missing Field", "Okay");
            }
            stopLoading();
        }

        private void emailEntry_Focused(object sender, FocusEventArgs e)
        {
            emailEntry.BorderColor = Color.Black;
        }
        private void emailEntry_Completed(object sender, EventArgs e)
        {
            this.CustomButton_Clicked(sender, e);
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