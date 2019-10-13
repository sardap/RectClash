namespace RectClash.ECS
{
    public abstract class Com : ICom
    {
        private bool _startRan = false;

        public IEnt Owner { get; set; }

        public bool StartRan { get { return _startRan; } }

        public virtual void Update()
        {
        } 

        protected virtual void InternalStart()
        {
        }

        public void OnStart()
        {
            if(!StartRan)
            {
                InternalStart();
            }

            _startRan = true;

        }
        
    }
}