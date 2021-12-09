using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace MatchGamesExample.GamePlay
{
    public class FieldController : IPipeListener
    {
        //private [,] _cells;
        public const float spacing = 1.1f;
        
        public FieldController()
        {
            SmartPipe2.RegisterListener_AsProccessor<FieldCreateAction>(this, CreateField);
            SmartPipe2.RegisterListener<JewelCreateAction>(this, OnJewelCreated);
        }

        private void OnJewelCreated(JewelCreateAction obj)
        {
        }

        private void CreateField(FieldCreateAction obj)
        {
            var rnd = new Random(obj.seed);        
    
            
            int width = obj.width;
            int height = obj.height;
            
            for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                int type = rnd.Next(obj.jewelTypeCount);

                JewelCreateAction.Process(type, new int2(i, j));
            }
        }
        
        
    }
}