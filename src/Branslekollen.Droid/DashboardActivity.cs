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
        }
    }
}