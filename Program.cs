using Boitata.Tools;

namespace Boitata{
    public class Program{
        public async static Task Main(){
            var _shell = new Shell();
            bool shell = true;

            while(shell){
                Console.Write("\n>> ");
                string? command = Console.ReadLine();

                if(command is null){
                    continue;
                }

                switch(command){
                    case "init":
                        await _shell.Init();
                        break;
                    case "login":
                        await _shell.SingIn();
                        break;
                    case "get":
                        await _shell.Get();
                        break;
                    case "set":
                        await _shell.Set();
                        break;
                    case "update":
                        await _shell.Put();
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
