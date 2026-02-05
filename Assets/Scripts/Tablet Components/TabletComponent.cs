using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class TabletComponent
{
    public int positionInDock;
    public string code;
    public event EventHandler<MessageFromTabletArs> CommandReceived;

    public abstract void updateStatus(JObject o);
    public abstract void setUp(JObject o);
    public abstract void setUp(JArray o);

    public abstract void FireEvent(string specificElementName, JObject message);

    public abstract JObject schematizeForTablet();

    protected virtual void OnMessageReceivedFromTablet(MessageFromTabletArs e)
    {
        // Safely raise the event for all subscribers
        CommandReceived?.Invoke(this, e);
    }
}

public class MessageFromTabletArs : EventArgs
{
    public MessageFromTabletArs(string componentname, JObject o)
    {
        Content = o;
        ComponentName = componentname;
    }

    public JObject Content { get; }
    public string ComponentName { get; }
}
