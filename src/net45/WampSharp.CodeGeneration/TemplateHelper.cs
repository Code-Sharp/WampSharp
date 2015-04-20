using System.Collections.Generic;

namespace WampSharp.CodeGeneration
{
    internal static class TemplateHelper
    {
        public static string ProcessTemplate(string template, IDictionary<string, string> dictionary)
        {
            string result = template;

            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                result = result.Replace(string.Format("{{${0}}}", keyValuePair.Key),
                                        keyValuePair.Value);
            }

            return result;
        }
    }
}