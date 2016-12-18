using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Branslekollen.Core.Persistence;
using Newtonsoft.Json;
using PCLStorage;
using Serilog;

namespace Branslekollen.Droid.Persistence
{
    public class LocalStorage : ILocalStorage
    {
        private const string VEHICLE_IDS_FILENAME = "vehicles.json";

        public async Task<List<string>> GetVehicleIds()
        {
            try
            {
                Log.Verbose("LocalStorage.GetVehicleIds: Getting vehicle ids from the local storage.");
                var rootFolder = FileSystem.Current.LocalStorage;
                if (await rootFolder.CheckExistsAsync(VEHICLE_IDS_FILENAME) == ExistenceCheckResult.FileExists)
                {
                    var file = await rootFolder.GetFileAsync(VEHICLE_IDS_FILENAME);
                    return JsonConvert.DeserializeObject<List<string>>(await file.ReadAllTextAsync());
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception e)
            {
                var errorMessage = "Error when getting vehicle ids from the local storage";
                Log.Warning(e, errorMessage);
                throw new IOException(errorMessage, e);
            }
        }

        public async void SaveVehicleIds(List<string> vehicleIds)
        {
            try
            {
                Log.Verbose("LocalStorage.SaveVehicleIds: Saving {@VehicleIds} to the local storage.", vehicleIds);
                var rootFolder = FileSystem.Current.LocalStorage;
                var file = await rootFolder.CreateFileAsync(VEHICLE_IDS_FILENAME, CreationCollisionOption.ReplaceExisting);
                await file.WriteAllTextAsync(JsonConvert.SerializeObject(vehicleIds));
            }
            catch (Exception e)
            {
                var errorMessage = "Error when saving vehicle ids to the local storage";
                Log.Warning(e, errorMessage);
                throw new IOException(errorMessage, e);
            }
        }
    }
}