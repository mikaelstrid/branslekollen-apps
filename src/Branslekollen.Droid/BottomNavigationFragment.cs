using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Branslekollen.Droid
{
    [Register("pixel.branslekollen.droid.BottomNavigationFragment")]
    public class BottomNavigationFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.BottomNavigation, container, false);
        }

        public void SetActiveItem(int activeItemId)
        {
            var activeItem = View.FindViewById<LinearLayout>(activeItemId);
            activeItem.SetPadding(activeItem.PaddingLeft, ConvertDpToPixels(6), activeItem.PaddingRight, activeItem.PaddingBottom);
            activeItem.Alpha = 1;
            var textView = activeItem.GetChildAt(1) as TextView;
            textView?.SetTextSize(ComplexUnitType.Sp, 14);
        }


        private int ConvertDpToPixels(int dpValue)
        {
            return (int) (dpValue * Resources.DisplayMetrics.Density);
        }
    }
}