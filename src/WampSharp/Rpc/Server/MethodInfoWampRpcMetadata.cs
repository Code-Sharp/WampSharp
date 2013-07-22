﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WampSharp.Rpc.Server
{
    public class MethodInfoWampRpcMetadata : IWampRpcMetadata
    {
        private readonly object mInstance;
        private readonly string mBaseUri;

        public MethodInfoWampRpcMetadata(object instance, string baseUri = null)
        {
            mInstance = instance;
            mBaseUri = baseUri;
        }

        public IEnumerable<IWampRpcMethod> GetServiceMethods()
        {
            Type type = mInstance.GetType();
            IEnumerable<Type> typesToExplore = GetTypesToExplore(type);

            return typesToExplore.SelectMany(currentType => GetServiceMethodsOfType(currentType));
        }

        private IEnumerable<Type> GetTypesToExplore(Type type)
        {
            yield return type;

            foreach (Type currentInterface in type.GetInterfaces())
            {
                yield return currentInterface;
            }
        }

        private IEnumerable<MethodInfoWampRpcMethod> GetServiceMethodsOfType(Type type)
        {
            return type.GetMethods()
                       .Where(method => method.IsDefined(typeof(WampRpcMethodAttribute), true))
                       .Select(method => CreateRpcMethod(method));
        }

        private MethodInfoWampRpcMethod CreateRpcMethod(MethodInfo method)
        {
            return new MethodInfoWampRpcMethod(mInstance, method, mBaseUri);
        }
    }
}