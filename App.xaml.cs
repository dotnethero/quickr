using Autofac;
using Quickr.Services;
using Quickr.Utils;
using Quickr.ViewModels;
using Quickr.ViewModels.Data;
using Quickr.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            builder.RegisterType<RedisProxy>().InstancePerLifetimeScope();
            builder.RegisterType<KeyViewModelFactory>().InstancePerLifetimeScope();

            // keys
            builder.RegisterType<StringViewModel>();
            builder.RegisterType<UnsortedSetViewModel>();
            builder.RegisterType<HashSetViewModel>();
            builder.RegisterType<ListViewModel>();
            builder.RegisterType<SortedSetViewModel>();

            // TODO: add support
            builder.RegisterType<PropertiesViewModel>();
            builder.RegisterType<ValueViewModel>();

            builder.RegisterType<MainWindowViewModel>();
            builder.RegisterType<MainWindow>().FindConstructorsWith(Finders.Internal);

            var container = builder.Build();
            var window = container.Resolve<MainWindow>();
            window.Show();
        }
    }
}
