#if CASTLE

using System;
using System.Threading;
using Castle.DynamicProxy;

namespace WampSharp.Core.Utilities
{
    internal static class CastleDynamicProxyGenerator
    {
        private static string PREFIX = typeof(CastleDynamicProxyGenerator).Assembly.GetName().Name + ".";

        public static readonly ProxyGenerator Instance = GetProxyGenerator();

        private static ProxyGenerator GetProxyGenerator()
        {
            ModuleScope moduleScope = GetModuleScope();
            return new ProxyGenerator(new DefaultProxyBuilder(moduleScope));
        }

        private static ModuleScope GetModuleScope()
        {
            return new ModuleScope(savePhysicalAssembly: false,
                                   disableSignedModule: false,
                                   strongAssemblyName: PREFIX + ModuleScope.DEFAULT_ASSEMBLY_NAME,
                                   strongModulePath: PREFIX + ModuleScope.DEFAULT_FILE_NAME,
                                   weakAssemblyName: PREFIX + ModuleScope.DEFAULT_ASSEMBLY_NAME,
                                   weakModulePath: PREFIX + ModuleScope.DEFAULT_FILE_NAME);
        }
    }
}

#endif