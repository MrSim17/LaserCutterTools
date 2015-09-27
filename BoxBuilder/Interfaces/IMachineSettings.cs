namespace BoxBuilder
{
    public interface IMachineSettings
    {
        /// <summary>
        /// X dimension of the maximum cuttable area of the machine in inches.
        /// </summary>
        decimal MaxX { get; set; }

        /// <summary>
        /// Y dimension of the maximum cuttable area of the machine in inches.
        /// </summary>
        decimal MaxY { get; set; }

        /// <summary>
        /// Width of the tool making the cuts. This value could apply to the width of the laser or CNC bit.
        /// </summary>
        decimal ToolSpacing { get; set; }
    }
}
