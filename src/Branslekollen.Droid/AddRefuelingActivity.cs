using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;

namespace Branslekollen.Droid
{
    [Activity(Label = "Lägg till tankning")]
    public class AddRefuelingActivity : Activity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AddRefuelingViewModel viewModel;

            using (var scope = App.Container.BeginLifetimeScope())
            {
                viewModel = App.Container.Resolve<AddRefuelingViewModel>();
            }

            var vehicles = await viewModel.GetLocalVehicleDescriptors();

            if (!vehicles.Any())
                StartActivity(new Intent(this, typeof(CreateVehicleActivity)));

            SetContentView(Resource.Layout.AddRefueling);

            var bottomNavigationFragment = FragmentManager.FindFragmentById<BottomNavigationFragment>(Resource.Id.bottom_navigation_fragment);
            bottomNavigationFragment.SetActiveItem(Resource.Id.bottom_navigation_refuelings);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menu_save, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }
            if (item.ItemId == Resource.Id.menu_save)
            {
                OnMenuSave();
                Finish();
            }
            return base.OnOptionsItemSelected(item);
        }

        private void OnMenuSave()
        {
        }
    }
}