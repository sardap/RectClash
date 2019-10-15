using System;
using System.Collections.Generic;
using RectClash.ECS.Input;
using RectClash.ECS.Performance;

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

        public static void Initialise(IKeyboardInput keyboardInput, IWindow window, IMouseInput mouseInput)
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
            _instance._perfMessure = new PerfMessure();
            _instance._toDraw = new HashSet<IEnt>();
            _instance._mouse = mouseInput;

            _instance._window.OnStart();
            _instance._time.Start();
        }

        private volatile IEnt _root;
        private volatile Stack<IEnt> _toBeUpdated;
        public volatile HashSet<IEnt> _toDraw;
        private Time _time;
        private long _max_loop_time;
        private long _timeDrawLoopTook;
        private IWindow _window;
        public IMouseInput _mouse;
        private PerfMessure _perfMessure;

        public IWindow Window { get { return _window; } }

        public IMouseInput Mouse { get { return _mouse; } }

        public Time Time { get { return _time; } }

        public PerfMessure PerfMessure { get { return _perfMessure; } }

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
      
        public void  Step()
        {
            _max_loop_time = _time.ElapsedTime + (MAX_UPDATE_TIME - _timeDrawLoopTook);

            if(_toBeUpdated.Count == 0)
            {
                _time.StartOfLoop();
                _toBeUpdated = GetEntsToUpdate();
            }

            while(_toBeUpdated.Count > 0)
            {
                var current = _toBeUpdated.Pop();

                current.Update();
                Window.Draw(current.DrawableComs);

                /*
                if(_time.ElapsedTime > _max_loop_time)
                {
                    _timeDrawLoopTook = _time.ElapsedTime;
                    foreach(var i in _toBeUpdated)
                    {
                        Window.Draw(i.DrawableComs);
                    }

                    _timeDrawLoopTook = _time.ElapsedTime - _timeDrawLoopTook;
                    Console.WriteLine("RAN OUT OF TIME LEFT : {0}", _toBeUpdated.Count);
                    return;
                }
                */
            }
        }

        public void UpdateWindow()
        {
            _window.Update();
            PerfMessure.Step();
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