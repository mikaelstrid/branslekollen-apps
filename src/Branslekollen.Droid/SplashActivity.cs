using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Branslekollen.Droid
{
    [Activity(NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Splash);

            var gettingStartedButton = FindViewById<Button>(Resource.Id.GettingStartedButton);
            gettingStartedButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(RefuelingActivity));
                StartActivity(intent);
            };
        }
    }
}


