namespace LaserCutterTools.Common
{
    public sealed class MachineSettings : IMachineSettings
    {
        public decimal MaxX
        {
            get;
            set;
        }

        public decimal MaxY
        {
            get;
            set;
        }

        public decimal ToolSpacing
        {
            get;
            set;
        }
    }
}
