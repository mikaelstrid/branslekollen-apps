using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Branslekollen.Droid
{
	[Activity (Label = "Branslekollen.Droid", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyCustomTheme.NoActionBar")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
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


