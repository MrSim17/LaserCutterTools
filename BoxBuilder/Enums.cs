namespace BoxBuilder
{
    public enum TabPosition
    {
        Crest,
        Trough
    }

    public enum PieceSide
    {
        X,
        Y,
        XMinus,
        YMinus
    }

    public enum CubeSide
    {
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    }

    public enum CubeTopConfiguration
    {
        Closed,
        Open,
        Slotted,
        DiagonalSlotted
    }

    public enum SlotDirection
    {
        X,
        Y
    }

    public enum PartType
    {
        Divider
    }

    public enum MeasurementModel
    {
        /// <summary>
        /// Measurements are for the inside capacity of the box. Dimensions are measured from the inside edges.
        /// This means that the box will fit these measurements inside of the box.
        /// </summary>
        Inside,
        /// <summary>
        /// Measurements are for the outside bounds of the box. Dimensions are measured from the outside edges.
        /// This means that the box will be this size and fit into that measurement if placed in something else.
        /// </summary>
        Outside
    }
}
