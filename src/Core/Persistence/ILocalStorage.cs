using System.Threading.Tasks;

namespace Branslekollen.Core.Persistence
{
    public interface ILocalStorage
    {
        Task WriteAsync(string key, string contents);
        Task<string> ReadAsync(string key);
    }
}

