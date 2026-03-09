using System.Drawing;
using System.Windows.Forms;

ApplicationConfiguration.Initialize();
Application.Run(new TrayApplicationContext());

internal sealed class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon notifyIcon;
    private readonly ToolStripMenuItem currentLayoutMenuItem;
    private readonly ToolStripMenuItem showAllFlagsMenuItem;
    private readonly Dictionary<string, Icon> iconCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly System.Windows.Forms.Timer updateTimer;
    private readonly KeyboardLayoutTracker layoutTracker;
    private AllFlagsForm? allFlagsForm;

    private IntPtr lastLayout = IntPtr.Zero;
    private ushort lastLanguageId;

    public TrayApplicationContext()
    {
        currentLayoutMenuItem = new ToolStripMenuItem("Layout: unknown")
        {
            Enabled = false
        };
        showAllFlagsMenuItem = new ToolStripMenuItem("Show all flags", null, (_, _) => ShowAllFlags());

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add(currentLayoutMenuItem);
        contextMenu.Items.Add(showAllFlagsMenuItem);
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add("Exit", null, (_, _) => ExitThread());

        notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = contextMenu,
            Text = "Keyboard layout flag",
            Visible = true,
            Icon = CreateFallbackIcon()
        };
        layoutTracker = new KeyboardLayoutTracker();

        updateTimer = new System.Windows.Forms.Timer
        {
            Interval = 50
        };
        updateTimer.Tick += (_, _) => RefreshLayout();
        updateTimer.Start();

        RefreshLayout();
    }

    protected override void ExitThreadCore()
    {
        updateTimer.Stop();
        updateTimer.Dispose();
        allFlagsForm?.Close();
        allFlagsForm?.Dispose();
        notifyIcon.Dispose();

        foreach (var icon in iconCache.Values)
        {
            icon.Dispose();
        }

        base.ExitThreadCore();
    }

    private void RefreshLayout()
    {
        if (!layoutTracker.TryGetCurrent(out var snapshot))
        {
            return;
        }

        if (snapshot.Handle == lastLayout && snapshot.LanguageId == lastLanguageId)
        {
            return;
        }

        lastLayout = snapshot.Handle;
        lastLanguageId = snapshot.LanguageId;

        var layout = KeyboardLayouts.FromLanguageId(snapshot.LanguageId);
        currentLayoutMenuItem.Text = $"Layout: {layout.DisplayName}";
        notifyIcon.Icon = GetOrCreateIcon(layout);
        notifyIcon.Text = BuildTooltip(layout);
    }

    private Icon GetOrCreateIcon(KeyboardLayoutInfo layout)
    {
        if (iconCache.TryGetValue(layout.FlagCode, out var icon))
        {
            return icon;
        }

        icon = FlagIconFactory.Create(layout);
        iconCache[layout.FlagCode] = icon;
        return icon;
    }

    private static Icon CreateFallbackIcon() => FlagIconFactory.Create(KeyboardLayouts.FromLanguageId(0));

    private static string BuildTooltip(KeyboardLayoutInfo layout)
    {
        var text = layout.DisplayName;
        return text.Length <= 63 ? text : text[..63];
    }

    private void ShowAllFlags()
    {
        if (allFlagsForm is null || allFlagsForm.IsDisposed)
        {
            allFlagsForm = new AllFlagsForm();
        }

        allFlagsForm.Show();
        allFlagsForm.BringToFront();
        allFlagsForm.Activate();
    }
}
