using Boitata.Models;
using Boitata.Tools.AuthUtils;
using Boitata.Tools.DataCrypt;


namespace Boitata.Tools{
    public class Shell{
        private Login? _login = null;
        private string? _key = null;

        public async Task Init(){
            Console.WriteLine("Init user context.");
            Console.Write("user: ");
            string? _user = Console.ReadLine();
            Console.Write("password: ");
            string? _password = Auth.PwdReader();
            Console.Write("\nrepeat password: ");
            string? _check_pass = Auth.PwdReader();

            if(_user is null || string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_check_pass)){
                Console.WriteLine("Invalid content!");
                return;
            }
            else if(!Auth.PwdIsSecure(_password)){
                Console.WriteLine("Waek password is detected!");
                Console.WriteLine("Need six caracters with one upper, one number and one symbol!");
                return;
            }
            else if(_password != _check_pass){
                Console.WriteLine("Password do not match!");
                return;
            }

            Login login = new Login(_user, _password);

            await login.Create();
            this._login = login;
            this._key = _password;
        }

        public async Task SingIn(){
            Console.Write("user: ");
            string? user = Console.ReadLine();
            Console.Write("password: ");
            string? password = Auth.PwdReader();

            Login login = new Login(user, password);
            
            if(! await login.SingIn(user, password)){
                Console.WriteLine("\nInvalid user or password!");
                return;
            }

            Console.WriteLine("\nLogged in.");
            this._login = login;
            this._key = password;
            return;
        }

        public async Task Get(){
            if(this._login is null || string.IsNullOrEmpty(this._key)){
                Console.WriteLine("You not logged!");
                return;
            }

            List<Data>? data = await Data.Get(this._key);

            if(data is null){
                Console.WriteLine("No content!");
                return;
            }

            foreach(Data _data in data){
                Console.WriteLine();
                Console.WriteLine($"ID: {_data.url}");
                Console.WriteLine($"User: {_data.username}");
                Console.WriteLine($"Password: {_data.password}");
            }
        }

        public async Task Set(){
            if(this._login is null || string.IsNullOrEmpty(this._key)){
                Console.WriteLine("You not logged!");
                return;
            }

            Console.Write("ID/URL: ");
            string? url = Console.ReadLine();
            Console.Write("User: ");
            string? user = Console.ReadLine();
            Console.Write("Password: ");
            string? password = Console.ReadLine();

            if(string.IsNullOrEmpty(url)
                || string.IsNullOrEmpty(user)
                || string.IsNullOrEmpty(password)){

                Console.WriteLine("Invalid content!");
                return;
            }

            bool _response = await Data.Set(this._key, new Data(url, user, password));

            if(!_response)  Console.WriteLine("Can't set data!");
            else Console.WriteLine("OK!");
        }

        public async Task Put(){
            if(this._login is null || string.IsNullOrEmpty(this._key)){
                Console.WriteLine("You not logged!");
                return;
            }

            List<Data>? _list = await Data.Get(this._key);
            
            if(_list is null) return;

            Console.Write("ID? ");
            string? _id = Console.ReadLine();

            if(string.IsNullOrEmpty(_id)) return;
            
            Data? _data = Data.Search(_id, _list);

            if(_data is null){
                Console.WriteLine("Not found!");
                return;
            }

            while(true){
                Console.Write($"Change ID [ {_data.url} ], sure? (y/n): ");
                var _key = Console.ReadKey(intercept: true);
                Console.WriteLine();
                if(_key.Key.Equals(ConsoleKey.N)) return;
                else if(_key.Key.Equals(ConsoleKey.Y)) break;
                else Console.WriteLine("Invalid option ...");
            }

            Console.Write("user: ");
            string? _user = Console.ReadLine();
            Console.Write("password: ");
            string? _password = Console.ReadLine();

            if(string.IsNullOrEmpty(_user) || string.IsNullOrEmpty(_password)) return;

            _data.username = _user;
            _data.password = _password;
    
            if(! await Data.Put(this._key, _data)) Console.WriteLine("Can not update data!");
            else Console.WriteLine("Done!");
        }
    }
}