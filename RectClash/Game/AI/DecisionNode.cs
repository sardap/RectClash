namespace RectClash.Game.AI
{
	public class DecisionNode : IDecisionTreeNode
	{
		public IDecisionTreeNode TrueNode
		{
			get;
			set;
		}

		public IDecisionTreeNode FalseNode
		{
			get;
			set;
		}

		public IDecisionNodeCondition Condition
		{
			get;
			set;
		}

		public IAIAction GetAction()
		{
			if(Condition.Resolve())
			{
				return TrueNode.GetAction();
			}
			else
			{
				return FalseNode.GetAction();
			}
		}
	}
}