using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;

namespace Branslekollen.Droid
{
    [Activity(Label = "Ny tankning")]
    public class AddRefuelingActivity : Activity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreateVehicle);

            AddRefuelingViewModel viewModel;

            using (var scope = App.Container.BeginLifetimeScope())
            {
                viewModel = App.Container.Resolve<AddRefuelingViewModel>();
            }

            var vehicles = await viewModel.GetLocalVehicleDescriptors();

            if (!vehicles.Any())
                StartActivity(new Intent(this, typeof(CreateVehicleActivity)));

            SetContentView(Resource.Layout.AddRefueling);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            //ActionBar.Title = "My Toolbar 2";
        }
    }
}