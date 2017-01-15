using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Branslekollen.Droid
{
    [Activity(Label = "Tankningar", MainLauncher = true)]
    public class RefuelingsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Refuelings);

            var bottomNavigationFragment = FragmentManager.FindFragmentById<BottomNavigationFragment>(Resource.Id.bottom_navigation_fragment);
            bottomNavigationFragment.SetActiveItem(Resource.Id.bottom_navigation_refuelings);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menu_add, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.top_menu_add)
            {
                OnTopMenuAdd();
            }
            return base.OnOptionsItemSelected(item);
        }

        private void OnTopMenuAdd()
        {
            var intent = new Intent(this, typeof(AddRefuelingActivity));
            StartActivity(intent);
        }
    }
}