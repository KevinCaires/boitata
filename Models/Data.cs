using System.Collections.Frozen;
using System.Dynamic;
using Boitata.Tools.DataCrypt;

namespace Boitata.Models{
    public class Data{
        public string url { get; set; } = null!;
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;

        public Data(string url, string username, string password){
            this.url = url;
            this.username = username;
            this.password = password;
        }

        public static Data? Search(string id, List<Data> data){
            foreach(var item in data){
                if(item.url == id) return item;
            }
            return null;
        }

        public static async Task<List<Data>?> Get(string key){
            List<Data>? data = await EncryptDecrypt.Decrypt(key);
            return data;
        }

        public static async Task<bool> Set(string key, Data data){

            List<Data>? _all_data = await EncryptDecrypt.Decrypt(key);

            if(_all_data is null){
                List<Data> _new = new List<Data>();
                await EncryptDecrypt.Encrypt(_new, key);
                return true;
            }
                
            Data? _data = Search(data.url, _all_data);
            if(_data is not null) return false;

            _all_data.Add(data);
            await EncryptDecrypt.Encrypt(_all_data, key);
            return true;
        }

        public static async Task<bool> Put(string key, Data data){
            List<Data>? _all_data = await EncryptDecrypt.Decrypt(key);
            if(_all_data is null) return false;
            Data? _old = Search(data.url, _all_data);
            if(_old is null) return false;

            int idx = _all_data.IndexOf(_old);
            _all_data.RemoveAt(idx);
            _all_data.Add(data);
            await EncryptDecrypt.Encrypt(_all_data, key);

            return true;
        }
    }
}
