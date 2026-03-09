using System.Drawing;

internal static class FlagCatalog
{
    public static readonly FlagDefinition UnitedStates = new(
        Rect(Color.Firebrick, 0f, 0f, 1f, 0.125f),
        Rect(Color.White, 0f, 0.125f, 1f, 0.125f),
        Rect(Color.Firebrick, 0f, 0.25f, 1f, 0.125f),
        Rect(Color.White, 0f, 0.375f, 1f, 0.125f),
        Rect(Color.Firebrick, 0f, 0.5f, 1f, 0.125f),
        Rect(Color.White, 0f, 0.625f, 1f, 0.125f),
        Rect(Color.Firebrick, 0f, 0.75f, 1f, 0.125f),
        Rect(Color.White, 0f, 0.875f, 1f, 0.125f),
        Rect(Color.MidnightBlue, 0f, 0f, 0.5f, 0.5f));

    public static readonly FlagDefinition UnitedKingdom = new(
        Rect(Color.MidnightBlue, 0f, 0f, 1f, 1f),
        Polygon(Color.White, new[]
        {
            new PointF(0f, 0f),
            new PointF(0.12f, 0f),
            new PointF(1f, 0.88f),
            new PointF(1f, 1f),
            new PointF(0.88f, 1f),
            new PointF(0f, 0.12f)
        }),
        Polygon(Color.White, new[]
        {
            new PointF(0.88f, 0f),
            new PointF(1f, 0f),
            new PointF(1f, 0.12f),
            new PointF(0.12f, 1f),
            new PointF(0f, 1f),
            new PointF(0f, 0.88f)
        }),
        Polygon(Color.Firebrick, new[]
        {
            new PointF(0f, 0.07f),
            new PointF(0.07f, 0f),
            new PointF(1f, 0.93f),
            new PointF(0.93f, 1f)
        }),
        Polygon(Color.Firebrick, new[]
        {
            new PointF(0.93f, 0f),
            new PointF(1f, 0.07f),
            new PointF(0.07f, 1f),
            new PointF(0f, 0.93f)
        }),
        Rect(Color.White, 0f, 0.36f, 1f, 0.28f),
        Rect(Color.White, 0.36f, 0f, 0.28f, 1f),
        Rect(Color.Firebrick, 0f, 0.44f, 1f, 0.12f),
        Rect(Color.Firebrick, 0.44f, 0f, 0.12f, 1f));

    public static readonly FlagDefinition Ukraine = new(
        Rect(Color.RoyalBlue, 0f, 0f, 1f, 0.5f),
        Rect(Color.Gold, 0f, 0.5f, 1f, 0.5f));

    public static readonly FlagDefinition Germany = new(
        Rect(Color.Black, 0f, 0f, 1f, 1f / 3f),
        Rect(Color.Firebrick, 0f, 1f / 3f, 1f, 1f / 3f),
        Rect(Color.Gold, 0f, 2f / 3f, 1f, 1f / 3f));

    public static readonly FlagDefinition France = new(
        Rect(Color.RoyalBlue, 0f, 0f, 1f / 3f, 1f),
        Rect(Color.White, 1f / 3f, 0f, 1f / 3f, 1f),
        Rect(Color.Firebrick, 2f / 3f, 0f, 1f / 3f, 1f));

    public static readonly FlagDefinition Spain = new(
        Rect(Color.Firebrick, 0f, 0f, 1f, 0.25f),
        Rect(Color.Gold, 0f, 0.25f, 1f, 0.5f),
        Rect(Color.Goldenrod, 0.109375f, 0.25f, 0.21875f, 0.375f),
        Rect(Color.Firebrick, 0.15625f, 0.328125f, 0.125f, 0.21875f),
        Rect(Color.Gold, 0.203125f, 0.375f, 0.03125f, 0.125f),
        Rect(Color.Goldenrod, 0.171875f, 0.203125f, 0.09375f, 0.078125f),
        Rect(Color.Firebrick, 0f, 0.75f, 1f, 0.25f));

    public static readonly FlagDefinition Italy = new(
        Rect(Color.ForestGreen, 0f, 0f, 1f / 3f, 1f),
        Rect(Color.White, 1f / 3f, 0f, 1f / 3f, 1f),
        Rect(Color.Firebrick, 2f / 3f, 0f, 1f / 3f, 1f));

    public static readonly FlagDefinition Poland = new(
        Rect(Color.White, 0f, 0f, 1f, 0.5f),
        Rect(Color.Crimson, 0f, 0.5f, 1f, 0.5f));

    public static readonly FlagDefinition Czechia = new(
        Rect(Color.White, 0f, 0f, 1f, 0.5f),
        Rect(Color.Firebrick, 0f, 0.5f, 1f, 0.5f),
        Polygon(Color.RoyalBlue, new[]
        {
            new PointF(0f, 0f),
            new PointF(0.5f, 0.5f),
            new PointF(0f, 1f)
        }));

    public static readonly FlagDefinition Japan = new(
        Rect(Color.White, 0f, 0f, 1f, 1f),
        Ellipse(Color.Crimson, 0.3125f, 0.28125f, 0.375f, 0.375f));

    public static FlagDefinition Code(string label) => new(
        Label(Color.White, label, 0f, 0f, 1f, 1f));

    private static FlagLayer Rect(Color color, float x, float y, float width, float height) =>
        new(FlagLayerType.Rectangle, color, new RectangleF(x, y, width, height));

    private static FlagLayer Ellipse(Color color, float x, float y, float width, float height) =>
        new(FlagLayerType.Ellipse, color, new RectangleF(x, y, width, height));

    private static FlagLayer Polygon(Color color, PointF[] points) =>
        new(FlagLayerType.Polygon, color, RectangleF.Empty, points);

    private static FlagLayer Label(Color color, string text, float x, float y, float width, float height) =>
        new(FlagLayerType.Label, color, new RectangleF(x, y, width, height), Text: text);
}
