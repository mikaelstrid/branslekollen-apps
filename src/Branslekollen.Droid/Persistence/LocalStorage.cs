using System;
using System.IO;
using System.Threading.Tasks;
using Branslekollen.Core.Persistence;
using PCLStorage;
using Serilog;

namespace Branslekollen.Droid.Persistence
{
    public class LocalStorage : ILocalStorage
    {
        public async Task WriteJson(string key, string contents)
        {
            try
            {
                Log.Verbose("LocalStorage.WriteJson: Writing key {@Key} to local storage", key);
                var rootFolder = FileSystem.Current.LocalStorage;
                var file = await rootFolder.CreateFileAsync($"{key}.json", CreationCollisionOption.ReplaceExisting);
                await file.WriteAllTextAsync(contents);
            }
            catch (Exception e)
            {
                string errorMessage = $"Error when writing key {key} to local storage";
                Log.Warning(e, errorMessage);
                throw new IOException(errorMessage, e);
            }
        }

        public async Task<string> ReadJson(string key)
        {
            try
            {
                Log.Verbose("LocalStorage.ReadJson: Reading key {@Key} from local storage", key);
                var rootFolder = FileSystem.Current.LocalStorage;
                var fileName = $"{key}.json";
                if (await rootFolder.CheckExistsAsync(fileName) == ExistenceCheckResult.FileExists)
                {
                    var file = await rootFolder.GetFileAsync(fileName);
                    return await file.ReadAllTextAsync();
                }

                return null;
            }
            catch (Exception e)
            {
                var errorMessage = $"Error when reading key {key} from local storage";
                Log.Warning(e, errorMessage);
                throw new IOException(errorMessage, e);
            }
        }
    }
}