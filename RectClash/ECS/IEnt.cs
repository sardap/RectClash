using System.Collections.Generic;

namespace RectClash.ECS
{
    public interface IEnt
    {
        IEnumerable<ICom> Coms { get; }

        IEnt Parent { get; set; }

        IEnumerable<IEnt> Children { get; }

        void AddChild(IEnt ent);

        T GetCom<T>() where T : ICom;

        void AddCom(ICom com);

        void AddComs(IEnumerable<ICom> com);

        void Update();
    } 
}