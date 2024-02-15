using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boitata.Models{
    public class Data{
        public string? url { get; set; }
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
    }
}