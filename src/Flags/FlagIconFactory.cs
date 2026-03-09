using System.Drawing;
using System.Drawing.Drawing2D;

internal static class FlagIconFactory
{
    public static Icon Create(KeyboardLayoutInfo layout)
    {
        using var bitmap = CreateBitmap(layout, 16);
        var handle = bitmap.GetHicon();
        try
        {
            return (Icon)Icon.FromHandle(handle).Clone();
        }
        finally
        {
            NativeMethods.DestroyIcon(handle);
        }
    }

    public static Bitmap CreateBitmap(KeyboardLayoutInfo layout, int size)
    {
        var bitmap = new Bitmap(size, size);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.Clear(Color.Transparent);
        DrawFlag(graphics, new Rectangle(0, 0, size, size), layout.Flag);
        return bitmap;
    }

    private static void DrawFlag(Graphics graphics, Rectangle rect, FlagDefinition definition)
    {
        foreach (var layer in definition.Layers)
        {
            DrawLayer(graphics, rect, layer);
        }

        if (definition.Layers.Any(layer => layer.Type != FlagLayerType.Label))
        {
            using var borderPen = new Pen(Color.FromArgb(80, 80, 80));
            graphics.DrawRectangle(borderPen, rect);
        }
    }

    private static void DrawLayer(Graphics graphics, Rectangle rect, FlagLayer layer)
    {
        switch (layer.Type)
        {
            case FlagLayerType.Rectangle:
                using (var brush = new SolidBrush(layer.Color))
                {
                    graphics.FillRectangle(brush, ScaleBounds(rect, layer.Bounds));
                }
                break;
            case FlagLayerType.Ellipse:
                using (var brush = new SolidBrush(layer.Color))
                {
                    graphics.FillEllipse(brush, ScaleBounds(rect, layer.Bounds));
                }
                break;
            case FlagLayerType.Polygon:
                using (var brush = new SolidBrush(layer.Color))
                {
                    var points = layer.Points!
                        .Select(point => new PointF(
                            rect.X + (point.X * rect.Width),
                            rect.Y + (point.Y * rect.Height)))
                        .ToArray();
                    graphics.FillPolygon(brush, points);
                }
                break;
            case FlagLayerType.Label:
                using (var brush = new SolidBrush(layer.Color))
                using (var font = new Font("Segoe UI", 5.5f, FontStyle.Bold, GraphicsUnit.Pixel))
                {
                    var bounds = ScaleBounds(rect, layer.Bounds);
                    var size = graphics.MeasureString(layer.Text, font);
                    var x = bounds.X + ((bounds.Width - size.Width) / 2);
                    var y = bounds.Y + ((bounds.Height - size.Height) / 2);
                    graphics.DrawString(layer.Text, font, brush, x, y);
                }
                break;
        }
    }

    private static RectangleF ScaleBounds(Rectangle rect, RectangleF relativeBounds) =>
        new(
            rect.X + (relativeBounds.X * rect.Width),
            rect.Y + (relativeBounds.Y * rect.Height),
            relativeBounds.Width * rect.Width,
            relativeBounds.Height * rect.Height);
}
