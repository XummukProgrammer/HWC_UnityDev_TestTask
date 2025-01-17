public class Plugin
{
    public virtual string Name => "";
    public virtual string Version => "";
    public virtual string Author => "";

    public ClientController ClientController { get; set; }
    public ServerController ServerController { get; set; }

    public virtual void OnLoad() { }
    public virtual void OnUnload() { }

    public virtual void OnAllPluginsLoaded() { }

    public virtual void OnUpdate() { }
}
