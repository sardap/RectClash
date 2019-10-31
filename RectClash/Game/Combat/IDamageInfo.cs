using RectClash.ECS;

namespace RectClash.Game.Combat
{
	public interface IDamageInfoCom : ICom
	{
		double DamageAmount { get; }
	}
}