using Boitata.Models;

namespace Boitata.Tools{
    public class Shell{
        private Login? _login;

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

            Login login = new Login(_user, _password);

            await login.Create();
            _login = login;
        }

        public async Task Run(){
            bool shell = true;

            while(shell){
                Console.Write(">> ");
                string? command = Console.ReadLine();

                if(command is null){
                    continue;
                }

                switch(command){
                    case "init":
                        await this.Init();
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