using System.Threading.Tasks;

namespace Branslekollen.Core.Persistence
{
    public interface ILocalStorage
    {
        Task WriteJson(string key, string contents);
        Task<string> ReadJson(string key);
    }
}

