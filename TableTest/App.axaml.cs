using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TableTest.Model;

namespace TableTest;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var database = new Database();

            desktop.MainWindow = new MainWindow
            {
                DataContext = database
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
