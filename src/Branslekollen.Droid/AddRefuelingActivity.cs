using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;

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

            FindViewById<EditText>(Resource.Id.PricePerLiterEditText).TextChanged += OnTextChanged;
            FindViewById<EditText>(Resource.Id.VolumeInLitersEditText).TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            double pricePerLiter;
            if (!double.TryParse(FindViewById<EditText>(Resource.Id.PricePerLiterEditText).Text, out pricePerLiter))
            {
                FindViewById<EditText>(Resource.Id.TotalPriceEditText).Text = "";
                return;
            }

            double volumeInLiters;
            if (!double.TryParse(FindViewById<EditText>(Resource.Id.VolumeInLitersEditText).Text, out volumeInLiters))
            {
                FindViewById<EditText>(Resource.Id.TotalPriceEditText).Text = "";
                return;
            }

            FindViewById<EditText>(Resource.Id.TotalPriceEditText).Text = $"{pricePerLiter*volumeInLiters:N2}";
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

        private bool OnMenuSave()
        {
            DateTime date;
            if (!DateTime.TryParse(FindViewById<EditText>(Resource.Id.DateEditText).Text, out date))
            {
                Toast.MakeText(this, "Du behöver fylla i ett korrekt datum", ToastLength.Long).Show();
                return false;
            }
            
            double pricePerLiter;
            if (!double.TryParse(FindViewById<EditText>(Resource.Id.PricePerLiterEditText).Text, out pricePerLiter))
            {
                Toast.MakeText(this, "Du behöver fylla i ett korrekt pris per liter", ToastLength.Long).Show();
                return false;
            }

            double volumeInLiters;
            if (!double.TryParse(FindViewById<EditText>(Resource.Id.VolumeInLitersEditText).Text, out volumeInLiters))
            {
                Toast.MakeText(this, "Du behöver fylla i en korrekt volym i liter", ToastLength.Long).Show();
                return false;
            }

            int odometerInKm;
            if (!int.TryParse(FindViewById<EditText>(Resource.Id.OdometerInKmEditText).Text, out odometerInKm))
            {
                Toast.MakeText(this, "Du behöver fylla i en korrekt mätarställning", ToastLength.Long).Show();
                return false;
            }

            var fullTank = FindViewById<Switch>(Resource.Id.FullTankSwitch).Checked;
            
            try
            {
                _viewModel.AddRefueling("vehicleId", date, pricePerLiter, volumeInLiters, odometerInKm, fullTank);
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