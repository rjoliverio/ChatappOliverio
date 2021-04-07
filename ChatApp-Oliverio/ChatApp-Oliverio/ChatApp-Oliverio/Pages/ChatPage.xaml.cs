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
    public partial class ChatPage : ContentPage
    {
        DataClass dataClass = DataClass.GetInstance;
        ObservableCollection<ContactModel> contactList = new ObservableCollection<ContactModel>();
        
        public ChatPage()
        {
            InitializeComponent();
            displayContactList();
        }

        private void clearEntry_Clicked(object sender, EventArgs e)
        {
            searchEntry.Text = "";
            clearEntry.IsVisible = false;
        }

        private void searchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            clearEntry.IsVisible = true;
        }

        private void contactsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var contact = e.Item as ContactModel;
            Navigation.PushModalAsync(new ConversationPage (contact));
        }
        async private void displayContactList()
        {
            CrossCloudFirestore.Current
                .Instance
                .GetCollection("contacts")
                .WhereArrayContains("contactID", dataClass.LoggedInUser.Uid)
                .AddSnapshotListener(async (snapshot, error) =>
                {
                    isLoading();

                    if (snapshot != null)
                    {
                        foreach(var documentChange in snapshot.DocumentChanges)
                        {
                            //var obj=documentChange.Document.ToObject<ContactModel>();
                            //var timestamp = documentChange.Document.Data["created_at"];
                            //System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            ////dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
                            //documentChange.Document.Data["created_at"] =DateTime.UtcNow;
                            //await DisplayAlert("", JsonConvert.SerializeObject(documentChange.Document.Data["created_at"]) + " ", "ok");
                            //var json = JsonConvert.SerializeObject(documentChange.Document.Data);
                            //var obj = JsonConvert.DeserializeObject<ContactModel>(json);
                            var obj = new ContactModel
                            {
                                id = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(documentChange.Document.Data["id"])),
                                contactID = JsonConvert.DeserializeObject<string[]>(JsonConvert.SerializeObject(documentChange.Document.Data["contactID"])),
                                contactEmail = JsonConvert.DeserializeObject<string[]>(JsonConvert.SerializeObject(documentChange.Document.Data["contactEmail"])),
                                contactName = JsonConvert.DeserializeObject<string[]>(JsonConvert.SerializeObject(documentChange.Document.Data["contactName"])),
                                created_at = DateTime.UtcNow
                            };
                            switch (documentChange.Type)
                            {
                                case DocumentChangeType.Added:
                                    contactList.Add(obj);
                                    break;
                                case DocumentChangeType.Modified:
                                    if (contactList.Where(c => c.id == obj.id).Any())
                                    {
                                        var item = contactList.Where(c => c.id == obj.id).FirstOrDefault();
                                        item = obj;
                                    }
                                    break;
                                case DocumentChangeType.Removed:
                                    if (contactList.Where(c => c.id == obj.id).Any())
                                    {
                                        var item = contactList.Where(c => c.id == obj.id).FirstOrDefault();
                                        contactList.Remove(item);
                                    }
                                    break;
                            }
                        }
                    }
                    emptyListLabel.IsVisible = contactList.Count == 0;
                    contactsList.IsVisible = !(contactList.Count == 0);
                    contactsList.ItemsSource = contactList;
                    
                    stopLoading();
                });
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

        private void searchEntry_Completed(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SearchPage(searchEntry.Text));
        }
    }
}