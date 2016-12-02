using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace EnergyTrading.Types
{
    public class AnonymousTypeBuilder
    {
        private static readonly AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("atbAssName"), AssemblyBuilderAccess.Run);
        private static int count;

        private static string BuildNamespace()
        {
            return "anonNs" + Interlocked.Increment(ref count);
        }

        private static string BuildName()
        {
            return "anonTypeName" + Interlocked.Increment(ref count);
        }

        private static ModuleBuilder Module()
        {
            return assemblyBuilder.GetDynamicModule("atbModuleName") ?? assemblyBuilder.DefineDynamicModule("atbModuleName");
        }

        public static TypeBuilderWrapper DefineType(string name = null)
        {
            return new TypeBuilderWrapper(Module().DefineType($"{BuildNamespace()}.{name ?? BuildName()}", TypeAttributes.Public));
        }

        public static object FromDictionary(IDictionary<string, object> properties)
        {
            var wrapper = AnonymousTypeBuilder.DefineType();
            foreach (var pair in properties)
            {
                var type = pair.Value?.GetType() ?? typeof(string);
                wrapper.WithProperty(pair.Key, type, pair.Value);
            }
            return wrapper.Instance();
        }
    }
}