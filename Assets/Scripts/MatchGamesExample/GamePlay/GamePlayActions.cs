using System.Collections.Generic;
using MatchGamesExample.GamePlay.View;
using Unity.Mathematics;


public class FieldCreateAction : IFactoryAction
{
    public int width;
    public int height;
    public int seed;
    public int jewelTypeCount;
    public List<JewelView> result = new List<JewelView>();

    public static FieldCreateAction Process(int width, int height, int jewelTypeCount, int seed)
    {
        var action = new FieldCreateAction();
        action.width = width;
        action.height = height;
        action.seed = seed;
        action.jewelTypeCount = jewelTypeCount;
        SmartPipe.Emmit(action);
        return action;
    }
}

public class JewelCreateAction : IFactoryAction
{
    public int2 at;
    public int type;
    public JewelView result;
    public bool moveUpper;
    
    public static JewelCreateAction Process(int type, int2 at, bool moveUpper)
    {
        var action = new JewelCreateAction();
        action.type = type;
        action.at = at;
        action.moveUpper = moveUpper;
        SmartPipe.Emmit(action);
        return action;
    }
}

public class GameStartedAction : IInfoAction
{
    public static GameStartedAction Emmit()
    {
        var action = new GameStartedAction();
        SmartPipe.Emmit(action);
        return action;
    }
}

public class GameClickAction : IInfoAction
{
    public int2 position;
    
    public static GameClickAction Emmit(int2 position)
    {
        var action = new GameClickAction();
        action.position = position;
        SmartPipe.Emmit(action);
        return action;
    }
}

public class JewelSelectAction : IProcessAction
{
    public int2 position;
    
    public static JewelSelectAction Process(int2 position)
    {
        var action = new JewelSelectAction();
        action.position = position;
        SmartPipe.Emmit(action);
        return action;
    }
}

public class JewelDestroyAction : IProcessAction
{
    public int2 position;
    
    public static JewelDestroyAction Process(int2 position)
    {
        var action = new JewelDestroyAction();
        action.position = position;
        SmartPipe.Emmit(action);
        return action;
    }
}

public class JewelStartFallAction : IProcessAction
{
    public int2 from;
    public int2 to;
    
    public static JewelStartFallAction Process(int2 from, int2 to)
    {
        var action = new JewelStartFallAction();
        action.from = from;
        action.to = to;
        SmartPipe.Emmit(action);
        return action;
    }
}

public class JewelStartFallFromTopAction : IProcessAction
{
    public int2 pos;
    
    public static JewelStartFallFromTopAction Process(int2 pos)
    {
        var action = new JewelStartFallFromTopAction();
        action.pos = pos;
        SmartPipe.Emmit(action);
        return action;
    }
}