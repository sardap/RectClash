using System.Collections.Generic;
using RectClash.ECS;

namespace RectClash.Game
{
    public class CellCom : Com
    {
        private IList<IEnt> _inside = new List<IEnt>();

        public ICollection<IEnt> Inside { get { return _inside; } }

                
    }
}