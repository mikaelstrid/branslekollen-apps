using Android.App;
using Android.OS;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;
using Serilog;

namespace Branslekollen.Droid
{
    [Activity(Label = "Statistik", NoHistory = true)]
    public class StatisticsActivity : Activity
    {
        private StatisticsViewModel _viewModel;

        // === LIFECYCLE METHODS ===
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewModel = App.Container.Resolve<StatisticsViewModel>(
                new NamedParameter("savedState", new AndroidSavedState(savedInstanceState)));
            await _viewModel.Initialize();

            SetContentView(Resource.Layout.Statistics);

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
            var bottomNavigationFragment = FragmentManager.FindFragmentById<BottomNavigationFragment>(Resource.Id.BottomNavigationFragment);
            bottomNavigationFragment.SetActiveItem(Resource.Id.BottomNavigationMenuItemRefuelings);
        }

        private void InitializeTopToolbar()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            SetActionBar(toolbar);
        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdateData();
        }

        private async void UpdateData()
        {
            Log.Verbose("StatisticsActivity.UpdateData: Updating data with vehicle id {VehicleId}", _viewModel.ActiveVehicleId);
            var averageConsumptionAsLiterPerKm = await _viewModel.GetAverageConsumptionAsLiterPerKm();
            var averageConsumptionTextView = FindViewById<TextView>(Resource.Id.StatisticsAverageConsumptionTextView);
            if (averageConsumptionAsLiterPerKm.HasValue)
            {
                averageConsumptionTextView.TextSize = 56;
                averageConsumptionTextView.Text = $"{averageConsumptionAsLiterPerKm*10:N2} l/mil";
            }
            else
            {
                averageConsumptionTextView.TextSize = 20;
                averageConsumptionTextView.Text = GetString(Resource.String.not_enough_refuelings_to_calculate_average);
            }
        }
    }
}