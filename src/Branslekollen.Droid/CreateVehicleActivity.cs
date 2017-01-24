using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;
using Branslekollen.Droid.Extensions;

namespace Branslekollen.Droid
{
    // https://forums.xamarin.com/discussion/55192/material-textinputlayout-renderer-floating-labels

    [Activity(Label = "Lägg till fordon", NoHistory = true)]
    public class CreateVehicleActivity : Activity
    {
        private CreateVehicleViewModel _viewModel;

        // === LIFECYCLE METHODS ===
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreateVehicle);

            _viewModel = App.Container.Resolve<CreateVehicleViewModel>();

            var toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            SetActionBar(toolbar);

            FindViewById<EditText>(Resource.Id.VehicleNameEditText).TextChanged += VehicleNameEditText_OnTextChanged;
        }



        // === MENU METHODS ===
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.TopMenuSave, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.MenuItemSave)
            {
                if (OnMenuSave()) StartActivity(new Intent(this, typeof(RefuelingsActivity)));
            }
            return base.OnOptionsItemSelected(item);
        }

        private bool OnMenuSave()
        {
            var vehicleNameEditText = FindViewById<EditText>(Resource.Id.VehicleNameEditText);
            if (!IsVehicleNameValid(false))
            {
                vehicleNameEditText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_vehicle_name));
                return false;
            }

            if (!IsFuelTypeValid())
            {
                Toast.MakeText(this, "Du behöver fylla i ett namn på fordonet samt välja en bränsletyp", ToastLength.Long).Show();
            }

            try
            {
                var vehicle = _viewModel.CreateVehicle(GetVehicleName(), GetFuelType()).Result;
                return true;
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Det gick inte att lägga till fordonet av någon anledning, försök igen... :(", ToastLength.Long).Show();
                return false;
            }
        }



        // === EVENT HANDLERS ===

        // === VEHICLE NAME ===
        private void VehicleNameEditText_OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var editText = sender as EditText;
            if (IsVehicleNameValid())
                editText.ClearError();
            else
                editText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_vehicle_name));
        }

        private bool IsVehicleNameValid(bool allowEmpty = true)
        {
            var editText = FindViewById<EditText>(Resource.Id.VehicleNameEditText);
            if (allowEmpty && string.IsNullOrWhiteSpace(editText.Text)) return true;
            return !string.IsNullOrWhiteSpace(editText.Text);
        }

        private string GetVehicleName()
        {
            var editText = FindViewById<EditText>(Resource.Id.VehicleNameEditText);
            return editText.Text;
        }

        // === FUEL TYPE ===
        private bool IsFuelTypeValid()
        {
            var radioGroup = FindViewById<RadioGroup>(Resource.Id.FuelTypeRadioGroup);
            return 
                radioGroup.CheckedRadioButtonId == Resource.Id.FuelTypePetrolRadioButton 
                || radioGroup.CheckedRadioButtonId == Resource.Id.FuelTypeDieselRadioButton;
        }

        private string GetFuelType()
        {
            var radioGroup = FindViewById<RadioGroup>(Resource.Id.FuelTypeRadioGroup);
            switch (radioGroup.CheckedRadioButtonId)
            {
                case Resource.Id.FuelTypePetrolRadioButton:
                    return "petrol";
                case Resource.Id.FuelTypeDieselRadioButton:
                    return "diesel";
                default:
                    return null;
            }
        }
    }
}