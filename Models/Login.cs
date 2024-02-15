using System.Text.Json;
using System.Text.Json.Serialization;
using Boitata.Tools.AuthUtils;

namespace Boitata.Models{
    public class Login{
        public string? user { get; set; }
        public string? password { get; set; }

        public Login(string? user=null, string? password=null){
            this.user=user;
            this.password=password;
        }

        public async Task Create(){
            if(this.user is null || this.password is null){
                Console.WriteLine("Null content!");
                return;
            }

            var _login = new Login(){
                user=this.user,
                password=await Auth.PwdHash(this.password)
            };

            await using FileStream createStream = File.Create($"{_login.user}.json");
            await JsonSerializer.SerializeAsync(createStream, _login);
        }

        public async Task<bool> SingIn(string? user, string? password){
            try{
                await using FileStream readStream = File.OpenRead($"{user}.json");
                var content = await JsonSerializer.DeserializeAsync<Login>(readStream);

                if(content is null){
                    return false;
                }
                else if(! await Auth.PwdCheck(password, content.password)){
                    return false;
                }

                this.user = content.user;
                this.password = content.password;

                return true;
            }
            catch (FileNotFoundException){
                return false;
            }
        }
    }
}