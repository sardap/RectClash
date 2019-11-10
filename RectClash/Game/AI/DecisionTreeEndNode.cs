namespace RectClash.Game.AI
{
	public class DecisionTreeEndNode : IDecisionTreeNode
	{
		public IAIAction Action
		{
			get;
			set;
		}

		public IAIAction GetAction()
		{
			return Action;
		}
	}
}