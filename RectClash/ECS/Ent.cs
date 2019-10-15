using System.Collections.Generic;
using RectClash.ECS.Exception;
using RectClash.ECS.Graphics;

namespace RectClash.ECS
{
    public class Ent : IEnt
    {
        private volatile IList<ICom> _coms = new List<ICom>();

        private volatile IList<IEnt> _children = new List<IEnt>();

        private volatile Stack<int> _justCreatedComs = new Stack<int>();

        private volatile IList<IDrawableCom> _drawables = new List<IDrawableCom>();

        private volatile PostionCom _postionCom;

        public IEnumerable<ICom> Coms { get { return _coms; } }

        public PostionCom PostionCom { get { return _postionCom; }}

        public Ent(IEnt parent) 
        {
            this.Parent = parent;
            _postionCom = AddCom(new PostionCom());
        }
        
        public IEnt Parent { get; set; }

        public IEnumerable<IEnt> Children { get { return _children; } }

        public IEnumerable<IDrawableCom> DrawableComs { get => _drawables; }

        private ICollection<IDrawableCom> GetDrawableComs(List<IDrawableCom> result)
        {
            foreach(var child in _children)
            {
                result.AddRange(((Ent)child).GetDrawableComs(result));
            }

            return _drawables;
        }

        public T AddCom<T>(T com) where T : ICom
        {
            if(com is PostionCom && _postionCom != null)
            {
                throw new PostionComAlreadyCreated();
            }

            _coms.Add(com);
            _justCreatedComs.Push(_coms.Count - 1);
            com.Owner = this;

            if(com is IDrawableCom)
            {
                _drawables.Add((IDrawableCom)com);
            }

            return com;
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
            while(_justCreatedComs.Count > 0)
            {
                _coms[_justCreatedComs.Pop()].OnStart();
            }

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