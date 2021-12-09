using MatchGamesExample.GamePlay.View;
using Unity.Mathematics;

public class GameplayActions
{
    
}



public class FieldCreateAction : IProcessAction
{
    public int width;
    public int height;
    public int seed;
    public int jewelTypeCount;
    
    public static FieldCreateAction Process(int width, int height, int jewelTypeCount, int seed)
    {
        var action = new FieldCreateAction();
        action.width = width;
        action.height = height;
        action.seed = seed;
        action.jewelTypeCount = jewelTypeCount;
        SmartPipe2.Emmit(action);
        return action;
    }
}

public class JewelCreateAction : IFactoryAction
{
    public int2 at;
    public int type;
    public JewelView result;
    
    public static JewelCreateAction Process(int type, int2 at)
    {
        var action = new JewelCreateAction();
        action.type = type;
        action.at = at;
        SmartPipe2.Emmit(action);
        return action;
    }
}