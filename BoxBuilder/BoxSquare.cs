namespace LaserCutterTools.BoxBuilder
{
    public sealed class BoxSquare : IBoxSquare
    {
        public decimal DimensionX { get; set; }
        public decimal DimensionY { get; set; }
        public decimal DimensionZ { get; set; }
        public MeasurementModel MeasurementModel { get; set; }

        public BoxSquare()
        {
            DimensionX = 0m;
            DimensionY = 0m;
            DimensionZ = 0m;
            MeasurementModel = MeasurementModel.Outside; // Always default to outside model
        }
    }
}