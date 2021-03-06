using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RectClash.ECS.Exception;
using RectClash.ECS.Graphics;

namespace RectClash.ECS
{
    public class Ent : IEnt
    {
		private static IList<IDrawableCom> _emptyDrawables = new List<IDrawableCom>();

		private static IList<IEnt> _emptyChildren = new List<IEnt>();

        private readonly IList<ICom> _coms = new List<ICom>();

        private readonly IList<IEnt> _children = new List<IEnt>();

        private readonly Stack<int> _justCreatedComs = new Stack<int>();

        private readonly IList<IDrawableCom> _drawables = new List<IDrawableCom>();

        private readonly PostionCom _postionCom;

        private readonly List<string> _tags = new List<string>();

		private readonly List<IEnt> _activeChildren = new List<IEnt>();

		private bool _enabled;

        public IEnumerable<ICom> Coms { get { return _coms; } }

        public PostionCom PostionCom { get { return _postionCom; }}

        public readonly string Name;

        public IEnt Parent { get; set; }

        public IEnumerable<IEnt> Children 
		{ 
			get 
			{
				_activeChildren.Clear();

				foreach(var child in _children)
				{
					if(child.Enabled)
						_activeChildren.Add(child);
				}

				return Enabled ? _activeChildren : _emptyChildren; 
			} 
		}

        public IEnumerable<IDrawableCom> DrawableComs 
		{ 
			get
			{
				return Enabled ? _drawables : _emptyDrawables;
			} 
		}

		public bool Enabled
		{
			get => _enabled;
			set
			{
				_enabled = value;
			}
		}

		public bool Destroyed
		{
			get;
			private set;
		}

		public ISubject Subject
		{
			get;
			set;
		}

        public IList<string> Tags => _tags;
        
        public Ent(IEnt parent, string name = "") : this(parent, name, new List<string>())
        {
			Destroyed = false;
        }

        public Ent(IEnt parent, string name, IEnumerable<string> tags)
        {
            _tags.AddRange(tags);

			Enabled = true;
			
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

		public void RemoveCom(ICom com)
		{
			Debug.Assert(!Destroyed);

			if(!_coms.Contains(com))
				throw new CannotRemoveNoneOwnedCom();

			_coms.Remove(com);
			if(com is IDrawableCom)
			{
				_drawables.Remove((IDrawableCom)com);
			}
			com.Destory();
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
			Debug.Assert(!Destroyed);

            foreach(var i in _coms)
            {
                if(i is T)
                {
                    return (T)i;
                }
            }

			return default(T);
        }

        public void Update()
        {
			Debug.Assert(!Destroyed);
			
			if(!Enabled)
			{
				return;
			}

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

        public void Destory()
        {
			((Ent)Parent)._children.Remove(this);

			foreach(var ent in _children.ToList())
			{
				ent.Destory();
			}

			foreach(var com in Coms.ToList())
			{
				RemoveCom(com);
			}
			_coms.Clear();

			// Parent = null;
			Destroyed = true;
        }

		private ICollection<IDrawableCom> GetDrawableComs(List<IDrawableCom> result)
        {
			Debug.Assert(!Destroyed);

            foreach(var child in _children)
            {
                result.AddRange(((Ent)child).GetDrawableComs(result));
            }

            return _drawables;
        }

		public override string ToString()
		{
			return Name;
		}

		public void Notify<S, T>(S sender, T message)
		{
			if(Subject is Subject<S, T>)
			{
				((Subject<S, T>)Subject).Notify(sender, message);
			}
		}

		public void NotifyChildren<S, T>(S sender, T message)
		{
			Notify(sender, message);

			foreach(IEnt i in Children.ToList())
			{
				i.NotifyChildren(sender, message);
			}
		}
	}
}