using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Autofac;
using Branslekollen.Core.ViewModels;

namespace Branslekollen.Droid
{
	[Activity (Label = "Branslekollen.Main", Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            MainViewModel viewModel;

            using (var scope = App.Container.BeginLifetimeScope())
            {
                viewModel = App.Container.Resolve<MainViewModel>();
            }

		    if ((await viewModel.GetVehicles()).Any())
		    {
                var intent = new Intent(this, typeof(RefuelingsActivity));
                StartActivity(intent);
            }

            SetContentView (Resource.Layout.Main);

            var gettingStartedButton = FindViewById<Button>(Resource.Id.GettingStartedButton);
            gettingStartedButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CreateVehicleActivity));
                StartActivity(intent);
            };
        }
	}
}


