namespace OrganizerApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            App.Current.UserAppTheme = AppTheme.Dark;
            MainPage = new AppShell();
        }
    }
}
