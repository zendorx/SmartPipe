using System.Collections.Generic;
using MatchGamesExample.GamePlay.View;
using Unity.Mathematics;
using UnityEngine;

namespace MatchGamesExample.GamePlay
{
    public class Match3Controller: IPipeListener
    {
        private struct Cell
        {
            public int type;

            public void SetEmpty()
            {
                type = -1;
            }

            public bool IsEmpty()
            {
                return type == -1;
            }
        }
        
        private int width;
        private int height;

        private Cell[,] _cells;
        private int2 selected;
        
        public Match3Controller()
        {
            SmartPipe.RegisterListener_AsProccessor<JewelSelectAction>(this, OnJewelSelected);
            SmartPipe.RegisterListener<FieldCreateAction>(this, OnFieldCreated);
            SmartPipe.RegisterListener<JewelDestroyAction>(this, OnJewelDestroyed);
        }

        private void OnJewelDestroyed(JewelDestroyAction obj)
        {
            _cells[obj.position.x, obj.position.y].SetEmpty();
        }

        private void OnFieldCreated(FieldCreateAction obj)
        {
            width = obj.width;
            height = obj.height;
            
            _cells = new Cell[width, height];

            foreach (var jewel in obj.result)
            {
                var cell = new Cell();
                cell.type = jewel.type;
                _cells[jewel.position.x, jewel.position.y] = cell;
            }
        }

        private void OnJewelSelected(JewelSelectAction obj)
        {
            selected = obj.position;
            obj.SetCompleted();
            JewelDestroyAction.Process(selected);
        }
    }
}