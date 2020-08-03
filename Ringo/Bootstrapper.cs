using Caliburn.Micro;
using HandyControl.Data;
using HandyControl.Tools;
using LibVLCSharp.Shared;
using Ringo.Helpers;
using Ringo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Windows;

namespace Ringo
{
    public class Bootstrapper : BootstrapperBase
    {

        SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            //Connect instance of itself
            _container.Instance(_container);

            // Connect Singletons
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

            // Register per request services
            _container
                .PerRequest<SubtitleHelper>();

            // Reflection - Gets current assembly, limits it to classes that ends with ViewModel (This can be very inneficient, but we only do it on startup)
            // Then we convert the list of types, and goes through every class that was found. It then registers the class to the container
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));

            // Set language to English
            //ConfigHelper.Instance.SetLang("en");
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //ConfigHelper.Instance.SetSystemVersionInfo(GetSystemVersionInfo());
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        public static SystemVersionInfo GetSystemVersionInfo()
        {
            var managementClass = new ManagementClass("Win32_OperatingSystem");
            var instances = managementClass.GetInstances();
            foreach (var instance in instances)
            {
                if (instance["Version"] is string version)
                {
                    var nums = version.Split('.').Select(int.Parse).ToList();
                    var info = new SystemVersionInfo(nums[0], nums[1], nums[2]);
                    return info;
                }
            }
            return default(SystemVersionInfo);
        }

    }
}
