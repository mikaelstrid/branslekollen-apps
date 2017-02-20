namespace Branslekollen.Core
{
    public interface ISavedState
    {
        string GetString(string key);
        void PutString(string key, string value);
        bool HasState { get; }
    }
}
