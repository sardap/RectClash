using RectClash.Misc;

namespace RectClash.ECS
{
    public class Matrix
    {
        public static Matrix Identity
        {
            get
            {
                return new Matrix(3, 3);
            }
        }

        private double[,] _ary;

        public Matrix(int x, int y)
        {
            _ary = new double[x,y];
        }
    }
}