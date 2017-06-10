namespace LaserCutterTools.Common
{
    public struct PointDouble
    {
        public PointDouble(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public double X
        {
            get; set;
        }

        public double Y
        {
            get; set;
        }
    }
}
