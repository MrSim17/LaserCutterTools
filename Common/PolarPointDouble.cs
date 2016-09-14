namespace LaserCutterTools.Common
{
    public struct PolarPointDouble
    {
        public PolarPointDouble(double R, double A)
        {
            this.R = R;
            this.A = A;
        }

        public double R
        {
            get; set;
        }

        public double A
        {
            get; set;
        }
    }
}
