using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DiscordUWA.Interfaces {
    public interface ISettingsService {
        T ReadFromLocalSettings<T>(string name);
        void WriteToLocalSettings<T>(string name, T obj);
        bool LocalSettingsContainsKey(string name);

        //todo: move these?
        void SaveFileToLocalCache(byte[] data, string name);

        Task<byte[]> ReadFileFromLocalCache(string name);
        Task<bool> LocalCacheContainsFile(string name);
    }
}
