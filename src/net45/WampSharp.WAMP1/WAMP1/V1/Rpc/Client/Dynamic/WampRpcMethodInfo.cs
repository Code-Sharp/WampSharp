using System;
using System.Globalization;
using System.Reflection;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// An implementation of <see cref="MethodInfo"/> for
    /// <see cref="IWampRpcSerializer"/> and
    /// <see cref="IWampProcUriMapper"/>.
    /// </summary>
    /// <remarks>
    /// This class was written because I think that it is a better solution
    /// than creating a structure that describes methods or creating overloads
    /// for these interfaces that receive method name and return type.
    /// </remarks>
    internal class WampRpcMethodInfo : MethodInfo
    {
        private readonly string mName;
        private readonly Type mReturnType;

        public WampRpcMethodInfo(string name, Type returnType)
        {
            mReturnType = returnType;
            mName = name;
        }

        public override string Name => mName;

        public override Type ReturnType => mReturnType;

        #region Not implemented

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetParameters()
        {
            throw new NotImplementedException();
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new NotImplementedException();
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        public override Type DeclaringType => throw new NotImplementedException();

        public override Type ReflectedType => throw new NotImplementedException();

        public override RuntimeMethodHandle MethodHandle => throw new NotImplementedException();

        public override MethodAttributes Attributes => throw new NotImplementedException();

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}