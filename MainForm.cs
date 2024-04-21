namespace SubtitleEncoder;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        // Set the application icon
        using (Icon icon = Icon.FromHandle(new Bitmap("D:\\Codes\\PersonalProjects\\SubtitleEncoder\\my-logo.png").GetHicon()))
        {
            this.Icon = icon;
        }
    }
}