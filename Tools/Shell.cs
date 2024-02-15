using Boitata.Models;
using Boitata.Tools.AuthUtils;
using Boitata.Tools.DataCrypt;


namespace Boitata.Tools{
    public class Shell{
        protected Login? _login = null;
        protected string? _key = null;

        private async Task Init(){
            Console.WriteLine("Init user context.");
            Console.Write("username: ");
            string? _user = Console.ReadLine();
            Console.Write("password: ");
            string? _password = Console.ReadLine();

            if(_user is null || _password is null){
                Console.WriteLine("Invalid content!");
                return;
            }
            else if(!Auth.PwdIsSecure(_password)){
                Console.WriteLine("Waek password is detected!");
                Console.WriteLine("Need six caracters with one upper, one number and one symbol!");
                return;
            }

            Login login = new Login(_user, _password);

            await login.Create();
            this._login = login;
            this._key = _password;
        }

        private async Task SingIn(){
            Console.Write("user: ");
            string? user = Console.ReadLine();
            Console.Write("password: ");
            string? password = Console.ReadLine();

            Login login = new Login(user, password);
            
            if(! await login.SingIn(user, password)){
                Console.WriteLine("Invalid user or password!");
                return;
            }

            Console.WriteLine("Logged in.");
            this._login = login;
            this._key = password;
            return;
        }

        private async Task Set(){
            if(this._login is null){
                Console.WriteLine("You not logged!");
                return;
            }
            Console.Write("URL: ");
            string? url = Console.ReadLine();
            Console.Write("User: ");
            string? user = Console.ReadLine();
            Console.Write("Password: ");
            string? password = Console.ReadLine();

            if(string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password)){
                Console.WriteLine("Invalid content!");
                return;
            }

            List<Data> data = new List<Data>();
            data.Add(new Data(url, user, password));
            await EncryptDecrypt.Encrypt(data, this._key);
        }

        public async Task Get(){
            if(this._login is null){
                Console.WriteLine("You not logged!");
                return;
            }

            List<Data>? data = await EncryptDecrypt.Decrypt(this._key);

            if(data is null){
                Console.WriteLine("No content!");
                return;
            }

            foreach(Data _data in data){
                Console.Write(@$"
                URL: {_data.url}
                User: {_data.username}
                Password: {_data.password}
                ");
            }
        }

        public async Task Run(){
            bool shell = true;

            while(shell){
                Console.Write("\n>> ");
                string? command = Console.ReadLine();

                if(command is null){
                    continue;
                }

                switch(command){
                    case "init":
                        await this.Init();
                        break;
                    case "login":
                        await this.SingIn();
                        break;
                    case "get":
                    await this.Get();
                        break;
                    case "set":
                        await this.Set();
                        break;
                    case "exit":
                        shell = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}