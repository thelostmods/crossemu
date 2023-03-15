using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text.Json.Nodes;

namespace DynaLib_DotNet
{
    public static class Conditionals
    {
        public static bool DL_ERROR = false;
        public static bool DL_WARNING = false;
        public static bool DL_NOTIFICATION = false;
    }

    internal class ManagedContext : AssemblyLoadContext
    {
        public Assembly? Handle = null;

        public bool TryLoad(string libName)
        {
            try
            {
                Handle = LoadFromAssemblyPath(libName);
                return true;
            }
            catch
            {
                Handle = null;
                return false;
            }
        }

        public new void Unload()
        {
            base.Unload();
            Handle = null;
        }
    }

    public struct BindDef
    {
        public string NameLinux;
        public string NameOSX;
        public string NameWindows;
        public string LoadPath;
        public string DepPath;

        public bool LoadBindConfig(string section, string altConfig = "")
        {
            var cd = AppContext.BaseDirectory;
            var file = cd + altConfig  + ".bindconf";

            if (!File.Exists(file))
            {
                file = cd + Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName) + ".bindconf";
                if (!File.Exists(file)) return false;
            }

            var json = JsonNode.Parse(File.ReadAllText(file));
            if (json?[section] == null) return false;

            From(json[section]);
            return true;
        }

        public void From(string input) { From(JsonNode.Parse(input)); }

        public void From(JsonNode? input)
        {
            if (input == null) return;

            if (input["nameLinux"] != null)
                NameLinux = (string) input["nameLinux"]!;
            else NameLinux = "";

            if (input["nameOSX"] != null)
                NameOSX = (string) input["nameOSX"]!;
            else NameOSX = "";

            if (input["nameWindows"] != null)
                NameWindows = (string) input["nameWindows"]!;
            else NameWindows = "";

            if (input["loadPath"] != null)
                LoadPath = (string) input["loadPath"]!;
            else LoadPath = "";

            if (input["depPath"] != null)
                DepPath = (string) input["depPath"]!;
            else DepPath = "";
        }

        public string NamePlatform()
        {
            if (OperatingSystem.IsLinux()) return NameLinux;
            if (OperatingSystem.IsMacOS()) return NameOSX;
            if (OperatingSystem.IsWindows()) return NameWindows;
            return "[Unknown Platform]";
        }

