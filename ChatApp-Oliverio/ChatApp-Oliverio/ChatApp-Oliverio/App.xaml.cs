using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp_Oliverio
{
    public partial class App : Application
    {
        public static float screenWidth { get; set; }
        public static float screenHeight { get; set; }
        public static float appScale { get; set; }
        public App()
        {
            InitializeComponent();
            DataClass dataClass = DataClass.GetInstance;
            if (dataClass.SignedIn)
            {
                Application.Current.MainPage = new UserTabbedPage();
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
           
        }

        protected override void OnResume()
        {
            
        }


    }
}
