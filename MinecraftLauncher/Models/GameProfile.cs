namespace MinecraftLauncher.Models
{
    public class GameProfile
    {
        public string Name { get; set; } = string.Empty;
        public string GameDirectory { get; set; } = string.Empty;
        public string MinecraftVersion { get; set; } = string.Empty;
        public string JavaPath { get; set; } = string.Empty;
        public string LauncherVersion { get; set; } = "1.0.0";
        public DateTime LastUsed { get; set; } = DateTime.Now;
        public bool IsDefault { get; set; } = true;
    }
}