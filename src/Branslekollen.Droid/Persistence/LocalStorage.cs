//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;
//using Branslekollen.Core.Persistence;
//using Branslekollen.Core.ViewModels;
//using Newtonsoft.Json;
//using PCLStorage;
//using Serilog;

//namespace Branslekollen.Droid.Persistence
//{
//    public class LocalStorage : ILocalStorage
//    {
//        private const string VEHICLE_DESCRIPTORS_FILENAME = "vehicles.json";

//        public async Task<List<string>> GetVehicleIds()
//        {
//            throw new NotImplementedException();
//        //    try
//        //    {
//        //        Log.Verbose("LocalStorage.GetVehicleIds: Getting vehicle ids from the local storage.");
//        //        var rootFolder = FileSystem.Current.LocalStorage;
//        //        if (await rootFolder.CheckExistsAsync(VEHICLE_DESCRIPTORS_FILENAME) == ExistenceCheckResult.FileExists)
//        //        {
//        //            var file = await rootFolder.GetFileAsync(VEHICLE_DESCRIPTORS_FILENAME);
//        //            return JsonConvert.DeserializeObject<List<string>>(await file.ReadAllTextAsync());
//        //        }
//        //        else
//        //        {
//        //            return new List<string>();
//        //        }
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        var errorMessage = "Error when getting vehicle ids from the local storage";
//        //        Log.Warning(e, errorMessage);
//        //        throw new IOException(errorMessage, e);
//        //    }
//        }

//        public async void SaveVehicleIds(List<string> vehicleIds)
//        {
//            throw new NotImplementedException();

//            //    try
//            //    {
//            //        Log.Verbose("LocalStorage.SaveVehicleIds: Saving {@VehicleIds} to the local storage.", vehicleIds);
//            //        var rootFolder = FileSystem.Current.LocalStorage;
//            //        var file = await rootFolder.CreateFileAsync(VEHICLE_DESCRIPTORS_FILENAME, CreationCollisionOption.ReplaceExisting);
//            //        await file.WriteAllTextAsync(JsonConvert.SerializeObject(vehicleIds));
//            //    }
//            //    catch (Exception e)
//            //    {
//            //        var errorMessage = "Error when saving vehicle ids to the local storage";
//            //        Log.Warning(e, errorMessage);
//            //        throw new IOException(errorMessage, e);
//            //    }
//        }

//        public async Task<List<VehicleDescriptor>> GetVehicleDescriptors()
//        {
//            try
//            {
//                Log.Verbose("LocalStorage.GetVehicleDescriptors: Getting vehicle descriptors from local storage.");
//                var rootFolder = FileSystem.Current.LocalStorage;
//                if (await rootFolder.CheckExistsAsync(VEHICLE_DESCRIPTORS_FILENAME) == ExistenceCheckResult.FileExists)
//                {
//                    var file = await rootFolder.GetFileAsync(VEHICLE_DESCRIPTORS_FILENAME);
//                    return JsonConvert.DeserializeObject<List<VehicleDescriptor>>(await file.ReadAllTextAsync());
//                }

//                return new List<VehicleDescriptor>();
//            }
//            catch (Exception e)
//            {
//                const string errorMessage = "Error when getting vehicle ids from the local storage";
//                Log.Warning(e, errorMessage);
//                throw new IOException(errorMessage, e);
//            }
//        }
//    }
//}