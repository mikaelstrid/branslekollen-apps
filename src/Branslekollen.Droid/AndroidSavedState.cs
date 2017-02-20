using Android.OS;
using Branslekollen.Core;

namespace Branslekollen.Droid
{
    public class AndroidSavedState : ISavedState
    {
        private readonly Bundle _bundle;

        public AndroidSavedState(Bundle bundle)
        {
            _bundle = bundle;
        }

        public string GetString(string key)
        {
            return _bundle.GetString(key);
        }

        public void PutString(string key, string value)
        {
            _bundle.PutString(key, value);
        }

        public bool HasState => _bundle != null;
    }
}