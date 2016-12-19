using Android.App;
using Android.OS;

namespace Branslekollen.Droid
{
	[Activity (Theme = "@style/MyCustomTheme.NoConnection", NoHistory = true)]
	public class NoConnectionActivity : Activity
	{
	    protected override void OnCreate(Bundle bundle)
	    {
            base.OnCreate(bundle);
        }
	}
}


