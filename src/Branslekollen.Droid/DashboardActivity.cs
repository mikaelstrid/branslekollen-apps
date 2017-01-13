using Android.App;
using Android.OS;
using Android.Widget;

namespace Branslekollen.Droid
{
    [Activity(Label = "DashboardActivity")]
    public class DashboardActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Dashboard);

            //var editToolbar = FindViewById<Toolbar>(Resource.Id.edit_toolbar);
            //editToolbar.Title = "Editing";
            //editToolbar.InflateMenu(Resource.Menu.bottom_navigation);
            //editToolbar.MenuItemClick += (sender, e) => {
            //    Toast.MakeText(this, "Bottom toolbar tapped: " + e.Item.TitleFormatted, ToastLength.Short).Show();
            //};
        }
    }
}