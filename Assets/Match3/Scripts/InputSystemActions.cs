using UnityEngine;

namespace Match3.Scripts
{
    public class Input_Click
    {
        public Vector2 point;
        
        public static void Emmit(Vector2 point)
        {
            var action = new Input_Click();
            action.point = point;
            SmartPipe.EmmitAction(action);
        }
    }
}