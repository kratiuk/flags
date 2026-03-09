using System.Drawing;

internal enum FlagLayerType
{
    Rectangle,
    Ellipse,
    Polygon,
    Label
}

internal sealed record FlagLayer(
    FlagLayerType Type,
    Color Color,
    RectangleF Bounds,
    PointF[]? Points = null,
    string Text = "");

internal sealed record FlagDefinition(params FlagLayer[] Layers);
