using System.Diagnostics;
using RectClash.ECS;
using RectClash.Game.Combat;
using RectClash.Misc;

namespace RectClash.Game.Unit
{
    public class UnitActionContCom : Com, IGameObv
    {
        private UnitInfoCom _unitInfoCom { get; set; }

		protected override void InternalStart()
		{
			_unitInfoCom = Owner.GetCom<UnitInfoCom>();
		}

		private void CheckTurnComplete()
		{
			if(_unitInfoCom.TurnTaken)
			{
				Owner.Parent.GetCom<CellInfoCom>().ChangeState(CellInfoCom.State.TurnComplete);
			}
		}

		public void OnNotify(IEnt ent, GameEvent evt)
		{
			switch(evt)
			{
				case GameEvent.UNIT_SELECTED:
					Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.SelectSound);
					break;

				case GameEvent.UNIT_MOVED:
					Debug.Assert(!_unitInfoCom.MoveTaken);
					_unitInfoCom.MoveTaken = true;
					Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.MoveSound);
					CheckTurnComplete();
					break;

				case GameEvent.ATTACK_TARGET:
					Debug.Assert(!_unitInfoCom.AttackTaken);
					_unitInfoCom.AttackTaken = true;
					if(PerformAttack(ent))
						Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.AttackSound);
					CheckTurnComplete();
					break;

				case GameEvent.RECEIVE_DAMAGE:
					Debug.Assert(ent.Tags.Contains(Tags.UNIT));
					Owner.GetCom<HealthCom>().ReceiveDamage(ent.GetCom<IDamageInfoCom>());
					Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.DamageSound);
					break;

				case GameEvent.UNIT_DIED:
					Engine.Instance.Sound.PlayRandomSound(_unitInfoCom.SoundInfo.DeathSound);
					Engine.Instance.DestoryEnt(Owner);
					break;
			}
		} 

		private bool PerformAttack(IEnt target)
		{
			var targetUnitCom = target.GetCom<HealthCom>();
			var attackerUnitCom = _unitInfoCom;

			var damageVariation = attackerUnitCom.DamageAmount * GameConstants.UNIT_DAMAGE_VARIATION;
			var damageModifer = Utility.RandomDouble(-(damageVariation), damageVariation);

			var result = targetUnitCom.CurrentHealth - attackerUnitCom.DamageAmount + damageModifer >= 0;

			target.GetCom<UnitActionContCom>().OnNotify
			(
				Owner,
				GameEvent.RECEIVE_DAMAGE
			);

			return result;
		}
	}
}