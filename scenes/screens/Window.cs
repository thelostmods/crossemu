using System;
using System.IO;

using Godot;

public partial class Window : Control
{
    private MenuButton?     MenuFile;
    private MenuButton?     MenuConsole;

    private PanelContainer? Screen;
    private ItemList?       GameList;

    private FileDialog?     FileBox;

    private Console.ID      curConsole;

    public override void _Ready()
    {
        // load settings before anything else
        SettingsDef.LoadFile();

        // variable initializers
        MenuFile    = GetNode<MenuButton>("Align/Menu/Bar/Align/File");
        MenuConsole = GetNode<MenuButton>("Align/Menu/Bar/Align/Console");

        Screen      = GetNode<PanelContainer>("Align/Screen");
        GameList    = GetNode<ItemList>("Align/Screen/Margin/GameList");

        FileBox     = GetNode<FileDialog>("FileBox");

        // ui initializers
        _InitMenuFile();
        _InitMenuConsole();
            
        // load an instance of the selected console
        Console.ID id;
        Enum.TryParse<Console.ID>(Program.Settings.ActiveConsole, out id);
        _ConsoleChange(id);
    }

    public override void _Process(double delta)
    {
        if (Program.UI_Hidden != Program.ActiveConsole.Running)
        {
            if (Program.ActiveConsole.Running)
            {
                // disable console change options
                MenuConsole!.Disabled = true;

                // hide screen so the real console can be seen
                foreach (var child in Screen!.GetChildren())
                    ((Control) child).Hide();
                
                // change marked state
                Program.UI_Hidden = true;
            }
            else
            {
                // enable console change options
                MenuConsole!.Disabled = false;

                // show screen elements again
                foreach (var child in Screen!.GetChildren())
                    ((Control)child).Show();
                
                // change marked state
                Program.UI_Hidden = false;
            }
        }
    }

    private void TryStartGame(string gameFile)
    {
        // check if console is installed properly
        if (!Program.ActiveConsole.VerifyInstall(true)) return;
        
        // boot the console
        Program.ActiveConsole.Boot(gameFile);
    }

    //////////////////////
    // UI Initializers
    ///////

    private void _InitMenuFile()
    {
        var list = MenuFile!.GetPopup();
            
        // add dropdown list items
        list.AddItem("Open Settings");
        list.AddSeparator();
        list.AddItem("Open Game");
        list.AddItem("Refresh Games");
        list.AddSeparator("Directories");
        list.AddItem("Open App Folder");
        list.AddItem("Open Data Folder");
        list.AddItem("Open Games Folder");
        list.AddItem("Open Plugins Folder");
        list.AddSeparator();
        list.AddItem("Exit");
            
        // connect press event handler
        list.IndexPressed += (index) => {
            switch (index)
            {
                case 0: OS.ShellOpen(Paths.SettingsFile()); break;
                // Separator
                case 2: _FileLoadGame();     break;
                case 3: _FileRefreshGames(); break;
                // Separator
                case 5: OS.ShellOpen(Paths.GetDirectory(false)); break;
                case 6: OS.ShellOpen(Paths.GetDirectory(true)); break;
                case 7: OS.ShellOpen(Paths.Games(curConsole)); break;
                case 8: OS.ShellOpen(Paths.Mods(curConsole)); break;
                // Separator
                case 10: GetTree().Quit(); break;
            }
        };
    }

    private void _InitMenuConsole()
    {
        var list = MenuConsole!.GetPopup();
            
        // add console list to dropdown
        foreach (var id in Enum.GetNames<Console.ID>())
            list.AddRadioCheckItem(id);
        
        // add switch console logic
        list.IndexPressed += (index) => {
            if (curConsole == (Console.ID) index) return;
            _ConsoleChange((Console.ID) index);
        };
    }

    //////////////////////
    // Menu Signals
    ///////

    private void _FileLoadGame()
    {
        FileBox!.Filters = Program.ActiveConsole.AvailableExtensions;
        FileBox.Size = new Vector2I(800, 400);
        FileBox.Position = (new Vector2I(
            (int) Size.X - (int) FileBox.Size.X,
            (int) Size.Y - (int) FileBox.Size.Y
        ) / 2);
        FileBox.Show();
    }

    private void _FileRefreshGames()
    {
        // empty old list
        GameList!.Clear();
            
        // ensure path to games exists
        string path = Paths.Games(curConsole);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
        // list files in games directory
        foreach (var game in Directory.GetFiles(path))
            GameList.AddItem(Path.GetFileName(game));
    }

    private void _ConsoleChange(Console.ID id)
    {
        // store the string name of the type
        string idName = id.ToString();
        
        // update discord info
        Discord.RpcReset(idName.ToLower());
        
        // perform the console change
        switch (id)
        {
            case Console.ID.Nil: Program.ActiveConsole = new Console();    break;
            // case Console.ID.SNES:  break;
            // case Console.ID.GBC:  break;
            // case Console.ID.GBA:  break;
            case Console.ID.N64: Program.ActiveConsole = new ConsoleN64(); break;
            // case Console.ID.GC:  break;
            // case Console.ID.Wii: break;
            // case Console.ID.WiiU:  break;
            // case Console.ID.Switch:  break;
        }
        
        // flag our current selection
        curConsole = id;
        
        // update our settings with current selection
        if (Program.Settings.ActiveConsole != idName)
        {
            Program.Settings.ActiveConsole = idName;
            SettingsDef.SaveFile();
        }
        
        // make sure our menu reflects current selection
        var list = MenuConsole!.GetPopup();
        for (var i = 0; i < list.ItemCount; i++)
            list.SetItemChecked(i, false);
        list.SetItemChecked((int) id, true);
        
        // refresh the games list
        _FileRefreshGames();
    }

    //////////////////////
    // Other UI Signals
    ///////

    private void _OnFileDialogFileSelected(string path) => TryStartGame(path);

    private void _OnGameListItemActivated(int index)
    {
        string path = Paths.Games(curConsole);
        string game = path + GameList!.GetItemText(index);
        TryStartGame(game);
    }
}