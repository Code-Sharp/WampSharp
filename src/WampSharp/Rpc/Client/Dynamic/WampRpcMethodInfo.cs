using System;
using System.Globalization;
using System.Reflection;

namespace WampSharp.Rpc.Client.Dynamic
{
    /// <summary>
    /// An implementation of <see cref="MethodInfo"/> for
    /// <see cref="IWampRpcSerializer{TMessage}"/> and
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

        public override string Name
        {
            get
            {
                return mName;
            }
        }

        public override Type ReturnType
        {
            get
            {
                return mReturnType;
            }
        }

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

        public override ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            get { throw new NotImplementedException(); }
        }

        public override Type DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        public override Type ReflectedType
        {
            get { throw new NotImplementedException(); }
        }

        public override RuntimeMethodHandle MethodHandle
        {
            get { throw new NotImplementedException(); }
        }

        public override MethodAttributes Attributes
        {
            get { throw new NotImplementedException(); }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}