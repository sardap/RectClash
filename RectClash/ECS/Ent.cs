using System.Collections.Generic;
using RectClash.ECS.Exception;

namespace RectClash.ECS
{
    public class Ent : IEnt
    {
        private volatile IList<ICom> _coms = new List<ICom>();

        private volatile IList<IEnt> _children = new List<IEnt>();

        public IEnumerable<ICom> Coms { get { return _coms; } }

        public Ent(IEnt parent) 
        {
            this.Parent = parent;
               
        }
        public IEnt Parent { get; set; }

        public IEnumerable<IEnt> Children { get { return _children; } }

        public void AddCom(ICom com)
        {
            _coms.Add(com);
            com.Owner = this;
        }

        public void AddComs(IEnumerable<ICom> coms)
        {
            foreach(var i in coms)
            {
                AddCom(i);
            }
        }

        public T GetCom<T>() where T : ICom
        {
            foreach(var i in _coms)
            {
                if(i is T)
                {
                    return (T)i;
                }
            }

            throw new ComNotFoundException();
        }

        public void Update()
        {
            foreach(var com in Coms)
            {
                com.Update();
            }
        }

        public void AddChild(IEnt ent)
        {
            _children.Add(ent);
        }
    }
}