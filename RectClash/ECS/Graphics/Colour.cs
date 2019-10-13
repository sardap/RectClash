namespace RectClash.ECS.Graphics
{
    public class Colour
    {
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public Colour(byte a = 0, byte r = 0, byte g = 0, byte b = 0)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
    }
}