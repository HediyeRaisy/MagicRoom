using Newtonsoft.Json.Linq;

public class ConnectionManager : TabletHandlerManager
{
    protected override void HandlerButton(CommandMessages command)
    {
        print(command);
    }

    protected override void HandlerConfiguration(JObject configuration)
    {
        print(configuration);
    }
}