using System;
using System.Collections.Generic;
using RectClash.ECS.Input;

namespace RectClash.ECS
{
    public class Engine
    {
        private const long MAX_UPDATE_TIME = 500;

        private static volatile Engine _instance;

        public static Engine Instance { get { return _instance; } }

        private Engine()
        {
        }

        public static void Initialise(IKeyboardInput input, IWindow window)
        {
            if(_instance != null)
            {
                throw new System.InvalidOperationException("Already Created");
            }

            Keyboard.Initialise(input);
            _instance = new Engine();
            _instance._root = new Ent(null);
            _instance._time = new Time();
            _instance._window = window;
        }

        private volatile IEnt _root;
        private Stack<IEnt> _toBeUpdated;
        private Time _time;
        private long _max_loop_time;
        private IWindow _window;

        public IWindow Window { get { return _window; } }

        public IEnt CreateEnt(IEnt parent)
        {
            var result = new Ent(parent);
            parent.AddChild(result);
            return result;
        }

        public IEnt CreateEnt()
        {
            return CreateEnt(_root);
        }
       
        public void Step()
        {
            _max_loop_time = MAX_UPDATE_TIME;
            
            if(_toBeUpdated == null)
            {
                _toBeUpdated = GetEntsToUpdate();
                _time.StartOfLoop();
            }
            
            while(_toBeUpdated.Count > 0)
            {
                var current = _toBeUpdated.Pop();

                current.Update();

                _max_loop_time -= _time.DeltaTime;

                if(_max_loop_time < 0 && _toBeUpdated.Count > 0)
                {
                    Console.WriteLine("ECS: Ran out of time in Step LEFT: " + _toBeUpdated.Count);
                    return;
                }
            }

            _toBeUpdated = null;
        }

        public void UpdateWindow()
        {
            _window.Update();
        }   

        private Stack<IEnt> GetEntsToUpdate()
        {
            return GetEntsToUpdate(_root, new Stack<IEnt>());
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