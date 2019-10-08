using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using PaulECS;
using VelcroPhysics.Dynamics;

namespace RectClash
{
	class MoveTowardsEnemyLogic : IMovementLogic
	{
		private Vector2? _target = null;

		public World World { get; set; }

		public void Step(IEntity entity)
		{
			if (_target == null)
				FindTarget(entity);


			var start = entity.GetCom<IMovementDataCom>().Postion;
			var end = (Vector2)_target;
			float distance = Vector2.Distance(start, end);
			Vector2 direction = Vector2.Normalize(end - start);

			/*
			entity.GetCom<IMovementDataCom>().Volicty = direction * speed * elapsed;

			object.Position += ;
			if (Vector2.Distance(start, object.Position) >= distance)
			{
				object.Position = end;
				moving = false;
			}
			*/
		}

		private void FindTarget(IEntity entity)
		{
			Vector2 point = Vector2.Zero, normal = Vector2.Zero;

			const float angle = 0.0f;
			const float l = 11.0f;
			Vector2 point1 = new Vector2(0.0f, 10.0f);
			Vector2 d = new Vector2(l * (float)Math.Cos(angle), l * (float)Math.Sin(angle));
			Vector2 point2 = point1 + d;

			var sortedList = new SortedList<Vector2, IEntity>();

			World.RayCast((f, p, n, fr) =>
			{
				Body body = f.Body;
				if (body.UserData != null)
				{
					var other = (IEntity)body.UserData;

					var otherTeamCom = other.GetCom<ITeamData>();

					if(otherTeamCom != null && otherTeamCom.TeamInfo != entity.GetCom<ITeamData>().TeamInfo)
					{
						sortedList.Add(n, other);
					}
				}

				point = p;
				normal = n;
				return fr;

			}, point1, point2);

			_target = sortedList.Values[0].GetCom<IMovementDataCom>().Postion;
		}
	}
}
