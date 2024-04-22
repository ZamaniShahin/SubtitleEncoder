namespace SubtitleEncoder;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        string imagePath = Path.Combine(Application.StartupPath, "logo.png");
        using (Icon icon = Icon.FromHandle(new Bitmap(imagePath).GetHicon()))
        {
            this.Icon = icon;
        }

    }
}