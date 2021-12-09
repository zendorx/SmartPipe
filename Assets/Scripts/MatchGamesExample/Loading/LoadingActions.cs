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



public class GameSceneLoadAction : IProcessAction
{
    public string sceneName;

    public static GameSceneLoadAction Process(string sceneName)
    {
        var action = new GameSceneLoadAction();
        action.sceneName = sceneName;
        SmartPipe2.Emmit(action);
        return action;
    }
}