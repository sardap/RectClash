
using RectClash.ECS;
using RectClash.ECS.Graphics;

namespace RectClash.Game
{
    public class GridCom : Com
    {
        private CellCom[,] _cells;

        public GridCom()
        {
        }

        public void GenrateGrid(int gridWidth, int gridHeight, double cellWidth, double cellHeight)
        {
            _cells = new CellCom[gridWidth, gridHeight];

            for(int i = 0; i < _cells.GetLength(0); i++)
            {
                for(int j = 0; j < _cells.GetLength(1); j++)
                {
                    var newCell = Engine.Instance.CreateEnt(Owner);
                    _cells[i, j] = newCell.AddCom(new CellCom());
                    newCell.PostionCom.X = i * cellWidth;
                    newCell.PostionCom.Y = j * cellHeight;

                    newCell.AddCom
                    (
                        new DrawRectCom()
                        {
                            Width = cellWidth,
                            Height = cellHeight,
                            OutlineColour = new Colour(125, byte.MaxValue, byte.MaxValue, byte.MaxValue),
                            LineThickness = 1
                        }
                    );
                }
            }
        }
    }
}