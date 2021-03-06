﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsControlsModule.cs" company="Catel development team">
//   Copyright (c) 2008 - 2014 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Catel
{
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using Catel.Reflection;
    using IoC;

#if !NET
    using Catel.Windows;
#endif

    /// <summary>
    /// MVVM module which allows the registration of default services in the service locator.
    /// </summary>
    public class MVVMModule : IServiceLocatorInitializer
    {
        /// <summary>
        /// Initializes the specified service locator.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public void Initialize(IServiceLocator serviceLocator)
        {
            Argument.IsNotNull(() => serviceLocator);

#if !NET && !PCL
            serviceLocator.RegisterTypeIfNotYetRegistered<IFrameworkElementLoadedManager, FrameworkElementLoadedManager>();
#endif

            serviceLocator.RegisterTypeIfNotYetRegistered<ICommandManager, CommandManager>();
            serviceLocator.RegisterTypeIfNotYetRegistered<IViewManager, ViewManager>();
            serviceLocator.RegisterTypeIfNotYetRegistered<IViewModelManager, ViewModelManager>();

            ViewModelServiceHelper.RegisterDefaultViewModelServices(serviceLocator);

            if (CatelEnvironment.IsInDesignMode)
            {
                foreach (var assembly in AssemblyHelper.GetLoadedAssemblies())
                {
                    var attributes = assembly.GetCustomAttributesEx(typeof (DesignTimeCodeAttribute));
                    foreach (var attribute in attributes)
                    {
                        // No need to do anything
                    }
                }
            }
        }
    }
}