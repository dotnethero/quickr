using Autofac;
using Quickr.Services;
using Quickr.Utils;
using Quickr.ViewModels;
using Quickr.Views;
using System.Windows;

namespace Quickr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<RedisMultiplexer>().InstancePerLifetimeScope();
            builder.RegisterType<KeyViewModelFactory>().InstancePerLifetimeScope();

            builder.RegisterType<MainWindowViewModel>();
            builder.RegisterType<MainWindow>().FindConstructorsWith(Finders.Internal);

            var container = builder.Build();
            var window = container.Resolve<MainWindow>();
            window.Show();
        }
    }
}
