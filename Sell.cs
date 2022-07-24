namespace PicturePixelFilter
{
    public class Sell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }

        public Sell(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }
}