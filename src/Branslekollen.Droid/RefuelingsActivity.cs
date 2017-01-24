using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;
using Serilog;

namespace Branslekollen.Droid
{
    [Activity(Label = "Tankningar", MainLauncher = true)]
    public class RefuelingsActivity : Activity
    {
        private RefuelingsViewModel _viewModel;

        // === LIFECYCLE METHODS ===
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            _viewModel = App.Container.Resolve<RefuelingsViewModel>();

            SetContentView(Resource.Layout.Refuelings);

            var bottomNavigationFragment = FragmentManager.FindFragmentById<BottomNavigationFragment>(Resource.Id.BottomNavigationFragment);
            bottomNavigationFragment.SetActiveItem(Resource.Id.BottomNavigationMenuItemRefuelings);

            var toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            SetActionBar(toolbar);
        }

        protected override async void OnResume()
        {
            base.OnResume();

            if (!string.IsNullOrWhiteSpace(_viewModel.ActiveVehicleId))
            {
                Log.Verbose("RefuelingsActivity.OnResume: Using ActiveVehicleId {ActiveVehicledId}", _viewModel.ActiveVehicleId);
                UpdateData();
            }
            else
            {
                var vehicles = await _viewModel.GetLocalVehicleDescriptors();
                if (!vehicles.Any())
                {
                    Log.Verbose("RefuelingsActivity.OnResume: No vehicle descriptors found, redirecting to CreateVehicleActivity");
                    StartActivity(new Intent(this, typeof(CreateVehicleActivity)));
                }
                else
                {
                    _viewModel.ActiveVehicleId = vehicles.First().Id;
                    Log.Verbose("RefuelingsActivity.OnResume: Received descriptors from local storage, using activeVehicleId {ActiveVehicledId}", _viewModel.ActiveVehicleId);
                    UpdateData();
                }
            }
        }

        private async void UpdateData()
        {
            var items = (await _viewModel.GetRefuelings()).Select(r => $"{r.RefuelingDate:yy-MM-dd}: {r.NumberOfLiters:N2}l").ToList();
            var refuelingsListView = FindViewById<ListView>(Resource.Id.RefuelingsList);
            refuelingsListView.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
        }



        // === MENU METHODS ===
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.TopMenuAdd, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.MenuItemAdd)
            {
                OnTopMenuAdd();
            }
            return base.OnOptionsItemSelected(item);
        }

        private void OnTopMenuAdd()
        {
            var intent = new Intent(this, typeof(AddRefuelingActivity));
            intent.PutExtra("VehicleId", _viewModel.ActiveVehicleId);
            StartActivity(intent);
        }
    }
}