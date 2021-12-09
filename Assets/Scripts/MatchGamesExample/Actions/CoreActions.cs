public class GameLoadAsstsAction : IProcessAction
{
    public string url;

    public static GameLoadAsstsAction Process(string url)
    {
        var action = new GameLoadAsstsAction();
        action.url = url;
        SmartPipe2.Emmit(action);
        return action;
    }
}
