using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boitata.Models{
    public class Login{
        public string? user { get; set; }
        public string? password { get; set; }
        public List<Data>? data { get; set; }

        public Login(string? user=null, string? password=null, List<Data>? data=null){
            this.user=user;
            this.password=password;
            this.data=data;
        }
    
        public async Task Create(){
            var _login = new Login(){
                user=this.user,
                password=this.password
            };

            await using FileStream createStream = File.Create($"{_login.user}.json");
            await JsonSerializer.SerializeAsync(createStream, _login);
        }
    }
}