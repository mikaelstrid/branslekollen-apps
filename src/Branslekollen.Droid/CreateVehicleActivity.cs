using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;

namespace Branslekollen.Droid
{
    // https://forums.xamarin.com/discussion/55192/material-textinputlayout-renderer-floating-labels

    [Activity(Label = "L�gg till fordon")]
    public class CreateVehicleActivity : Activity
    {
        private CreateVehicleViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreateVehicle);

            using (var scope = App.Container.BeginLifetimeScope())
            {
                _viewModel = App.Container.Resolve<CreateVehicleViewModel>();
            }

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_save)
            {
                OnMenuSave();
            }
            return base.OnOptionsItemSelected(item);
        }

        private async void OnMenuSave()
        {
            var vehicleName = FindViewById<TextView>(Resource.Id.VehicleNameEditText).Text;
            string fuelType = null;

            var radioGroup = FindViewById<RadioGroup>(Resource.Id.FuelTypeRadioGroup);
            if (radioGroup.CheckedRadioButtonId == Resource.Id.FuelTypePetrolRadioButton)
            {
                fuelType = "petrol";
            }
            else if (radioGroup.CheckedRadioButtonId == Resource.Id.FuelTypeDieselRadioButton)
            {
                fuelType = "diesel";
            }

            if (string.IsNullOrWhiteSpace(vehicleName) || string.IsNullOrWhiteSpace(fuelType))
            {
                Toast.MakeText(this, "Du beh�ver fylla i ett namn p� fordonet samt v�lja en br�nsletyp", ToastLength.Long).Show();
                return;
            }

            try
            {
                await _viewModel.CreateVehicle(vehicleName, fuelType);
                var intent = new Intent(this, typeof(RefuelingsActivity));
                StartActivity(intent);
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Det gick inte att l�gga till fordonet av n�gon anledning, f�rs�k igen... :(", ToastLength.Long).Show();
            }
        }
    }
}