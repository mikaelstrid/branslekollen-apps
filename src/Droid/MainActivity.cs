using Android.App;
using Android.OS;

namespace Branslekollen.Droid
{
	[Activity (Label = "Branslekollen.Droid", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyCustomTheme")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);
		}
	}
}


