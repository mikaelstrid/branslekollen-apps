using Android.App;
using Android.OS;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;

namespace Branslekollen.Droid
{
    [Activity(Label = "Profil", NoHistory = true)]
    public class ProfileActivity : Activity
    {
        private ProfileViewModel _viewModel;

        // === LIFECYCLE METHODS ===
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewModel = App.Container.Resolve<ProfileViewModel>(
                new NamedParameter("savedState", new AndroidSavedState(savedInstanceState)));
            await _viewModel.Initialize();

            SetContentView(Resource.Layout.Profile);

            var deleteAllVehiclesButton = FindViewById<Button>(Resource.Id.DeleteAllVehiclesButton);
            deleteAllVehiclesButton.Click += (sender, e) => _viewModel.DeleteAllVehicles();

            InitializeBottomNavigation();
            InitializeTopToolbar();
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
        }
    }
}