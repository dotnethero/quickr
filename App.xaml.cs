using System;
using Autofac;
using Quickr.Services;
using Quickr.Utils;
using Quickr.ViewModels;
using Quickr.Views;
using System.Windows;
using System.Windows.Threading;

namespace Quickr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow _main;

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<RedisMultiplexer>().InstancePerLifetimeScope();
            builder.RegisterType<KeyViewModelFactory>().InstancePerLifetimeScope();

            builder.RegisterType<MainWindowViewModel>();
            builder.RegisterType<MainWindow>().FindConstructorsWith(Finders.Internal);

            var container = builder.Build();
            _main = container.Resolve<MainWindow>();
            _main.Show();

            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var window = new ErrorWindow();
            window.DataContext = e.Exception;
            window.Owner = _main;
            window.ShowDialog();
            e.Handled = true;
        }
    }
}
