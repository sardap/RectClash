namespace RectClash.ECS.Exception
{
    [System.Serializable]
    public class OnStartRanTwiceException : System.Exception
    {
        public OnStartRanTwiceException() { }
        public OnStartRanTwiceException(string message) : base(message) { }
        public OnStartRanTwiceException(string message, System.Exception inner) : base(message, inner) { }
        protected OnStartRanTwiceException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}