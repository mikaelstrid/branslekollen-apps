using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Autofac;
using Branslekollen.Core;
using Branslekollen.Core.ViewModels;
using Serilog;

namespace Branslekollen.Droid
{
    [Activity]
    public class RefuelingsActivity : Activity
    {
        private RefuelingsViewModel _viewModel;

        // === LIFECYCLE METHODS ===
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            using (var scope = App.Container.BeginLifetimeScope())
            {
                _viewModel = scope.Resolve<RefuelingsViewModel>(new NamedParameter("savedState", new AndroidSavedState(savedInstanceState)));
            }
            await _viewModel.InitializeAsync();

            SetContentView(Resource.Layout.Refuelings);

            InitializeBottomNavigation();
            InitializeTopToolbar();
            InitializeList();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            _viewModel.OnSaveInstanceState(new AndroidSavedState(outState));
            base.OnSaveInstanceState(outState);
        }

        private void InitializeBottomNavigation()
        {
            var bottomNavigationFragment =
                FragmentManager.FindFragmentById<BottomNavigationFragment>(Resource.Id.BottomNavigationFragment);
            bottomNavigationFragment.SetActiveItem(Resource.Id.BottomNavigationMenuItemRefuelings);
        }

        private void InitializeTopToolbar()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = Resources.GetString(Resource.String.refuelings);
        }

        private void InitializeList()
        {
            var refuelingsListView = FindViewById<ListView>(Resource.Id.RefuelingsList);
            refuelingsListView.ItemClick += OnListItemClick;
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Log.Verbose("RefuelingsActivity.OnListItemClick: Position {Position} clicked", e.Position);

            var adapter = (RefuelingsAdapter)((ListView)sender).Adapter;

            var intent = new Intent(this, typeof(RefuelingActivity));
            intent.PutExtra(Constants.RefuelingIdName, adapter[e.Position].Id);
            StartActivity(intent);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await UpdateDataAsync();
        }

        private async Task UpdateDataAsync()
        {
            Log.Verbose("RefuelingsActivity.UpdateDataAsync: Updating data with vehicle id {VehicleId}", _viewModel.ActiveVehicleId);
            var items = await _viewModel.GetRefuelingsAsync();
            var refuelingsListView = FindViewById<ListView>(Resource.Id.RefuelingsList);
            refuelingsListView.Adapter = new RefuelingsAdapter(this, items.ToArray());
        }



        // === MENU METHODS ===
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.TopMenuAdd, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.MenuItemAdd:
                    var intent = new Intent(this, typeof(RefuelingActivity));
                    StartActivity(intent);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}