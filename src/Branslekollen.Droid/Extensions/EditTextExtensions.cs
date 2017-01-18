using Android.Widget;

namespace Branslekollen.Droid.Extensions
{
    public static class EditTextExtensions
    {
        public static void ShowError(this TextView textView, string errorMessage)
        {
            textView.Error = errorMessage;
            //textView.Background.SetColorFilter(Color.Red, PorterDuff.Mode.SrcAtop);
        }

        public static void ClearError(this TextView textView)
        {
            textView.Error = null;
            //textView.Background.SetColorFilter(Color.Green, PorterDuff.Mode.SrcAtop);
        }
    }
}