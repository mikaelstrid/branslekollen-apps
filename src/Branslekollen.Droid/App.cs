using System;
using Android.App;
using Android.Runtime;
using Autofac;
using Branslekollen.Core;
using Branslekollen.Core.Persistence;
using Branslekollen.Core.Services;
using Branslekollen.Core.ViewModels;
using Serilog;

namespace Branslekollen.Droid
{
    // http://arteksoftware.com/ioc-containers-with-xamarin/

    [Application]
    public class App : Application
    {
        public static IContainer Container { get; set; }

        public App(IntPtr h, JniHandleOwnership jho) : base(h, jho)
        {
        }

        public override void OnCreate()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterInstance(new DroidPlatform()).As<IPlatform>();
            builder.RegisterType<Configuration>().As<IConfiguration>();
#if DEBUG
            builder.RegisterType<DummyLocalStorage>().As<ILocalStorage>();
            builder.RegisterType<DummyVehicleService>().As<IVehicleService>().InstancePerLifetimeScope();
#else
            builder.RegisterType<LocalStorage>().As<ILocalStorage>();
            builder.RegisterType<VehicleService>().As<IVehicleService>().InstancePerLifetimeScope();
#endif
            builder.RegisterType<SplashViewModel>();
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<CreateVehicleViewModel>();
            builder.RegisterType<RefuelingsViewModel>();
            builder.RegisterType<AddRefuelingViewModel>();

            App.Container = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Trace()
                .CreateLogger();

            Log.Information("Application setup completed, starting application...");

            base.OnCreate();
        }
    }
}