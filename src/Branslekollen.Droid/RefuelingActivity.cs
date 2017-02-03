using System;
using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;
using Branslekollen.Droid.Extensions;
using Serilog;

namespace Branslekollen.Droid
{
    [Activity]
    public class RefuelingActivity : Activity
    {
        private RefuelingViewModelBase _viewModel;

        // === LIFECYCLE METHODS ===
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var vehicleId = Intent.GetStringExtra("VehicleId");
            var refuelingId = Intent.GetStringExtra("RefuelingId");

            Log.Verbose("AddRefuelingViewModel.OnCreate: Using VehicleId to {VehicleId}", vehicleId);
            using (var scope = App.Container.BeginLifetimeScope())
            {
                if (string.IsNullOrWhiteSpace(refuelingId))
                {
                    Log.Verbose("AddRefuelingViewModel.OnCreate: Received no refueling id, creating AddRefuelingViewModel");
                    _viewModel = App.Container.Resolve<AddRefuelingViewModel>(new NamedParameter("vehicleId", vehicleId));
                }
                else
                {
                    Log.Verbose("AddRefuelingViewModel.OnCreate: Received refueling id {refuelingId}, creating EditRefuelingViewModel", refuelingId);
                    _viewModel = App.Container.Resolve<EditRefuelingViewModel>(new NamedParameter("vehicleId", vehicleId), new NamedParameter("refuelingId", refuelingId));
                }
            }

            SetContentView(Resource.Layout.Refueling);

            var bottomNavigationFragment = FragmentManager.FindFragmentById<BottomNavigationFragment>(Resource.Id.RefuelingBottomNavigationFragment);
            bottomNavigationFragment.SetActiveItem(Resource.Id.BottomNavigationMenuItemRefuelings);

            var toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            SetActionBar(toolbar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            FindViewById<EditText>(Resource.Id.RefuelingDateEditText).Text = _viewModel.Date;
            FindViewById<EditText>(Resource.Id.RefuelingDateEditText).Click += (sender, args) =>
            {
                var frag = DatePickerFragment.NewInstance(Date_OnSelectedDateChanged);
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            FindViewById<EditText>(Resource.Id.RefuelingPriceEditText).Text = _viewModel.Price;
            FindViewById<EditText>(Resource.Id.RefuelingPriceEditText).TextChanged += PricePerLiter_OnTextChanged;

            FindViewById<EditText>(Resource.Id.RefuelingVolumeEditText).Text = _viewModel.Volume;
            FindViewById<EditText>(Resource.Id.RefuelingVolumeEditText).TextChanged += VolumeInLiters_OnTextChanged;

            FindViewById<EditText>(Resource.Id.RefuelingOdometerEditText).Text = _viewModel.Odometer;
            FindViewById<EditText>(Resource.Id.RefuelingOdometerEditText).TextChanged += OdometerInKm_OnTextChanged;

            FindViewById<Switch>(Resource.Id.RefuelingFullTankSwitch).Checked = _viewModel.FullTank;

            UpdateTotalPrice();
        }

        
        
        // === MENU METHODS ===
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.TopMenuSave, menu);
            menu.FindItem(Resource.Id.MenuItemDelete).SetVisible(_viewModel.RefuelingId != null);
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
            if (item.ItemId == Resource.Id.MenuItemDelete)
            {
                var alertBuilder = new AlertDialog.Builder(this);
                alertBuilder.SetTitle("Ta bort tankning?");
                alertBuilder.SetMessage("Är du säker på att du vill ta bort tankningen?");
                alertBuilder.SetPositiveButton("Ta bort", (senderAlert, args) => 
                { 
                    _viewModel.HandleDelete();
                    Finish();
                });
                alertBuilder.SetNegativeButton("Avbryt", (senderAlert, args) => { });
                var dialog = alertBuilder.Create();
                dialog.Show();
            }
            return base.OnOptionsItemSelected(item);
        }