        public string FullName()
        {
            return LoadPath + "/" + NamePlatform();
        }
    }

    public struct DynaLib
    {
        private ManagedContext? Context;
        public IntPtr Handle;
        public string FileName;
        public string Directory;
        public string DependencyDir;

        public bool IsManaged => Context != null;

        private bool Load()
        {
            if (IsLoaded())
            {
                if (Conditionals.DL_ERROR)
                    Godot.GD.Print("DynaLib: Invalid request, handle is not yet free!");

                return false;
            }

            if (FileName == "" || OperatingSystem.IsWindows() && (
                                        Directory == "" ||
                                        DependencyDir == ""
                                    )
            ) {
                if (Conditionals.DL_ERROR)
                    Godot.GD.Print("DynaLib: Invalid input, not enough information to load library!");
                return false;
            }

            var success = false;

            Native.DependencyPath(DependencyDir);

            var context = new ManagedContext();
            context.TryLoad(Directory + FileName);
            if (context.Handle != null)
            {
                Context = context;
                success = true;
            }
            else
            {
                Handle = Native.LoadLib(Directory + FileName);
                success = Handle != IntPtr.Zero;
                context = null;
            }

            Native.DependencyPath("");

            return success;
        }

        public bool LoadFromConfig(string section, string altConfig = "")
        {
            var bind = new BindDef();
            if (!bind.LoadBindConfig(section, altConfig)) return false;
            return Load(bind);
        }

        public bool Load(BindDef def)
        {
            string? file;
            if (OperatingSystem.IsLinux())
                file = def.NameLinux;
            else if (OperatingSystem.IsMacOS())
                file = def.NameOSX;
            else if (OperatingSystem.IsWindows())
                file = def.NameWindows;
            else file = "Platform Unknown";

            return Load(file, def.LoadPath, def.DepPath);
        }

        public bool Load(string libName, string libDir = "", string depDir = "")
        {
            var cd = AppContext.BaseDirectory;
            if (libDir == "") libDir = "./";
            else if (libDir == "!") libDir = cd;
            if (depDir == "") depDir = "./";
            else if (depDir == "!") depDir = libDir;

            if (libDir[0] == '.') libDir = cd + libDir.Substring(1);
            if (depDir[0] == '.') depDir = cd + depDir.Substring(1);

            var e = libDir.Length - 1;
            if (libDir[e] != '/' && libDir[e] != '\\') libDir += '/';

            e = depDir.Length - 1;
            if (depDir[e] != '/' && depDir[e] != '\\') depDir += '/';

            string? ext;
            if (OperatingSystem.IsLinux())
                ext = ".so";
            else if (OperatingSystem.IsMacOS())
                ext = ".dylib";
            else if (OperatingSystem.IsWindows())
                ext = ".dll";
            else
            {
                if (Conditionals.DL_ERROR)
                    Godot.GD.Print("DynaLib: Platform not detected!");

                return false;
            }

            var file = libDir + libName;
            if (!File.Exists(file))
            {
                file += ext;
                if (!File.Exists(file))
                {
                    file = libName;
                    if (!File.Exists(file))
                    {
                        file += ext;
                        if (!File.Exists(file))
                        {
                            if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
                            {
                                // Pray its on the system paths
                                if (libName.EndsWith(ext))
                                    FileName = libName;
                                else FileName = libName + ext;

                                Directory = "";
                                DependencyDir = "";

                                return Load();
                            }
                            else
                            {
                                if (Conditionals.DL_ERROR)
                                    Godot.GD.Print("DynaLib: File not found [" + libName + "]");

                                return false;
                            }
                        } libName += ext;
                    }
                } libName += ext;
            }

            FileName = libName;
            Directory = libDir;
            DependencyDir = depDir;

            return Load();
        }

        public bool Bind<T>(ref T ptr, string symName)
        {
            if (IsManaged)
            {
                var type = Context!.Handle!.GetType(symName, false, true);
                if (type != null)
                    if (typeof(T).IsAssignableFrom(type))
                        ptr = (T)Context.Handle.CreateInstance(symName, true)!;
            }
            else Native.BindSymbol(Handle, ref ptr, symName);

            return ptr != null;
        }

        public bool Unload()
        {
            if (IsManaged)
            {
                Context!.Unload();
                Context = null;
                return true;
            }
            else if (Native.UnloadLib(Handle))
            {
                Handle = IntPtr.Zero;
                return true;
            } return false;
        }

        public bool Reload()
        {
            Unload();

            return Load();
        }

        public bool IsLoaded() => Handle != IntPtr.Zero || IsManaged;
    }

    public static class Native
    {
        public static IntPtr LoadLib(string libName)
        {
            NativeLibrary.TryLoad(libName, out IntPtr lib);
            return lib;
        }

        public static bool UnloadLib(IntPtr lib)
        {
            if (lib == IntPtr.Zero)
            {
                if (Conditionals.DL_ERROR)
                    Godot.GD.Print("UnloadLib: Library was already null!");

                return false;
            }

            NativeLibrary.Free(lib);

            return true;
        }

        public static bool BindSymbol<T>(IntPtr lib, ref T ptr, string symbol)
        {
            if (lib == IntPtr.Zero)
            {
                if (Conditionals.DL_ERROR)
                    Godot.GD.Print("BindSymbol: Library is null!");

                return false;
            }

            if (!NativeLibrary.TryGetExport(lib, symbol, out IntPtr addr))
            {
                if (Conditionals.DL_WARNING)
                    Godot.GD.Print($"BindSymbol: Symbol({symbol}) was not found in the library!");

                return false;
            }

            ptr = Marshal.GetDelegateForFunctionPointer<T>(addr);
            return true;
        }

        public static bool DependencyPath(string? path)
        {
            if (_environment == null) return true;

            if (path == null)
            {
                Environment.SetEnvironmentVariable("Path", _environment);
                return true;
            }
                
            if (_environment.Contains(path)) return false;

            var env = _environment = Environment.GetEnvironmentVariable("Path");
            Environment.SetEnvironmentVariable("Path", $"{env};{path};");
            return true;
        }

        private static string? _environment = Environment.GetEnvironmentVariable("Path");
    }
}