namespace LaserCutterTools.BoxBuilder
{
    /// <summary>
    /// Deinfines the dimensions of a box. All dimensions are in inches.
    /// </summary>
    public interface IBoxSquare
    {
        /// <summary>
        /// X dimension in inches.
        /// </summary>
        decimal DimensionX { get; set; }

        /// <summary>
        /// Y dimension in inches.
        /// </summary>
        decimal DimensionY { get; set; }

        /// <summary>
        /// Z dimension in inches.
        /// </summary>
        decimal DimensionZ { get; set; }

        /// <summary>
        /// Determines whether or not the measurements are from the outer edges or inner edges of the box.
        /// </summary>
        MeasurementModel MeasurementModel { get; set; }
    }
}
