using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;
using Serilog;

namespace Branslekollen.Droid
{
    [Activity]
    public class StatisticsActivity : Activity
    {
        private StatisticsViewModel _viewModel;

        private TextView _averageConsumptionTextView;

        // === LIFECYCLE METHODS ===
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            using (var scope = App.Container.BeginLifetimeScope())
            {
                _viewModel = scope.Resolve<StatisticsViewModel>(new NamedParameter("savedState", new AndroidSavedState(savedInstanceState)));
            }
            Task.Run(async () => { await _viewModel.InitializeAsync(); }).Wait();

            SetContentView(Resource.Layout.Statistics);
            _averageConsumptionTextView = FindViewById<TextView>(Resource.Id.StatisticsAverageConsumptionTextView);

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
            bottomNavigationFragment.SetActiveItem(Resource.Id.BottomNavigationMenuItemStatistics);
        }

        private void InitializeTopToolbar()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = Resources.GetString(Resource.String.statistics);
        }

        protected override void OnStart()
        {
            base.OnStart();
            Task.Run(async () => { await UpdateDataAsync(); }).Wait();
        }

        private async Task UpdateDataAsync()
        {
            Log.Verbose("StatisticsActivity.UpdateDataAsync: Updating data with vehicle id {VehicleId}", _viewModel.ActiveVehicleId);
            var averageConsumptionAsLiterPerKm = await _viewModel.GetAverageConsumptionAsLiterPerKmAsync();

            var textSize = averageConsumptionAsLiterPerKm.HasValue ? 56 : 20;
            var text = averageConsumptionAsLiterPerKm.HasValue 
                ? $"{averageConsumptionAsLiterPerKm * 10:N2} l/mil" 
                : GetString(Resource.String.not_enough_refuelings_to_calculate_average);

            RunOnUiThread(() =>
            {
                _averageConsumptionTextView.TextSize = textSize;
                _averageConsumptionTextView.Text = text;
            });
        }

        public override void OnBackPressed()
        {
            StartActivity(new Intent(this, typeof(RefuelingsActivity)));
            Finish();
        }
    }
}