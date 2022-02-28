using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Data
{
    public static class MyFileUtil<T>
    {
        public static async Task MyCreateFileAsync(List<T> TList, string pathAddress, string fileName)
        {
            var Json = JsonConvert.SerializeObject(TList);
            await File.WriteAllTextAsync(@$"{pathAddress}\{fileName}", Json);
        }

        public static async Task MyCreateFileAsync(T TObject, string pathAddress, string fileName)
        {
            var Json = JsonConvert.SerializeObject(TObject);
            await File.WriteAllTextAsync(@$"{pathAddress}\{fileName}", Json);
        }

        public static T MyReadFile(string pathAddress, string fileName)
        {
            var Json = File.ReadAllText(@$"{pathAddress}\{fileName}");

            return JsonConvert.DeserializeObject<T>(Json);
        }
    }
}
