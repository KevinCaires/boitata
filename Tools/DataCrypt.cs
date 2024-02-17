using Boitata.Models;
using System.Text;
using System.Security.Cryptography;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

namespace Boitata.Tools.DataCrypt{
    public class EncryptDecrypt{
        public static async Task Encrypt(List<Data>? data, string? key){
            if(data is null || key is null){
               return; 
            }

            var _content = await Decrypt(key);

            if(_content is not null){
                foreach(var item in _content){
                    data.Add(item);
                }
            }

            string _data = JsonSerializer.Serialize(data);

            try{
                using (FileStream fileStream = new(".data", FileMode.OpenOrCreate)){
                    using(Aes aes = Aes.Create()){
                        byte[] _key = Encoding.UTF8.GetBytes(key);
                        aes.Key = _key;
                        byte[] iv = aes.IV;
                        await fileStream.WriteAsync(iv, 0, iv.Length);

                        using(CryptoStream cryptoStream = new(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write)){
                            using(StreamWriter writer = new(cryptoStream)){
                                await writer.WriteAsync(_data.ToString());
                            }
                        }
                    }
                }
            }
            catch(CryptographicException e){
                Console.WriteLine(e.ToString());
                return;
            }
        }

        public static async Task<List<Data>?> Decrypt(string? key){
            if(key is null){
                return null;
            }

            try{
                using (FileStream fileStream = new(".data", FileMode.Open)){
                    using (Aes aes = Aes.Create()){
                        byte[] iv = new byte[aes.IV.Length];
                        int bytesToRead = aes.IV.Length;
                        int bytesRead = 0;

                        while(bytesToRead > 0){
                            int n = fileStream.Read(iv, bytesRead, bytesToRead);

                            if(n == 0) break;

                            bytesRead += n;
                            bytesToRead -= n;
                        }

                        byte[] _key = Encoding.UTF8.GetBytes(key);

                        using(CryptoStream cryptoStream = new(fileStream, aes.CreateDecryptor(_key, iv), CryptoStreamMode.Read)){
                            using(StreamReader reader = new(cryptoStream)){
                                string _str = await reader.ReadToEndAsync();
                                byte[] _data = Encoding.UTF8.GetBytes(_str);
                                List<Data>? data = await JsonSerializer.DeserializeAsync<List<Data>>(new MemoryStream(_data));

                                return data;
                            }
                        }
                    }
                }
            }
            catch(CryptographicException e){
                Console.WriteLine(e.ToString());
                return null;
            }
            catch(FileNotFoundException){
                return null;
            }
        }
    }
}