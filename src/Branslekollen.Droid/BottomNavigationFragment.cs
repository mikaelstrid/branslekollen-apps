using Android.App;
using Android.Content;
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
            var view = inflater.Inflate(Resource.Layout.BottomNavigation, container, false);
            view.FindViewById<LinearLayout>(Resource.Id.bottom_navigation_statistics).Click +=
                (sender, args) => StartActivity(new Intent(Context, typeof(CreateVehicleActivity)));
            view.FindViewById<LinearLayout>(Resource.Id.bottom_navigation_refuelings).Click +=
                (sender, args) => StartActivity(new Intent(Context, typeof(RefuelingsActivity)));
            view.FindViewById<LinearLayout>(Resource.Id.bottom_navigation_profile).Click +=
                (sender, args) => StartActivity(new Intent(Context, typeof(CreateVehicleActivity)));
            return view;
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