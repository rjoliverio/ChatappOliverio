using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ChatApp_Oliverio.Droid;
using Android.Content;
using ChatApp_Oliverio;
[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace ChatApp_Oliverio.Droid
{
    class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            CustomButton elem = Element as CustomButton;
            Control.SetAllCaps(elem.AutoCapitalization);
        }
    }
}