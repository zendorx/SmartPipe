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
    
    
    
    /*private void FallUpperJewel(int x, int y)
    {
        if (y > 0)
        {
            Match3_FallStart.Emmit(x, y-1, x, y);
        }
        else
        {
            GenerateNewJewelForColumn(x, y, true);
        }
    }*/

    private void GenerateNewJewelForColumn(int x, int y, bool withAnimation)
    {
        if (_cells[x, y]._Jewel != null)
        {
            Debug.LogError("Can't generate jewel for this cell");
            return;
        }
        
        var result = SmartPipe.EmmitActionWithResult<Match3_CreateJewel>(new Match3_CreateJewel());
        
        var j = result.jewel;
        j.Construct();
        
        j.SetPosition(x, y);
        j.SetType(Random.Range(0, 4));
        _cells[x, y]._Jewel = j;

        if (withAnimation)
        {
            j.MoveForFalling();
        }
    }


    /*private void OnDestroyJewel(Match3_DestroyJewel obj)
    {
        var j = obj.jewel;

        if (j == null)
        {
            return;
        }

        int x = j.x;
        int y = j.y;
        
        j.Kill();
        
        
        
        FallColumn(x);
    }*/

    /*private void OnStartFall(Match3_FallStart obj)
    {
        /*if (_cells[obj.targetX, obj.targetY]._Jewel != null && !(obj.x != obj.targetX && obj.y != obj.x))
        {
            Debug.LogError("Trying to fall on other jewel place");
            return;
        }* /

        var j = _cells[obj.x, obj.y]._Jewel;

        if (j == null)
        {
            //GenerateNewJewelForColumn(obj.x, obj.y, true);
            //FallUpperJewel(obj.x, obj.y);
            return;
        }
        
        _cells[obj.x, obj.y]._Jewel = null;
        int oldX = j.GetX();
        int oldY = j.GetY();
        
        if (j != null)
        {
            j.SetPosition(obj.targetX, obj.targetY, true);
            _cells[obj.targetX, obj.targetY]._Jewel = j;
        }
        
        FallColumn(oldY);
    }*/

    /*private void OnJewelDestroy(Match3_DestroyJewel obj)
    {
        if (obj.jewel == _selected)
        {
            _selected = null;
        }

        if (obj.jewel == null)
        {
            return;
        }

        int x = obj.jewel.GetX();
        int y = obj.jewel.GetY();

        _cells[x, y]._Jewel = null;
    }*/

    private void OnClick(Input_Click click)
    {
        var position = Jewel.PositionToIndex(click.point);

        if (position.x >= 0 && position.y >= 0 && position.x < width && position.y < height)
        {
            var j = _cells[position.x, position.y]._Jewel;

            if (j == null)
            {
                return;
            }

            if (j.IsFalling())
            {
                return;
            }
            
            Unselect();
            
            _selected = j;
            Match3_SelectJewel.Emmit(_selected);
            
            DestroyJewel(j);
        }
    }

    private void Unselect()
    {
        if (_selected)
        {
            _selected = null;
            Match3_UnSelectJewel.Emmit(_selected);                
        }
    }

    public void DestroyJewel(Jewel jewel)
    {
        if (jewel == _selected)
        {
            Unselect();
        }

        int x = jewel.x;
        int y = jewel.y;
        _cells[x, y]._Jewel = null;
        
        Match3_DestroyJewel.Emmit(jewel);

        FallColumn(x);

        GenerateJewelsForEmptyCells(x);
    }

    public void GenerateJewelsForEmptyCells(int x)
    {
        for (int y = height - 1; y >= 0; y--)
        {
            var j = _cells[x, y]._Jewel;

            if (j == null)
            {
                GenerateNewJewelForColumn(x, y, true);
            }

        }
    }

    private int FindLowestEmptyCell(int x)
    {
        int lowest = -1;
        
        for (int y = height - 1; y >= 0; y--)
        {
            if (_cells[x, y]._Jewel == null)
            {
                return y;
            }
        }

        return lowest;
    }

    private void ChangePosition(Jewel jewel, int x, int y)
    {
        if (_cells[x, y]._Jewel != null)
        {
            Debug.LogError("Can't change position on this cell");
            return;
        }

        _cells[jewel.x, jewel.y]._Jewel = null;
        _cells[x, y]._Jewel = jewel;
        jewel.SetPosition(x, y, true);
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
            var j = _cells[x, y]._Jewel;

            if (j != null)
            {
               ChangePosition(j, x, lowest);
               FallColumn(x);
               return;
            }
        }
    }

    private void FallAtCell(int i, int i1)
    {
        
    }

    private void DestroyField()
    {
        
    }
    
    private void OnInitField(Match3_InitField data)
    {
        DestroyField();
        
        _selected = null;
        
        width = data.x;
        height = data.y;
        
        _cells = new Cell[width, height];

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                var cell = new Cell();
                cell.x = i;
                cell.y = j;
                
                GenerateNewJewelForColumn(i, j, false);
            }

    }
    
}
