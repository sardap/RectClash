namespace RectClash.ECS.Exception
{
    [System.Serializable]
    public class PostionComAlreadyCreated : System.Exception
    {
        public PostionComAlreadyCreated() { }
        public PostionComAlreadyCreated(string message) : base(message) { }
        public PostionComAlreadyCreated(string message, System.Exception inner) : base(message, inner) { }
        protected PostionComAlreadyCreated(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}