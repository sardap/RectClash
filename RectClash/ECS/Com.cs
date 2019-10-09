namespace RectClash.ECS
{
    public abstract class Com : ICom
    {
        public IEnt Owner { get; set; }

        public virtual void Update()
        {

        } 

        public virtual void OnStart()
        {

        }

        
    }
}