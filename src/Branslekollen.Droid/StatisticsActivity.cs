using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;
using Serilog;

namespace Branslekollen.Droid
{
    [Activity(Label = "Statistik")]
    public class StatisticsActivity : Activity
    {
        private StatisticsViewModel _viewModel;

        // === LIFECYCLE METHODS ===
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            _viewModel = App.Container.Resolve<StatisticsViewModel>();

            SetContentView(Resource.Layout.Statistics);

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
                Log.Verbose("StatisticsActivity.OnResume: Using ActiveVehicleId {ActiveVehicledId}", _viewModel.ActiveVehicleId);
                UpdateData();
            }
            else
            {
                var vehicles = await _viewModel.GetLocalVehicleDescriptors();
                if (!vehicles.Any())
                {
                    Log.Verbose("StatisticsActivity.OnResume: No vehicle descriptors found, redirecting to CreateVehicleActivity");
                    StartActivity(new Intent(this, typeof(CreateVehicleActivity)));
                }
                else
                {
                    _viewModel.ActiveVehicleId = vehicles.First().Id;
                    Log.Verbose("StatisticsActivity.OnResume: Received descriptors from local storage, using activeVehicleId {ActiveVehicledId}", _viewModel.ActiveVehicleId);
                    UpdateData();
                }
            }
        }

        private async void UpdateData()
        {
            var averageConsumption = await _viewModel.GetAverageConsumption();
            var averageConsumptionTextView = FindViewById<TextView>(Resource.Id.StatisticsAverageConsumptionTextView);
            if (averageConsumption.HasValue)
            {
                averageConsumptionTextView.TextSize = 56;
                averageConsumptionTextView.Text = $"{averageConsumption:N2} l/mil";
            }
            else
            {
                averageConsumptionTextView.TextSize = 20;
                averageConsumptionTextView.Text = GetString(Resource.String.not_enough_refuelings_to_calculate_average);
            }
        }



        // === MENU METHODS ===
        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.TopMenuAdd, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    if (item.ItemId == Resource.Id.MenuItemAdd)
        //    {
        //        OnTopMenuAdd();
        //    }
        //    return base.OnOptionsItemSelected(item);
        //}

        //private void OnTopMenuAdd()
        //{
        //    var intent = new Intent(this, typeof(RefuelingActivity));
        //    intent.PutExtra("VehicleId", _viewModel.ActiveVehicleId);
        //    StartActivity(intent);
        //}
    }
}