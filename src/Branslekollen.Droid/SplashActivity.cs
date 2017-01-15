using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Autofac;
using Branslekollen.Core.ViewModels;

namespace Branslekollen.Droid
{
	[Activity (NoHistory = true)]
	public class SplashActivity : Activity
	{
	    protected override void OnCreate(Bundle bundle)
	    {
            base.OnCreate(bundle);
        }

        protected override void OnResume()
        {
            base.OnResume();

            SetContentView(Resource.Layout.Splash);

            SplashViewModel viewModel;
            using (var scope = App.Container.BeginLifetimeScope())
            {
                viewModel = App.Container.Resolve<SplashViewModel>();
            }

            var startupWork = new Task(async () => 
            {
                try
                {
                    var vehicles = await viewModel.GetVehicles();

                    if (vehicles.Any())
                        StartActivity(new Intent(this, typeof(RefuelingsActivity)));
                    else
                        StartActivity(new Intent(this, typeof(CreateVehicleActivity)));
                }
                catch
                {
                    StartActivity(new Intent(this, typeof(NoConnectionActivity)));
                }
            });

            startupWork.Start();
        }
	}
}


