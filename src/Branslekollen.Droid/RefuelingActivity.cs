using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Autofac;
using Branslekollen.Core;
using Branslekollen.Core.ViewModels;
using Branslekollen.Droid.Extensions;

namespace Branslekollen.Droid
{
    [Activity(MainLauncher = true, NoHistory = true)]
    public class RefuelingActivity : Activity
    {
        private RefuelingViewModel _viewModel;

        // === LIFECYCLE METHODS ===
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            using (var scope = App.Container.BeginLifetimeScope())
            {
                _viewModel = scope.Resolve<RefuelingViewModel>(
                    new NamedParameter("savedState", new AndroidSavedState(savedInstanceState)),
                    new NamedParameter("refuelingId", Intent.GetStringExtra(Constants.RefuelingIdName) ?? ""));
            }
            await _viewModel.InitializeAsync();

            if (_viewModel.FreshApplicationStart)
            {
                StartActivity(new Intent(this, typeof(SplashActivity)));
            }
            else
            {
                SetContentView(Resource.Layout.Refueling);
                InitializeBottomNavigation();
                InitializeTopToolbar();
                InitializeFormFields();
                UpdateTotalPrice();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            _viewModel.OnSaveInstanceState(new AndroidSavedState(outState));
            base.OnSaveInstanceState(outState);
        }

        private void InitializeBottomNavigation()
        {
            var bottomNavigationFragment =
                FragmentManager.FindFragmentById<BottomNavigationFragment>(Resource.Id.RefuelingBottomNavigationFragment);
            bottomNavigationFragment.SetActiveItem(Resource.Id.BottomNavigationMenuItemRefuelings);
        }

        private void InitializeTopToolbar()
        {
            var toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = Resources.GetString(Resource.String.refueling);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        private void InitializeFormFields()
        {
            FindViewById<EditText>(Resource.Id.RefuelingDateEditText).Text = _viewModel.Date;
            FindViewById<EditText>(Resource.Id.RefuelingDateEditText).Click += (sender, args) =>
            {
                var frag = DatePickerFragment.NewInstance(Date_OnSelectedDateChanged);
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            FindViewById<EditText>(Resource.Id.RefuelingPriceEditText).Text = _viewModel.Price;
            FindViewById<EditText>(Resource.Id.RefuelingPriceEditText).TextChanged += PricePerLiter_OnTextChanged;
            FindViewById<EditText>(Resource.Id.RefuelingPriceEditText).RequestFocus();

            FindViewById<EditText>(Resource.Id.RefuelingVolumeEditText).Text = _viewModel.Volume;
            FindViewById<EditText>(Resource.Id.RefuelingVolumeEditText).TextChanged += VolumeInLiters_OnTextChanged;

            FindViewById<EditText>(Resource.Id.RefuelingOdometerEditText).Text = _viewModel.Odometer;
            FindViewById<EditText>(Resource.Id.RefuelingOdometerEditText).TextChanged += OdometerInKm_OnTextChanged;

            FindViewById<Switch>(Resource.Id.RefuelingFullTankSwitch).Checked = _viewModel.FullTank;
        }



        // === MENU METHODS ===
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.TopMenuSave, menu);
            menu.FindItem(Resource.Id.MenuItemDelete).SetVisible(!string.IsNullOrWhiteSpace(_viewModel.RefuelingId));
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    GoToRefuelings();
                    return true;
                case Resource.Id.MenuItemSave:
                    OnMenuSave();
                    return true;
                case Resource.Id.MenuItemDelete:
                    var alertBuilder = new AlertDialog.Builder(this);
                    alertBuilder.SetTitle("Ta bort tankning?");
                    alertBuilder.SetMessage("Är du säker på att du vill ta bort tankningen?");
                    alertBuilder.SetPositiveButton("Ta bort", async (senderAlert, args) =>
                    {
                        await _viewModel.HandleDeleteAsync();
                        GoToRefuelings();
                    });
                    alertBuilder.SetNegativeButton("Avbryt", (senderAlert, args) => { });
                    var dialog = alertBuilder.Create();
                    dialog.Show();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void OnMenuSave()
        {
            var dateEditText = FindViewById<EditText>(Resource.Id.RefuelingDateEditText);
            if (!IsDateValid(false))
            {
                dateEditText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_date));
            }

            var pricePerLiterEditText = FindViewById<EditText>(Resource.Id.RefuelingPriceEditText);
            if (!IsPricePerLiterValid(false))
            {
                pricePerLiterEditText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_price));
            }

            var volumeInLitersEditText = FindViewById<EditText>(Resource.Id.RefuelingVolumeEditText);
            if (!IsVolumeInLitersValid(false))
            {
                volumeInLitersEditText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_volume));
            }

            var odometerInKmEditText = FindViewById<EditText>(Resource.Id.RefuelingOdometerEditText);
            if (!IsOdometerInKmValid(false))
            {
                odometerInKmEditText.ShowError(Application.Context.Resources.GetString(Resource.String.validation_error_odometer));
            }

            var fullTank = FindViewById<Switch>(Resource.Id.RefuelingFullTankSwitch).Checked;

            try
            {
                // http://stackoverflow.com/questions/14485115/synchronously-waiting-for-an-async-operation-and-why-does-wait-freeze-the-pro
                Task.Run(async () =>
                    {
                        await _viewModel.HandleSaveAsync(
                            GetDate(),
                            GetPricePerLiter(),
                            GetVolumeInLiters(),
                            GetOdometerInKm(),
                            fullTank);
                    })
                    .Wait();
                GoToRefuelings();
            }
            catch (Exception)
            {
                Toast.MakeText(this, Application.Context.Resources.GetString(Resource.String.error_adding_refueling), ToastLength.Long).Show();
            }
        }

        private void GoToRefuelings()
        {
            var intent = new Intent(this, typeof(RefuelingsActivity));
            StartActivity(intent);
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