namespace RectClash.ECS.Exception
{
    [System.Serializable]
    public class ComNotFoundException : System.Exception
    {
        public ComNotFoundException() { }
        public ComNotFoundException(string message) : base(message) { }
        public ComNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected ComNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}