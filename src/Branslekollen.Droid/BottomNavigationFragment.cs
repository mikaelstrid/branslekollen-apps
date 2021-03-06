using System;
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
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.BottomNavigation, container, false);
            view.FindViewById<LinearLayout>(Resource.Id.BottomNavigationMenuItemStatistics).Click +=
                (sender, args) => GoToActivity(typeof(StatisticsActivity));
            view.FindViewById<LinearLayout>(Resource.Id.BottomNavigationMenuItemRefuelings).Click +=
                (sender, args) => GoToActivity(typeof(RefuelingsActivity));
            view.FindViewById<LinearLayout>(Resource.Id.BottomNavigationMenuItemProfile).Click +=
                (sender, args) => GoToActivity(typeof(ProfileActivity));
            return view;
        }

        public void SetActiveItem(int activeItemId)
        {
            var activeItem = View.FindViewById<LinearLayout>(activeItemId);
            activeItem.SetPadding(activeItem.PaddingLeft, Utilities.ConvertDpToPixels(Resources, 6), activeItem.PaddingRight, activeItem.PaddingBottom);
            activeItem.Alpha = 1;
            var textView = activeItem.GetChildAt(1) as TextView;
            textView?.SetTextSize(ComplexUnitType.Sp, 14);
        }

        private void GoToActivity(Type activityType)
        {
            StartActivity(new Intent(Context, activityType));
            ((Activity) Context).Finish();
            ((Activity)Context).OverridePendingTransition(Resource.Animation.fade_in, Resource.Animation.fade_out);
        }
    }
}