using System.Collections.Generic;
using System.IO.Pipes;
using MatchGamesExample.GamePlay.View;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace MatchGamesExample.GamePlay
{
    public class FieldController : IPipeListener, IWaiter
    {
        private JewelView[,] _cells;
        private FieldCreateAction fieldCreateAction;
        private int width;
        private int height;
        private Random random;
        private int jewelTypeCount;
        
        public FieldController()
        {
            SmartPipe.RegisterListener_AsFactory<FieldCreateAction>(this, CreateField);
            SmartPipe.RegisterListener<JewelCreateAction>(this, OnJewelCreated);
            SmartPipe.RegisterListener<JewelDestroyAction>(this, OnJewelDestroyed);
            //SmartPipe2.RegisterListener<JewelStartFallAction>(this, OnJewelStartFall);
        }

        private void OnJewelStartFall(JewelStartFallAction obj)
        {
            if (obj.from.y == 0 && _cells[obj.from.x, obj.from.y] == null)
            {
                int2 newPos = new int2(obj.from.x, 0);
                CreateRandomJewel(newPos, true);
            }
        }

        private void OnJewelDestroyed(JewelDestroyAction obj)
        {
            _cells[obj.position.x, obj.position.y] = null;

            FallColumn(obj.position.x);
        }
        
        private void FallColumn(int x)
        {
            int lowest = FindLowestEmptyCell(x);

            if (lowest == -1)
            {
                return;
            }

            for (int y = lowest - 1; y >= 0; y--)
            {
                var j = _cells[x, y];

                if (j != null)
                {
                    int2 to = new int2(x, lowest);
                    _cells[j.position.x,j.position.y] = null;
                    _cells[to.x, to.y] = j;
                    
                    JewelStartFallAction.Process(j.position, to);
                    FallColumn(x);

                    if (j.position.y == 0)
                    {
                        CreateRandomJewel(j.position, true);
                    }
                    
                    return;
                }
            }
            
            
        }
        
        private int FindLowestEmptyCell(int x)
        {
            int lowest = -1;
        
            for (int y = height - 1; y >= 0; y--)
            {
                if (_cells[x, y] == null)
                {
                    return y;
                }
            }

            return lowest;
        }
        
        private void OnJewelCreated(JewelCreateAction obj)
        {
            _cells[obj.at.x, obj.at.y] = obj.result;
            
            if (fieldCreateAction != null)
            {
                fieldCreateAction.result.Add(obj.result);
            }

            if (obj.moveUpper)
            {
                JewelStartFallAction.Process(obj.at, obj.at);
            }
        }
        
        private void CreateField(FieldCreateAction obj)
        {
            fieldCreateAction = obj;
            random = new Random(obj.seed);        
            fieldCreateAction.result = new List<JewelView>();
            jewelTypeCount = obj.jewelTypeCount;
            width = obj.width;
            height = obj.height;
            
            _cells = new JewelView[width, height];
            
            for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
              CreateRandomJewel(new int2(i, j), false).Wait(this);
            }
        }

        public JewelCreateAction CreateRandomJewel(int2 position, bool moveUpper)
        {
            int type = random.Next(jewelTypeCount);
            return JewelCreateAction.Process(type, position, moveUpper);
        }

        public void OnAllActionsCompleted()
        {
            fieldCreateAction.Resolve();
            fieldCreateAction = null;
        }
    }
}