using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using TimeManager.Model;
using TimeManager.Services;
using TimeManager.Views;

namespace TimeManager
{
    public partial class App : PrismApplication
    {

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<AppDbContext>();
            containerRegistry.RegisterDialog<TimeEntries>();
        }
        protected override Window CreateShell()
        {

            MainWindow shell = Container.Resolve<MainWindow>();
            return shell;
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<MainModule>();
        }
    }
}
