using System;
using System.Reflection;
using WampSharp.Core.Utilities;

namespace WampSharp.V2.Rpc
{
    internal static class WampResultAttributeExtensions
    {
        public static bool HasMultivaluedResult(this MethodInfo method, Type typeToCheck)
        {
            WampResultAttribute resultAttribute = 
                method.ReturnParameter.GetCustomAttribute<WampResultAttribute>(true);

            Type unwrapped = typeToCheck;

            if (!unwrapped.IsArray)
            {
                return false;
            }

            if ((resultAttribute != null) &&
                (resultAttribute.CollectionResultTreatment == CollectionResultTreatment.Multivalued))
            {
                return true;
            }

            return false;
        }

        public static CollectionResultTreatment GetCollectionResultTreatment(this MethodInfo method)
        {
            CollectionResultTreatment result;

            WampResultAttribute wampResultAttribute =
                method.ReturnParameter.GetCustomAttribute<WampResultAttribute>();

            if (wampResultAttribute == null)
            {
                result = CollectionResultTreatment.SingleValue;
            }
            else
            {
                result =
                    wampResultAttribute.CollectionResultTreatment;
            }

            return result;
        }
    }
}