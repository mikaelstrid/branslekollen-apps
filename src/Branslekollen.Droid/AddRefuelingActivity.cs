using System;
using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;
using Branslekollen.Droid.Extensions;

namespace Branslekollen.Droid
{
    [Activity(Label = "Lägg till tankning")]
    public class AddRefuelingActivity : Activity
    {
        private AddRefuelingViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            using (var scope = App.Container.BeginLifetimeScope())
            {
                _viewModel = App.Container.Resolve<AddRefuelingViewModel>();
            }

            SetContentView(Resource.Layout.AddRefueling);

            var bottomNavigationFragment = FragmentManager.FindFragmentById<BottomNavigationFragment>(Resource.Id.BottomNavigationFragment);
            bottomNavigationFragment.SetActiveItem(Resource.Id.BottomNavigationMenuItemRefuelings);

            var toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            SetActionBar(toolbar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            FindViewById<EditText>(Resource.Id.DateEditText).TextChanged += Date_OnTextChanged;
            FindViewById<EditText>(Resource.Id.PricePerLiterEditText).TextChanged += PricePerLiter_OnTextChanged;
            FindViewById<EditText>(Resource.Id.VolumeInLitersEditText).TextChanged += VolumeInLiters_OnTextChanged;
            FindViewById<EditText>(Resource.Id.OdometerInKmEditText).TextChanged += OdometerInKm_OnTextChanged;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.TopMenuSave, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
            }
            if (item.ItemId == Resource.Id.MenuItemSave)
            {
                if (OnMenuSave())
                    Finish();
            }
            return base.OnOptionsItemSelected(item);
        }


        // === DATE ===
        private void Date_OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var editText = sender as EditText;
            if (IsDateValid())
                editText.ClearError();
            else
                editText.ShowError("Du behöver fylla i ett korrekt datum");
        }

        private bool IsDateValid(bool allowEmpty = true)
        {
            var editText = FindViewById<EditText>(Resource.Id.DateEditText);
            if (allowEmpty && string.IsNullOrWhiteSpace(editText.Text)) return true;
            DateTime val;
            return DateTime.TryParse(editText.Text, out val);
        }

        private DateTime GetDate()
        {
            var editText = FindViewById<EditText>(Resource.Id.DateEditText);
            return DateTime.Parse(editText.Text);
        }
        

        // === PRICE PER LITER ===
        private void PricePerLiter_OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var editText = sender as EditText;
            if (IsPricePerLiterValid())
            {
                editText.ClearError();
                UpdateTotalPrice();
            }
            else
                editText.ShowError("Du behöver fylla i ett korrekt pris per liter");
        }

        private bool IsPricePerLiterValid(bool allowEmpty = true)
        {
            var editText = FindViewById<EditText>(Resource.Id.PricePerLiterEditText);
            if (allowEmpty && string.IsNullOrWhiteSpace(editText.Text)) return true;
            double val;
            return double.TryParse(editText.Text, out val);
        }

        private double GetPricePerLiter()
        {
            var editText = FindViewById<EditText>(Resource.Id.PricePerLiterEditText);
            return double.Parse(editText.Text);
        }


        // === VOLUME IN LITERS ===
        private void VolumeInLiters_OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var editText = sender as EditText;
            if (IsVolumeInLitersValid())
            {
                editText.ClearError();
                UpdateTotalPrice();
            }
            else
                editText.ShowError("Du behöver fylla i en korrekt volym i liter");
        }

        private bool IsVolumeInLitersValid(bool allowEmpty = true)
        {
            var editText = FindViewById<EditText>(Resource.Id.VolumeInLitersEditText);
            double val;
            if (allowEmpty && string.IsNullOrWhiteSpace(editText.Text)) return true;
            return double.TryParse(editText.Text, out val);
        }

        private double GetVolumeInLiters()
        {
            var editText = FindViewById<EditText>(Resource.Id.VolumeInLitersEditText);
            return double.Parse(editText.Text);
        }


        // === ODOMETER IN KM ===
        private void OdometerInKm_OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var editText = sender as EditText;
            if (IsOdometerInKmValid())
                editText.ClearError();
            else
                editText.ShowError("Du behöver fylla i en korrekt mätarställning");
        }

        private bool IsOdometerInKmValid(bool allowEmpty = true)
        {
            var editText = FindViewById<EditText>(Resource.Id.OdometerInKmEditText);
            if (allowEmpty && string.IsNullOrWhiteSpace(editText.Text)) return true;
            int val;
            return int.TryParse(editText.Text, out val);
        }

        private int GetOdometerInKm()
        {
            var editText = FindViewById<EditText>(Resource.Id.OdometerInKmEditText);
            return int.Parse(editText.Text);
        }


        private void UpdateTotalPrice()
        {
            FindViewById<EditText>(Resource.Id.TotalPriceEditText).Text =
                IsPricePerLiterValid(false) && IsVolumeInLitersValid(false)
                    ? $"{GetPricePerLiter()*GetVolumeInLiters():N2}"
                    : "";
        }

        private bool OnMenuSave()
        {
            var dateEditText = FindViewById<EditText>(Resource.Id.DateEditText);
            if (!IsDateValid(false))
            {
                dateEditText.ShowError("Du behöver fylla i ett korrekt datum");
                return false;
            }
            
            var pricePerLiterEditText = FindViewById<EditText>(Resource.Id.PricePerLiterEditText);
            if (!IsPricePerLiterValid(false))
            {
                pricePerLiterEditText.ShowError("Du behöver fylla i ett korrekt pris per liter");
                return false;
            }

            var volumeInLitersEditText = FindViewById<EditText>(Resource.Id.VolumeInLitersEditText);
            if (!IsVolumeInLitersValid(false))
            {
                volumeInLitersEditText.ShowError("Du behöver fylla i en korrekt volym i liter");
                return false;
            }

            var odometerInKmEditText = FindViewById<EditText>(Resource.Id.OdometerInKmEditText);
            if (!IsOdometerInKmValid(false))
            {
                odometerInKmEditText.ShowError("Du behöver fylla i en korrekt mätarställning");
                return false;
            }

            var fullTank = FindViewById<Switch>(Resource.Id.FullTankSwitch).Checked;
            
            try
            {
                _viewModel.AddRefueling(
                    "vehicleId", 
                    GetDate(), 
                    GetPricePerLiter(), 
                    GetVolumeInLiters(), 
                    GetOdometerInKm(), 
                    fullTank);
                return true;
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Det gick inte att lägga till tankningen av någon anledning, försök igen... :(", ToastLength.Long).Show();
                return false;
            }
        }
    }
}