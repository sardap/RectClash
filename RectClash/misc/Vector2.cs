namespace RectClash.misc
{
    public class Vector2<T>
    {
        public Vector2(T x, T y)
        {
            X = x;
            Y = y;
        }

        public Vector2()
        {

        }

        public T X { get; set; }
        
        public T Y { get; set; }
    }
}