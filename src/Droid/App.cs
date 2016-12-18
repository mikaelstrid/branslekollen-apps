using System;
using Android.App;
using Android.Runtime;
using Autofac;
using Branslekollen.Core;
using Branslekollen.Core.Services;
using Branslekollen.Core.ViewModels;

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
            builder.RegisterInstance(new VehicleService()).As<IVehicleService>();
            builder.RegisterType<CreateVehicleViewModel>();

            App.Container = builder.Build();

            base.OnCreate();
        }
    }
}