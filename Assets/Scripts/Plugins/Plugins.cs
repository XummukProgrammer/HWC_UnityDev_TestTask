using System.Collections.Generic;

public static class Plugins
{
    private static List<Plugin> _plugins = new();

    public static ClientController ClientController { get; set; }
    public static ServerController ServerController { get; set; }

    public static void Add(Plugin plugin)
    {
        _plugins.Add(plugin);

        plugin.ClientController = ClientController;
        plugin.ServerController = ServerController;

        plugin.OnLoad();
    }

    public static void Remove(Plugin plugin)
    {
        plugin.OnUnload();
        _plugins.Remove(plugin);
    }

    public static void OnAllPluginsLoaded()
    {
        foreach (var plugin in _plugins)
        {
            plugin.OnAllPluginsLoaded();
        }
    }

    public static void OnUpdate()
    {
        foreach (var plugin in _plugins)
        {
            plugin.OnUpdate();
        }
    }
}
