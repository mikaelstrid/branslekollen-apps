using Android.Content.Res;

namespace Branslekollen.Droid
{
    public static class Utilities
    {
        public static int ConvertDpToPixels(Resources resources, int dpValue)
        {
            return (int)(dpValue * resources.DisplayMetrics.Density);
        }
    }
}