using System.Drawing;
using System.Windows.Forms;

internal sealed class AllFlagsForm : Form
{
    private readonly ImageList imageList;
    private readonly ListView listView;

    public AllFlagsForm()
    {
        Text = "Supported flags";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(420, 360);

        imageList = new ImageList
        {
            ImageSize = new Size(24, 24),
            ColorDepth = ColorDepth.Depth32Bit
        };

        listView = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            HeaderStyle = ColumnHeaderStyle.Nonclickable,
            MultiSelect = false,
            SmallImageList = imageList
        };
        listView.Columns.Add("Flag", 64);
        listView.Columns.Add("Layout", 320);

        Controls.Add(listView);

        LoadLayouts();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            imageList.Dispose();
            listView.Dispose();
        }

        base.Dispose(disposing);
    }

    private void LoadLayouts()
    {
        foreach (var layout in KeyboardLayouts.GetSupportedLayouts())
        {
            if (!imageList.Images.ContainsKey(layout.FlagCode))
            {
                using var bitmap = FlagIconFactory.CreateBitmap(layout, 24);
                imageList.Images.Add(layout.FlagCode, (Bitmap)bitmap.Clone());
            }

            var item = new ListViewItem(layout.FlagCode)
            {
                ImageKey = layout.FlagCode
            };
            item.SubItems.Add(layout.DisplayName);
            listView.Items.Add(item);
        }
    }
}
