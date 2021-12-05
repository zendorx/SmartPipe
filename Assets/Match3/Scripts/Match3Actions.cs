namespace Match3.Scripts
{
    public class Match3_InitField
    {
        public int x;
        public int y;
        
        public static void Emmit(int x, int y)
        {
            var action = new Match3_InitField();
            action.x = x;
            action.y = y;
            SmartPipe.EmmitAction(action);
        }
    }

    public class Match3_CreateJewel
    {
        public Jewel jewel;
        
        public static void Emmit(Jewel jewel)
        {
            var action = new Match3_CreateJewel();
            action.jewel = jewel;
            SmartPipe.EmmitAction(action);
        }
    }

    public class Match3_SelectJewel
    {
        public Jewel jewel;
        
        public static void Emmit(Jewel jewel)
        {
            var action = new Match3_SelectJewel();
            action.jewel = jewel;
            SmartPipe.EmmitAction(action);
        }
        
    }

    public class Match3_UnSelectJewel
    {
        public Jewel jewel;
        
        public static void Emmit(Jewel jewel)
        {
            var action = new Match3_UnSelectJewel();
            action.jewel = jewel;
            SmartPipe.EmmitAction(action);
        }
    }
    
    public class Match3_DestroyJewel
    {
        public Jewel jewel;
        
        public static void Emmit(Jewel jewel)
        {
            var action = new Match3_DestroyJewel();
            action.jewel = jewel;
            SmartPipe.EmmitAction(action);
        }
    }
    
    public class Match3_FallStart
    {
        public int x;
        public int y;

        public int targetX;
        public int targetY;
        
        public static void Emmit(int x, int y, int targetX, int targetY)
        {
            var action = new Match3_FallStart();
            action.x = x;
            action.y = y;
            action.targetX = targetX;
            action.targetY = targetY;
            SmartPipe.EmmitAction(action);
        }
    }
    
    public class Match3_FallEnd
    {
        public Jewel jewel;
        
        public static void Emmit(Jewel jewel)
        {
            var action = new Match3_FallEnd();
            action.jewel = jewel;
            SmartPipe.EmmitAction(action);
        }
    }
}