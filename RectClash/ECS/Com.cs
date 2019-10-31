namespace RectClash.ECS
{
    public abstract class Com : ICom
    {
        private bool _startRan = false;

        public IEnt Owner { get; set; }

        public bool StartRan { get { return _startRan; } }

		public bool Enabled { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

		public virtual void Update()
        {
        } 

        protected virtual void InternalStart()
        {
        }

		protected virtual void InternalDestroy()
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

        public void Destory()
        {
			InternalDestroy();
			Owner = null;
        }
    }
}