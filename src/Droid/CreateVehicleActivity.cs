using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;

namespace Branslekollen.Droid
{
    // https://forums.xamarin.com/discussion/55192/material-textinputlayout-renderer-floating-labels

    [Activity(Label = "Lägg till fordon", Theme = "@style/MyCustomTheme")]
    public class CreateVehicleActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreateVehicle);

            CreateVehicleViewModel viewModel = null;

            using (var scope = App.Container.BeginLifetimeScope())
            {
                viewModel = App.Container.Resolve<CreateVehicleViewModel>();
            }

            var createVehicleButton = FindViewById<Button>(Resource.Id.CreateVehicleButton);
            createVehicleButton.Click += async (sender, e) =>
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

                if (string.IsNullOrWhiteSpace(vehicleName) || string.IsNullOrWhiteSpace(fuelType)) return;

                await viewModel.CreateVehicle(vehicleName, fuelType);

                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
        }
    }
}