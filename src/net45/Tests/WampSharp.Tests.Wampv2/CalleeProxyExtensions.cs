using WampSharp.V2;

namespace WampSharp.Tests.Wampv2
{
    internal static class CalleeProxyExtensions
    {
        public static TProxy GetCalleeProxyPortable<TProxy>(this IWampRealmServiceProvider provider) where TProxy : class
        {
            return provider.GetCalleeProxyPortable<TProxy>(new CalleeProxyInterceptor());
        }

        public static TProxy GetCalleeProxyPortable<TProxy>(this IWampRealmServiceProvider provider, ICalleeProxyInterceptor interceptor) where TProxy : class
        {
#if !PCL
            return provider.GetCalleeProxy<TProxy>(interceptor);
#else
            try
            {
                return provider.GetCalleeProxy<TProxy>(interceptor);
            }
            catch (NotSupportedException ex)
            {
                GeneratedCodeException exception = 
                    ex.InnerException as GeneratedCodeException;

                var code = exception.GeneratedCode;

                Type instanceType = 
                    GetInstanceType<TProxy>(code);

                MethodInfo methodToInvoke =
                    typeof (IWampRealmServiceProvider)
                        .GetMethods()
                        .FirstOrDefault(x => x.Name == "GetCalleeProxy" &&
                                             x.GetParameters().Any());

                TProxy result =
                    methodToInvoke.MakeGenericMethod(instanceType)
                                  .Invoke(provider, new object[] {interceptor}) as TProxy;

                return result;
            }
#endif
        }

#if PCL
        private static Type GetInstanceType<T>(string code)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider
                (new Dictionary<string, string>
            {
                { "CompilerVersion", "v4.0" }
            });

            CompilerParameters compilerParameters = new CompilerParameters();

            compilerParameters.ReferencedAssemblies.AddRange(
                GetAssemblies(typeof(T).Assembly)
                    .Union(new[] { typeof(Enumerable).Assembly })
                    .Select(x => Path.GetFileName(x.Location)).ToArray());

            CompilerResults compilerResults =
                provider.CompileAssemblyFromSource(compilerParameters, code);

            Assembly assembly = compilerResults.CompiledAssembly;

            Type type =
                assembly.GetTypes().FirstOrDefault(x => typeof(T).IsAssignableFrom(x));

            return type;
        }

        private static IEnumerable<Assembly> GetAssemblies(Assembly assembly)
        {
            HashSet<Assembly> hashSet = new HashSet<Assembly>();
            GetAssemblies(assembly, hashSet);
            return hashSet;
        }

        private static void GetAssemblies(Assembly assembly, HashSet<Assembly> assemblies)
        {
            if (!assemblies.Add(assembly))
            {
                return;
            }

            foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
            {
                Assembly loaded = Assembly.Load(assemblyName);
                GetAssemblies(loaded, assemblies);
            }
        }    

#endif
    }
}