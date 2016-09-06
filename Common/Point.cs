namespace LaserCutterTools.Common
{
    public struct Point
    {
        public Point(decimal X, decimal Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public decimal X
        {
            get; set;
        }

        public decimal Y
        {
            get; set;
        }
    }
}