        private bool OnMenuSave()
        {
            var dateEditText = FindViewById<EditText>(Resource.Id.RefuelingDateEditText);
            if (!IsDateValid(false))
            {
                dateEditText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_date));
                return false;
            }

            var pricePerLiterEditText = FindViewById<EditText>(Resource.Id.RefuelingPriceEditText);
            if (!IsPricePerLiterValid(false))
            {
                pricePerLiterEditText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_price));
                return false;
            }

            var volumeInLitersEditText = FindViewById<EditText>(Resource.Id.RefuelingVolumeEditText);
            if (!IsVolumeInLitersValid(false))
            {
                volumeInLitersEditText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_volume));
                return false;
            }

            var odometerInKmEditText = FindViewById<EditText>(Resource.Id.RefuelingOdometerEditText);
            if (!IsOdometerInKmValid(false))
            {
                odometerInKmEditText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_odometer));
                return false;
            }

            var fullTank = FindViewById<Switch>(Resource.Id.RefuelingFullTankSwitch).Checked;

            try
            {
                _viewModel.HandleSave(
                    GetDate(),
                    GetPricePerLiter(),
                    GetVolumeInLiters(),
                    GetOdometerInKm(),
                    fullTank);
                return true;
            }
            catch (Exception)
            {
                Toast.MakeText(this, Application.Context.Resources.GetString(Resource.String.error_adding_refueling), ToastLength.Long).Show();
                return false;
            }
        }



        // === EVENT HANDLERS ===

        // === DATE ===
        private void Date_OnSelectedDateChanged(DateTime selectedDate)
        {
            FindViewById<EditText>(Resource.Id.RefuelingDateEditText).Text = selectedDate.ToString("d");
        }

        private bool IsDateValid(bool allowEmpty = true)
        {
            var editText = FindViewById<EditText>(Resource.Id.RefuelingDateEditText);
            if (allowEmpty && string.IsNullOrWhiteSpace(editText.Text)) return true;
            DateTime val;
            return DateTime.TryParse(editText.Text, out val);
        }

        private DateTime GetDate()
        {
            var editText = FindViewById<EditText>(Resource.Id.RefuelingDateEditText);
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
                editText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_price));
        }

        private bool IsPricePerLiterValid(bool allowEmpty = true)
        {
            var editText = FindViewById<EditText>(Resource.Id.RefuelingPriceEditText);
            if (allowEmpty && string.IsNullOrWhiteSpace(editText.Text)) return true;
            double val;
            return double.TryParse(editText.Text, out val);
        }

        private double GetPricePerLiter()
        {
            var editText = FindViewById<EditText>(Resource.Id.RefuelingPriceEditText);
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
                editText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_volume));
        }

        private bool IsVolumeInLitersValid(bool allowEmpty = true)
        {
            var editText = FindViewById<EditText>(Resource.Id.RefuelingVolumeEditText);
            double val;
            if (allowEmpty && string.IsNullOrWhiteSpace(editText.Text)) return true;
            return double.TryParse(editText.Text, out val);
        }

        private double GetVolumeInLiters()
        {
            var editText = FindViewById<EditText>(Resource.Id.RefuelingVolumeEditText);
            return double.Parse(editText.Text);
        }


        // === ODOMETER IN KM ===
        private void OdometerInKm_OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var editText = sender as EditText;
            if (IsOdometerInKmValid())
                editText.ClearError();
            else
                editText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_odometer));
        }

        private bool IsOdometerInKmValid(bool allowEmpty = true)
        {
            var editText = FindViewById<EditText>(Resource.Id.RefuelingOdometerEditText);
            if (allowEmpty && string.IsNullOrWhiteSpace(editText.Text)) return true;
            int val;
            return int.TryParse(editText.Text, out val);
        }

        private int GetOdometerInKm()
        {
            var editText = FindViewById<EditText>(Resource.Id.RefuelingOdometerEditText);
            return int.Parse(editText.Text);
        }


        private void UpdateTotalPrice()
        {
            FindViewById<EditText>(Resource.Id.RefuelingTotalPriceEditText).Text =
                IsPricePerLiterValid(false) && IsVolumeInLitersValid(false)
                    ? $"{GetPricePerLiter()*GetVolumeInLiters():N2}"
                    : "";
        }

    }
}