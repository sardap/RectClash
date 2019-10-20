namespace RectClash.ECS.Exception
{
    [System.Serializable]
    public class CannotRemoveNoneOwnedCom : System.Exception
    {
        public CannotRemoveNoneOwnedCom() { }
        public CannotRemoveNoneOwnedCom(string message) : base(message) { }
        public CannotRemoveNoneOwnedCom(string message, System.Exception inner) : base(message, inner) { }
        protected CannotRemoveNoneOwnedCom(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}