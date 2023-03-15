using System;

using Discord_RPC_DotNet;

static class Discord
{
	private static DiscordRPC.DiscordRichPresence Rpc;
	private static bool Enabled = TryInitialize();

	public static void HandleReady(ref DiscordRPC.DiscordUser connectedUser)
	{
		// Console.WriteLine(string.Format(
		//     "Discord: connected to user %s#%s - %s",
		//     connectedUser.username,
		//     connectedUser.discriminator,
		//     connectedUser.userId
		// ));
	}

	public static void HandleDisconnected(int errcode, string message)
	{
		// Console.WriteLine($"Discord: disconnected ({errcode}: {message})");
	}

	public static void HandleError(int errcode, string message)
	{
		// Console.WriteLine($"Discord: error ({errcode}: {message})");
	}

	public static void HandleJoin(string secret)
	{
		// Console.WriteLine($"Discord: join ({secret})");
	}

	public static void HandleSpectate(string secret)
	{
		// Console.WriteLine($"Discord: spectate ({secret})");
	}

	public static void HandleJoinRequest(ref DiscordRPC.DiscordUser request)
	{
		// int response = -1;
		// Console.WriteLine(string.Format(
		//     "Discord: join request from %s#%s - %s",
		//     request.username,
		//     request.discriminator,
		//     request.userId
		// ));

		// response = DiscordReply.YES;
		// response = DiscordReply.NO;
		// response = DiscordReply.IGNORE;
		// response = DiscordReply.PRIVATE;
		// response = DiscordReply.PUBLIC;

		//if (response != -1)
		//    DiscordRPC.Discord_Respond(request.userId, response);
	}

	private static bool TryInitialize()
	{	
		if (!DiscordRPC.Initialized)
		{
			// find discord binary inside the godot-editor
			if (!Godot.OS.HasFeature("standalone"))
			{
				var binPath = AppContext.BaseDirectory +
					"../../../../../extensions/binaries/";
				switch(Godot.OS.GetName())
				{
					case "Linux": DiscordRPC.TryInitialize(binPath + "linux"); break;
					case "macOS": DiscordRPC.TryInitialize(binPath + "macos"); break;
					case "Windows":
						if (System.Environment.Is64BitProcess)
							DiscordRPC.TryInitialize(binPath + "win64");
						else DiscordRPC.TryInitialize(binPath + "win32");
						break;
				}
			}

			// still failed to load
			if (!DiscordRPC.Initialized) return false;
		}
		
		// callbacks to pass to discord
		var handlers = new DiscordRPC.DiscordEventHandlers()
		{
			ready = HandleReady,
			disconnected = HandleDisconnected,
			errored = HandleError,
			joinGame = HandleJoin,
			spectateGame = HandleSpectate,
			joinRequest = HandleJoinRequest
		};

		// initialize discord api with the Cross-Emu app id
		DiscordRPC.Discord_Initialize_Managed!("782914399526584330", ref handlers, true, "");

		// set some presence details and pass to discord
		// Rpc.state = "In a Group";
		// Rpc.partyId = "Some Party Name?";
		// Rpc.partySize = 1;
		// Rpc.partyMax = 6;
		// Rpc.matchSecret = "4b2fdce12f639de8bfa7e3591b71a0d679d7c93f";
		// Rpc.spectateSecret = "e7eb30d2ee025ed05c71ea495f770b76454ee4e0";
		// Rpc.instance = 1;
		DiscordRPC.Discord_UpdatePresence!(ref Rpc);

		// set default status
		RpcReset();

		return true;
	}

	private static void ShutDown()
	{
		if (!Enabled) return;

		DiscordRPC.Discord_Shutdown!();
	}

	public static void RpcReset(string console = "nil")
	{
		if (!Enabled) return;

		// reset timers
		Rpc.startTimestamp = DiscordRPC.ToTimeStamp(DateTime.UtcNow);
		Rpc.endTimestamp   = 0;

		// set the default status
		RpcSetLargeImage("large", "Cross-Emulator");
		RpcSetSmallImage(console, "No Game Selected");
		RpcSetDescription("Selecting a game...", true);
	}

	public static void RpcSetDescription(string value, bool updateNow = false)
	{
		if (!Enabled) return;

		Rpc.details = value;

		if (updateNow) DiscordRPC.Discord_UpdatePresence!(ref Rpc);
	}

	public static void RpcSetLargeImage(string img, string desc, bool updateNow = false)
	{
		if (!Enabled) return;

		Rpc.largeImageKey = img;
		Rpc.largeImageText = desc;

		if (updateNow) DiscordRPC.Discord_UpdatePresence!(ref Rpc);
	}

	public static void RpcSetSmallImage(string img, string desc, bool updateNow = false)
	{
		if (!Enabled) return;

		Rpc.smallImageKey = img;
		Rpc.smallImageText = desc;

		if (updateNow) DiscordRPC.Discord_UpdatePresence!(ref Rpc);
	}

	public static void RpcSetGame(string console, string icon, string game)
	{
		if (!Enabled) return;

		game = ToTitle(game.Trim().ToLower());

		RpcSetLargeImage((console + "_" + icon).ToLower(), game);
		RpcSetSmallImage(
			console.ToLower(),
			char.ToUpper(console[0]) + console[1..].ToLower()
		);

		// debug or romhacking mode
		if (!Godot.OS.HasFeature("standalone"))
			RpcSetDescription("Modding: " + game);
		else
			RpcSetDescription("Playing: " + game);
		
		// reset timers
		Rpc.startTimestamp = DiscordRPC.ToTimeStamp(DateTime.UtcNow);
		Rpc.endTimestamp   = 0;

		// reset and update
		DiscordRPC.Discord_ClearPresence!();
		DiscordRPC.Discord_UpdatePresence!(ref Rpc);
	}

	static string ToTitle(string name)
	{
		// professionalize the name
		var words = name.Split(' ');
		var title = "";

		// scan words for hyphens
		for (var i = 0; i < words.Length; i++)
		{
			// fix words within hyphenated titles
			if (words[i].Contains('-'))
			{
				var parts = words[i].Split('-');
				var hyphened = TitleCase(parts[0]);

				// hyphen and capitalize each word after
				for (var n = 1; n < parts.Length; n++)
					hyphened += "-" + TitleCase(parts[n]);

				title += " " + hyphened;
			}
			else // post the normal single-word
				title += " " + TitleCase(words[i]);
		}
		
		// remove the leading space and capitalize first 
		// letter in case the title started with a non-capitalized word
		title = char.ToUpper(title[1]) + title.Substring(2);

		// return the fixed title
		return title;
	}

	static string TitleCase(string word)
	{
		word = word.ToLower();

		switch (word)
		{
			// articles
			case "a":
			case "an":
			case "the":

			// coordinate conjunctions
			case "for":
			case "and":
			case "nor":
			case "but":
			case "or":
			case "yet":
			case "so":

			// prepositions
			case "at":
			case "around":
			case "by":
			case "after":
			case "along":
			case "from":
			case "of":
			case "on":
			case "to":
			case "with":
			case "without":

				// return as lower cased
				return word;

			// capitalize this word
			default:
				return char.ToUpper(word[0]) + word[1..];
		}
	}
}
