namespace RectClash.ECS
{
    public interface ICom
    {

        IEnt Owner { get; set; }

		bool Enabled { get; set; }
		
        bool StartRan { get; }

        void Update();

        void OnStart();

		void Destory();
    }
}