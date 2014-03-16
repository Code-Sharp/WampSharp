using System;

namespace WampSharp.V2.Rpc
{
    public class RpcParameter
    {
        private readonly int mPosition;
        private readonly string mName;
        private readonly Type mType;
        private readonly object mDefaultValue;
        private readonly bool mHasDefaultValue;

        public RpcParameter(Type type, int position) :
            this(null, type, null, false, position)
        {
        }

        public RpcParameter(Type type, object defaultValue, int position) :
            this(null, type, defaultValue, true, position)
        {
        }

        public RpcParameter(string name, Type type, int position) :
            this(name, type, null, false, position)
        {
        }

        public RpcParameter(string name, Type type, object defaultValue, int position) :
            this(name, type, defaultValue, true, position)
        {
        }

        public RpcParameter(string name, Type type, object defaultValue, bool hasDefaultValue, int position)
        {
            mName = name;
            mType = StripByRef(type);
            mHasDefaultValue = hasDefaultValue;

            if (hasDefaultValue)
            {
                mDefaultValue = defaultValue;
            }
            
            mPosition = position;
        }

        private Type StripByRef(Type type)
        {
            if (type.IsByRef)
            {
                return type.GetElementType();
            }

            return type;
        }

        public string Name
        {
            get
            {
                return mName;
            }
        }

        public Type Type
        {
            get
            {
                return mType;
            }
        }

        public object DefaultValue
        {
            get
            {
                return mDefaultValue;
            }
        }

        public bool HasDefaultValue
        {
            get
            {
                return mHasDefaultValue;
            }
        }

        public int Position
        {
            get { return mPosition; }
        }
    }
}