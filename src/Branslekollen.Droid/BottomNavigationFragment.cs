using Android.App;
using Android.OS;
using Android.Views;

namespace Branslekollen.Droid
{
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
    }
}