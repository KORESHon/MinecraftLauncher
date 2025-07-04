using System.Collections.Generic;

namespace MinecraftLauncher.Models
{
    public class ServerInfo
    {
        public string MinecraftVersion { get; set; } = string.Empty;
        public string ModsHash { get; set; } = string.Empty;
        public List<ClientFile> ClientFiles { get; set; } = new List<ClientFile>();
        public List<ModFile> Mods { get; set; } = new List<ModFile>();
    }

    public class ClientFile
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
    }

    public class ModFile
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
    }
}