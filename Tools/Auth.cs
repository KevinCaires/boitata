using System.Text;
using System.Security.Cryptography;
using System.Text.Unicode;


namespace Boitata.Tools.AuthUtils{
    public class Auth{
        public static bool PwdIsSecure(string password){
            if(password.Length < 6){
                return false;
            }
            else if(!password.Any(char.IsNumber) && !password.Any(char.IsUpper) && !password.Any(char.IsSymbol)){
                return false;
            }
            return true;
        }

        public static async Task<string> PwdHash(string password){
            var hashBuilder = new StringBuilder();
            using var hash = SHA256.Create();
            byte[] _hash = await hash.ComputeHashAsync(new MemoryStream(Encoding.UTF8.GetBytes(password)));

            foreach(var _byte in _hash){
                hashBuilder.Append(_byte.ToString("x2"));
            }

            return hashBuilder.ToString();
        }

        public static async Task<bool> PwdCheck(string? password, string? hash){
            if(password is null || hash is null){
                return false;
            }
            bool isEqual = SHA256.Equals(await Auth.PwdHash(password), hash);
            return isEqual;
        }
    }
}