(Note: Slightly out-dated. Will update soon!)

# Cross-Emulator

This is the official repository for the crossemu app development.

# What is Cross-Emulator (CrossEmu)?

CrossEmu is a multi-emulator (Currently only supports N64) meant to expand modding possibilities beyond romhacking. It aims to support the ease-of-use ability to manage mods (both romhack and ramhack variety) without hard-patching your legally dumped roms, as well as expand the limits of what modding is possible by ignoring some of the console limitations.

Examples of 'expanding the limits' include network support where the console doesnt normally support it (consider online multiplayer mods for single player games) or expanded memory, or doing audio/gui tasks which otherwise wouldn't be supported either well or at all.

Something to compare the project to is similar to Vortex/NexusMods with games like Skyrim, but for emulated console games.

# How does it all work?

All of this operates on a child-emulator injection process, where the games memory is exposed to the application, which is then passed on to the mod/plugins giving them control over how the game operates.

# How do I make a mod?

CrossEmu supports the creation of 'plugin' mods where a developer can simply target the sdk headers provided with the application (using the language of your choice) as well as any gdk if available and begin writing human readable code to interact with the game as it is running.

The functionality provided would allow a developer to import a small bit of boiler plate code and be ready to start doing things!

Generally the outlined process is:
  - Init any private/local data the mod will need to manage external of the game
  - Patch the rom or supply data to the gdk
  - Read/Write data to the game in the update loop

# What do I need to know to begin modding?

Soon I will be providing tutorials on developing mods or gdks (TBA), though some existing basic knowlege in a supported language is preferred.

For the Source Dev Kit (SDK), everything is exposed via a C library, granting access to development in any language with a simple binding. Some will already be provided out of the box, such as Rust, DLang, and Nim (Although the rust is untested and Nim is incomplete at this time).

For Game Dev Kits (GDK), they will only have proper support in DLang, though will be exposed through json and binding-generated. If a lang is a popular enough request, or help is received, binding generators may be created for other languages to have proper support as well.

Later in the life cycle of this app, support for scripting languages will be added as requested. The first of script lang support will be gdscript(a python esque language built into godot), next will be java/type-script, anything further will require a high enough demand.

# History

CrossEmu started off as a modding emulator to rival ModLoader64 which I helped to create, after working with NodeJS made some of my modding goals too painful and a brief instability of the update cycles for the app I decided I should make my own where I would have a little more control over what is happening in the background that may or may not break my mods.

After reaching the point of being functional, the goal of being a rival to ModLoader64 basically dropped as I grew to have the motivation of supporting different design patterns and goals. Today the purpose of Cross-Emulator is to have a more direct interface with its emulators than NodeJS offers, and to support a wide variety of languages to open up the ability for people to mod coming from different code backgrounds and experience levels.

It started off as a C# project which intended to only be a hybrid emulator for N64 and PS1. Upon realizing the PS1 part wasn't realistic due to poor open source resources and not having enough knowlege about the console to do it myself, the project shrunk down to only the intent of a N64 focused modding emulator known as Cross64.

Later, when limitations of C# (through the current .net core version and libraries available to it) not being very mature and bloated made some design goals impossible, the move to the application being created in DLang happened which opened the plans back up to supporting multiple emulators.

After the release of .Net 5, it seemed that language offered more opportunities and features that would reduce the workload than DLang, and seemingly would make Networking and GUI more accessible, so it was once again brought back to C#.

Finally, with Godot 4 reaching beta, it seemed actually the most possibilities were open by embedding with it, which provides a full game engine worth of opportunities without any risk of stability and wide-feature support to be maintained by possibly one guy, and better platform distribution. Thus the transition to Godot began.

# Feature Roadmap

The following is a list of features which may be at different stages [Partial, Complete, Planned]:
  - Emulator Loading
	- NES    [Planned]
	- SNES   [Planned]
	- GBA    [Planned]
	- N64    [Complete]
	- GC/Wii [Planned]
	- PS1    [Planned]
	- PS2    [Planned]
  
  - Modding capability
	- Ram/Rom Manipulation        [Complete]
	- ASM Injecting               [Planned]
	- Mid-Frame Callbacks         [Planned]
	- Hybrid/Fake ASM Interaction [Planned]
	- Online/Lobby Support        [Planned]
	- Gui Creation                [Planned]
	- Audio Player (SFX/Music)    [Planned]

  - Platform Support
	- Windows x86 [Partial]
	- Windows x64 [Complete]
	- Linux   x64 [Complete]
	- MacOSX  x64 [Partial]
	- Android     [Planned/Maybe]
	- IOS         [Planned/Maybe]

  - Alternate Emulator Support  [Planned]
  - Plugin Loading              [Complete]
  - Gui Support                 [Implicit]
  - Mod Manager                 [Planned]
  - Mod Catelog/Browser         [Planned]
  - Integrated Networking       [Implicit]

# Join the Discord

For development news, support, or to participate in the community; Join with the link below.
https://discord.com/invite/EdH7jkh

Remember to read the rules and become a [member-role] for full access!