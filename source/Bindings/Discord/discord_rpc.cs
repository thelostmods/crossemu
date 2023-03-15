using System;
using System.IO;

using DynaLib_DotNet;

namespace Discord_RPC_DotNet
{
    public static partial class DiscordRPC
    {
        public static bool Initialized { get; private set; } = TryInitialize();
        public static bool TryInitialize(string scanDirectory = "!")
        {
            if (Initialized) return true;

            var lib = new DynaLib();
            if (!lib.LoadFromConfig("discord-rpc", "discord"))
            {
                if (!lib.Load(new BindDef() {
                    NameLinux = "libdiscord-rpc",
                    NameOSX = "libdiscord-rpc",
                    NameWindows = "discord-rpc",
                    LoadPath = scanDirectory,
                    DepPath = "!"
                })) {
                    if (DynaLib_DotNet.Conditionals.DL_ERROR)
                        Godot.GD.Print("There was a problem loading discord-rpc");
                    return false;
                }
            }

            // Register
            lib.Bind(ref Discord_Register, "Discord_Register");
            lib.Bind(ref Discord_RegisterSteamGame, "Discord_RegisterSteamGame");

            // RPC
            lib.Bind(ref Discord_Initialize, "Discord_Initialize");
            lib.Bind(ref Discord_Initialize_Managed, "Discord_Initialize");
            lib.Bind(ref Discord_Shutdown, "Discord_Shutdown");
            lib.Bind(ref Discord_RunCallbacks, "Discord_RunCallbacks");

            if (lib.Bind(ref Discord_UpdateConnection, "Discord_UpdateConnection"))
            {
                if (DynaLib_DotNet.Conditionals.DL_NOTIFICATION)
                    Godot.GD.Print("Dynalib-Conditional: [Discord-RPC] Disable-IO-Thread active.");
                Conditionals.DisableIOThread = true;
            }
            else Conditionals.DisableIOThread = false;

            lib.Bind(ref Discord_UpdatePresence, "Discord_UpdatePresence");
            lib.Bind(ref Discord_ClearPresence, "Discord_ClearPresence");
            lib.Bind(ref Discord_Respond, "Discord_Respond");
            lib.Bind(ref Discord_UpdateHandlers, "Discord_UpdateHandlers");

            Initialized = true;
            return true;
        }

        [Serializable]
        public struct DiscordRichPresence
        {
            public string state;   /* max 128 bytes */
            public string details; /* max 128 bytes */
            public long startTimestamp;
            public long endTimestamp;
            public string largeImageKey;  /* max 32 bytes */
            public string largeImageText; /* max 128 bytes */
            public string smallImageKey;  /* max 32 bytes */
            public string smallImageText; /* max 128 bytes */
            public string partyId;        /* max 128 bytes */
            public int partySize;
            public int partyMax;
            public int partyPrivacy;
            public string matchSecret;    /* max 128 bytes */
            public string joinSecret;     /* max 128 bytes */
            public string spectateSecret; /* max 128 bytes */
            public byte instance;
        }

        [Serializable]
        public struct DiscordUser
        {
            public string userId;
            public string username;
            public string discriminator;
            public string avatar;
        }

        [Serializable]
        public struct DiscordEventHandlers
        {
            public delegate void d_ready(ref DiscordUser request);
            public delegate void d_disconnected(int errorCode, string message);
            public delegate void d_errored(int errorCode, string message);
            public delegate void d_joinGame(string joinSecret);
            public delegate void d_spectateGame(string spectateSecret);
            public delegate void d_joinRequest(ref DiscordUser request);

            public d_ready? ready;
            public d_disconnected? disconnected;
            public d_errored? errored;
            public d_joinGame? joinGame;
            public d_spectateGame? spectateGame;
            public d_joinRequest? joinRequest;
    }

        public enum DiscordReply : int
        {
            NO      = 0,
            YES     = 1,
            IGNORE  = 2,
            PRIVATE = 0,
            PUBLIC  = 1
        }

        public delegate void d_Discord_Initialize(string applicationId, IntPtr handlers, int autoRegister, string? optionalSteamId);
        public static d_Discord_Initialize? Discord_Initialize;

        public delegate void d_Discord_Initialize_Managed(string applicationId, ref DiscordEventHandlers handlers, bool autoRegister, string? optionalSteamId);
        public static d_Discord_Initialize_Managed? Discord_Initialize_Managed;

        public delegate void d_Discord_Shutdown();
        public static d_Discord_Shutdown? Discord_Shutdown;

        /* checks for incoming messages, dispatches callbacks */
        public delegate void d_Discord_RunCallbacks();
        public static d_Discord_RunCallbacks? Discord_RunCallbacks;

        /* If you disable the lib starting its own io thread, you'll need to call this from your own */
        public delegate void d_Discord_UpdateConnection();
        public static d_Discord_UpdateConnection? Discord_UpdateConnection;

        public delegate void d_Discord_UpdatePresence(ref DiscordRichPresence presence);
        public static d_Discord_UpdatePresence? Discord_UpdatePresence;

        public delegate void d_Discord_ClearPresence();
        public static d_Discord_ClearPresence? Discord_ClearPresence;

        public delegate void d_Discord_Respond(string userid, DiscordReply reply);
        public static d_Discord_Respond? Discord_Respond;

        public delegate void d_Discord_UpdateHandlers(ref DiscordEventHandlers handlers);
        public static d_Discord_UpdateHandlers? Discord_UpdateHandlers;

        public static long ToTimeStamp(DateTime date)
        {
            return Convert.ToInt64((date - DateTime.UnixEpoch).TotalMilliseconds);
        }
    }
}