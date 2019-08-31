using System;
using System.Reflection;
using WampSharp.V2.Core;

namespace WampSharp.V2.Rpc
{
    public class RpcParameter : LocalParameter
    {
        public RpcParameter(Type type, int position) : base(type, position)
        {
        }

        public RpcParameter(Type type, object defaultValue, int position) : base(type, defaultValue, position)
        {
        }

        public RpcParameter(string name, Type type, int position) : base(name, type, position)
        {
        }

        public RpcParameter(string name, Type type, object defaultValue, int position) : base(name, type, defaultValue, position)
        {
        }

        public RpcParameter(string name, Type type, object defaultValue, bool hasDefaultValue, int position) : base(name, type, defaultValue, hasDefaultValue, position)
        {
        }

        public RpcParameter(ParameterInfo parameter) : base(parameter)
        {
        }
    }
}