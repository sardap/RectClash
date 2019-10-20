using System.Collections.Generic;
using RectClash.ECS.Exception;
using RectClash.ECS.Graphics;

namespace RectClash.ECS
{
    public class Ent : IEnt
    {
        private readonly IList<ICom> _coms = new List<ICom>();

        private readonly IList<IEnt> _children = new List<IEnt>();

        private readonly Stack<int> _justCreatedComs = new Stack<int>();

        private readonly IList<IDrawableCom> _drawables = new List<IDrawableCom>();

        private readonly PostionCom _postionCom;

        private readonly List<string> _tags = new List<string>();

        public IEnumerable<ICom> Coms { get { return _coms; } }

        public PostionCom PostionCom { get { return _postionCom; }}

        public readonly string Name;

        
        public IEnt Parent { get; set; }

        public IEnumerable<IEnt> Children { get { return _children; } }

        public IEnumerable<IDrawableCom> DrawableComs { get => _drawables; }

        public IList<string> Tags => _tags;
        
        public Ent(IEnt parent, string name = "") : this(parent, name, new List<string>())
        {
        }

        public Ent(IEnt parent, string name, IEnumerable<string> tags)
        {
            _tags.AddRange(tags);

            Name = name;

            if(Parent == null)
            {
                Parent = parent;
            }
            else
            {
               ChangeParent(parent);
            }
            _postionCom = AddCom(new PostionCom());
        }

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

        public void ChangeParent(IEnt ent)
        {
            if(Parent != null)
                ((Ent)Parent)._children.Remove(this);
            Parent = ent;
            ent.AddChild(this);
            PostionCom.ParentChanged();
        }
    }
}