using Android.App;
using Android.OS;

namespace Branslekollen.Droid
{
    [Activity(Label = "DashboardActivity")]
    public class DashboardActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Dashboard);

            var bottomNavigationFragment = FragmentManager.FindFragmentById<BottomNavigationFragment>(Resource.Id.bottom_navigation_fragment);
            bottomNavigationFragment.SetActiveItem(Resource.Id.bottom_navigation_history);
        }
    }
}