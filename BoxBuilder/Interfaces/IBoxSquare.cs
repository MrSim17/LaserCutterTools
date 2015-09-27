namespace BoxBuilder
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
    }
}
