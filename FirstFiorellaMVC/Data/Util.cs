using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Data
{
    public static class Util<T>
    {
        public static async Task MyCreateFileAsync(List<T> Tlist, string pathAddress, string fileName)
        {
            var Json = JsonConvert.SerializeObject(Tlist);
            await File.WriteAllTextAsync(@$"{pathAddress}\{fileName}", Json);
        }

        public static async Task MyCreateFileAsync(T Tobject, string pathAddress, string fileName)
        {
            var Json = JsonConvert.SerializeObject(Tobject);
            await File.WriteAllTextAsync(@$"{pathAddress}\{fileName}", Json);
        }

        public static T MyReadFile(string pathAddress, string fileName)
        {
            var Json = File.ReadAllText(@$"{pathAddress}\{fileName}");

            return JsonConvert.DeserializeObject<T>(Json);
        }
    }
}
