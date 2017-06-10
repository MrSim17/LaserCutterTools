namespace LaserCutterTools.Common
{
    public struct PolarPointDouble
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="R">Radius</param>
        /// <param name="A">Angle</param>
        public PolarPointDouble(double R, double A)
        {
            this.R = R;
            this.A = A;
        }

        /// <summary>
        /// Radius
        /// </summary>
        public double R
        {
            get; set;
        }

        /// <summary>
        /// Angle
        /// </summary>
        public double A
        {
            get; set;
        }
    }
}
