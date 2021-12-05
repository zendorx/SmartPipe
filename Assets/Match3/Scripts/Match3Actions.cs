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
}