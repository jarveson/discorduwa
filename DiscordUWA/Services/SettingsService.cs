using DiscordUWA.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DiscordUWA.Services {
    public class SettingsService : ISettingsService {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        // Should this be temporary folder instead? bleh
        StorageFolder localCacheFolder = ApplicationData.Current.LocalCacheFolder;

        public T ReadFromLocalSettings<T>(String name) {
            String content = (String)localSettings.Values[name];
            return JsonConvert.DeserializeObject<T>(content);
        }

        public void WriteToLocalSettings<T>(String name, T obj) {
            try {
                localSettings.Values[name] = JsonConvert.SerializeObject(obj); ;
            }
            catch (Exception e) {
                // idk what to do about this yet
                throw e;
            }
        }

        public bool LocalSettingsContainsKey(string name) {
            return localSettings.Values.ContainsKey(name);
        }


        // todo -- move these to like a 'cache' service or something
        public async void SaveFileToLocalCache(byte[] data, string name) {
            StorageFile cacheFile = await localCacheFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(cacheFile, data);
        }

        public async Task<bool> LocalCacheContainsFile(string name) {
            return await localCacheFolder.TryGetItemAsync(name) != null;
        }

        public async Task<byte[]> ReadFileFromLocalCache(string name) {
            StorageFile file = await localCacheFolder.GetFileAsync(name);
            IBuffer data = await FileIO.ReadBufferAsync(file);
            return data.ToArray();
        }
    }
}
