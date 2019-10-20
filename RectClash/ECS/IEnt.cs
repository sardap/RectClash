using System.Collections.Generic;
using RectClash.ECS.Graphics;

namespace RectClash.ECS
{
    public interface IEnt
    {
        IEnt Parent { get; set; }

        IEnumerable<ICom> Coms { get; }

        IEnumerable<IEnt> Children { get; }

        IEnumerable<IDrawableCom> DrawableComs { get; }

        IList<string> Tags { get; }

        PostionCom PostionCom { get; }

        void AddChild(IEnt ent);

        void ChangeParent(IEnt ent);

        T GetCom<T>() where T : ICom;

        T AddCom<T>(T com) where T : ICom;

		void RemoveCom(ICom com);

        void AddComs(IEnumerable<ICom> com);

        void Update();

		void Destory();
    } 
}