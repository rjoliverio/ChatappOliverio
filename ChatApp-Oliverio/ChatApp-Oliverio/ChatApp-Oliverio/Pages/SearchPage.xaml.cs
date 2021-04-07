using Newtonsoft.Json;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_Oliverio
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        ObservableCollection<Account> result = new ObservableCollection<Account>();
        DataClass dataClass = DataClass.GetInstance;
        public SearchPage(string param)
        {
            InitializeComponent();
            displaySearchResultAsync(param);
        }

        private void closeSearchPage_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync(true);
        }
        private async Task displaySearchResultAsync(string param)
        {
            isLoading();
            var documents = await CrossCloudFirestore.Current
                                .Instance
                                .GetCollection("users")
                                .WhereEqualsTo("Email",param)
                                .GetDocumentsAsync();

            foreach (var documentChange in documents.Documents)
            {

                var obj = documentChange.ToObject<Account>();
                result.Add(obj);
            }
            //DataClass dataClass = DataClass.GetInstance;
            //await DisplayAlert("", dataClass.LoggedInUser.Uid + " " + result[0].UserName+ result[0].Uid, "Okay");
            resultsList.ItemsSource = result;
            resultsList.IsVisible = true;
            if (result.Count == 0)
            {
                await DisplayAlert("", "User not found.", "Okay");
                await Navigation.PopModalAsync(true);
            }
            stopLoading();
        }

        async private void resultsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var account = e.Item as Account;
            

            if (dataClass.LoggedInUser.Uid == account.Uid)
            {
                await DisplayAlert("Error", "You are not allowed to add your own self", "OKAY");
            }
            else
            {
                var res = await DisplayAlert("Add contact", "Would you like to add " + account.UserName + "?", "Yes", "No");
                if (res)
                {
                    isLoading();
                    var contact=DependencyService.Get<iFirebaseAuth>().GenerateID(account);
                    
                    //users(owner)->contacts
                    if (dataClass.LoggedInUser.contacts == null)
                    {
                        dataClass.LoggedInUser.contacts = new List<string>();
                    }
                    if (!(dataClass.LoggedInUser.contacts.Contains(account.Uid)))
                    {
                        //contacts
                        await CrossCloudFirestore.Current
                            .Instance
                            .GetCollection("contacts")
                            .GetDocument(contact.id)
                            .SetDataAsync(contact);

                        dataClass.LoggedInUser.contacts.Add(account.Uid);
                        await CrossCloudFirestore.Current
                        .Instance
                        .GetCollection("users")
                        .GetDocument(dataClass.LoggedInUser.Uid)
                        .UpdateDataAsync(new { contacts = dataClass.LoggedInUser.contacts });

                        //users(addedContact)->contacts
                        if (account.contacts == null)
                        {
                            account.contacts = new List<string>();
                        }
                        account.contacts.Add(dataClass.LoggedInUser.Uid);
                        await CrossCloudFirestore.Current
                            .Instance
                            .GetCollection("users")
                            .GetDocument(account.Uid)
                            .UpdateDataAsync(new { contacts = account.contacts });
                        await DisplayAlert("Success", "Contact Added!", "Okay");
                    }
                    else
                    {
                        await DisplayAlert("Failed", "You both already have a connection", "Okay");
                    }
                    await Navigation.PopModalAsync(true);
                    stopLoading();
                }
                else
                {
                    await Navigation.PopModalAsync(true);
                }
            }
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