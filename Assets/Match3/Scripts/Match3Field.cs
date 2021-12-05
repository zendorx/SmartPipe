using System.Collections;
using System.Collections.Generic;
using Match3.Scripts;
using UnityEngine;

public class Match3Field : IPipeListener
{
    public struct Cell
    {
        public Jewel _Jewel;
        public int x;
        public int y;
    }

    private Cell[,] _cells;
    private int width;
    private int height;

    private Jewel _selected;
    
    public Match3Field()
    {
        SmartPipe.RegisterListener<Match3_InitField>(this, OnInitField);
        SmartPipe.RegisterListener<Input_Click>(this, OnClick);
    }

    private void OnClick(Input_Click click)
    {
        var position = Jewel.PositionToIndex(click.point);

        if (position.x >= 0 && position.y >= 0 && position.x < width && position.y < height)
        {
            if (_selected != null)
            {
                Match3_UnSelectJewel.Emmit(_selected);                
                _selected = null;
            }
            
            //_cells[position.x, position.y]._Jewel.Select();
            _selected = _cells[position.x, position.y]._Jewel;
            
            Match3_SelectJewel.Emmit(_selected);
        }
    }

    private void DestoyFieled()
    {
        
    }
    
    private void OnInitField(Match3_InitField data)
    {
        DestoyFieled();
        
        _selected = null;
        
        width = data.x;
        height = data.y;
        
        _cells = new Cell[width, height];

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                var result = SmartPipe.EmmitActionWithResult<Match3_CreateJewel>(new Match3_CreateJewel());
                
                var cell = new Cell();
                cell.x = i;
                cell.y = j;
                cell._Jewel = result.jewel;
                    
                cell._Jewel.SetPosition(i, j);
                cell._Jewel.SetType(Random.Range(0, 4));
                _cells[i, j] = cell;
            }

    }
    
}
