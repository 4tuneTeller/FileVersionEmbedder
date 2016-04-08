using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileVersionEmbedder
{
    public static class UrlExtensions
    {
        public static string SetUrlParameter(this string url, string paramName, string value)
        {
            var startedWithHttp = true;
            if (!url.StartsWith("http://"))
            {
                startedWithHttp = false;
                url = "http://" + url;
            }
            var result = new Uri(url).SetParameter(paramName, value).ToString();
            if (!startedWithHttp)
            {
                result = result.Remove(0, "http://".Length);
            }
            return result;
        }

        public static Uri SetParameter(this Uri url, string paramName, string value)
        {
            var queryParts = HttpUtility.ParseQueryString(url.Query);
            queryParts[paramName] = value;
            return new Uri(url.AbsoluteUriExcludingQuery() + '?' + queryParts);
        }

        public static string AbsoluteUriExcludingQuery(this Uri url)
        {
            return url.AbsoluteUri.Split('?').FirstOrDefault() ?? String.Empty;
        }
    }
}
