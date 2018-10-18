using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace trerep
{
    public static class ExtensionMethods
    {
        public static void AddProperty(this ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public static string toJsonString(this string queryString)
        {
            var nvs = HttpUtility.ParseQueryString(queryString);
            var dict = nvs.AllKeys.ToDictionary(k => k, k => nvs[k]);
            return JsonConvert.SerializeObject(dict, new KeyValuePairConverter());
        }
    }
}
