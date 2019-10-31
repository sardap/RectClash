using System;
using System.Collections.Generic;
using System.Linq;
using RectClash.ECS.Input;
using RectClash.ECS.Sound;

namespace RectClash.ECS
{
    public class Engine
    {
        private const long MAX_UPDATE_TIME = 500 * 1000000;

        private static volatile Engine _instance;

        public static Engine Instance { get { return _instance; } }

        private Engine()
        {
        }

        public static void Initialise(IKeyboardInput keyboardInput, IWindow window, IMouseInput mouseInput, ISoundOutput soundOutput)
        {
            if(_instance != null)
            {
                throw new System.InvalidOperationException("Already Created");
            }

            Keyboard.Initialise(keyboardInput);
            _instance = new Engine();
            _instance._root = new Ent(null);
            _instance._time = new Time();
            _instance._window = window;
            _instance._toBeUpdated = new Stack<IEnt>();
            _instance._mouse = mouseInput;
			_instance._sound = soundOutput;

            _instance._window.OnStart();
            _instance._time.Start();
        }

        private IEnt _root;
        private Stack<IEnt> _toBeUpdated;
		private bool _entDirty;
        private Time _time;
        private long _max_loop_time;
        private IWindow _window;
		private ISoundOutput _sound;
        public IMouseInput _mouse;

        public IWindow Window { get { return _window; } }

        public IMouseInput Mouse { get { return _mouse; } }

		public ISoundOutput Sound { get { return _sound; } }

        public Time Time { get { return _time; } }

		public int MaxDrawables 
		{
			get; private set;
		}

		private void BroadcastMessage<S, T>(IEnt ent, S sender, T message)
		{
			foreach(var com in ent.Coms)
			{
				if(com is IObv<S, T>)
				{
					((IObv<S, T>)com).OnNotify(sender, message);
				}
			}

			foreach(var child in ent.Children)
			{
				BroadcastMessage(child, sender, message);
			}
		}

		public void BroadcastMessage<S, T>(S sender, T message)
		{
			BroadcastMessage(_root, sender, message);
		}

        public IEnt CreateEnt(IEnt parent, string name, List<string> tags)
        {
			_entDirty = true;
            var result = new Ent(parent, name, tags);
            parent.AddChild(result);
            return result;
        }

        public IEnt CreateEnt(IEnt parent, string name, params string[] tags)
		{
			return CreateEnt(parent, name, tags.ToList());
		}

        public IEnt CreateEnt(IEnt parent, string name = "")
        {
            return CreateEnt(parent, name, new List<string>());
        }

        public IEnt CreateEnt(string name = "")
        {
            return CreateEnt(_root, name);
        }
      
        public void  Step()
        {
            _max_loop_time = _time.ElapsedTime + (MAX_UPDATE_TIME);
			_time.StartOfLoop();

            if(_entDirty)
            {
				_entDirty = false;
				_toBeUpdated.Clear();
                _toBeUpdated = GetEntsToUpdate();
            }

            foreach(IEnt current in _toBeUpdated)
            {
                //var current = _toBeUpdated.Pop();

                current.Update();
                Window.Draw(current.DrawableComs);
            }
        }

        public void UpdateWindow()
        {
            _window.Update();
        }

        private Stack<IEnt> GetEntsToUpdate()
        {
            return GetEntsToUpdate(_root, _toBeUpdated);
        }

        private Stack<IEnt> GetEntsToUpdate(IEnt ent, Stack<IEnt> result)
        {
            result.Push(ent);

            foreach(var child in ent.Children)
            {
                GetEntsToUpdate(child, result);
            }

            return result;
        }
    }
}