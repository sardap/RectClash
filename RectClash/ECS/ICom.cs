namespace RectClash.ECS
{
    public interface ICom
    {
        IEnt Owner { get; set; }

        void Update();

        void OnStart();
    }
}