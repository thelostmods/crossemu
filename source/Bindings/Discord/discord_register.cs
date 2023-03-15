namespace Discord_RPC_DotNet
{
    public static partial class DiscordRPC
    {
        public delegate void d_Discord_Register(string applicationId, string command);
        public static d_Discord_Register? Discord_Register;

        public delegate void d_Discord_RegisterSteamGame(string applicationId, string steamId);
        public static d_Discord_RegisterSteamGame? Discord_RegisterSteamGame;
    }
}