using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using LOLApiAdapter.CommonDefinitions;
using LOLApiAdapter.CommonDefinitions.Interfaces;
using Newtonsoft.Json;

namespace LOLApiAdapter.Utils
{
    public static class WebUtilFunctions
    {
        public static ILoLResponse GetEndpointResponseFromUrl<T, ET>(LoLWebAdapter handler, string url)
            where T : ILoLResponse
            where ET : ILoLResponse
        {
            var result = string.Empty;
            try
            {
                var response = handler.GetResponseFromUrl(url);

                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    using (var r = new StreamReader(stream))
                        result = r.ReadToEnd();

                    return JsonConvert.DeserializeObject<T>(result, new JsonSerializerSettings
                    {
                        Culture = CultureInfo.InvariantCulture,
                        FloatParseHandling = FloatParseHandling.Double,
                        
                    });
                }
            }
            catch (JsonException jex)
            {
                return JsonConvert.DeserializeObject<ET>(result);
            }
            catch (WebException e)
            {
                //Console.WriteLine(e);
            }

            return default(T);
        }

        public static async Task<ILoLResponse> GetEndpointResponseFromUrlAsync<T, ET>(LoLWebAdapter handler, string url)
            where T : ILoLResponse
            where ET : ILoLResponse
        {
            var response = await handler.GetResponseFromUrlAsync(url);
            var result = string.Empty;

            try
            {
                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    using (var r = new StreamReader(stream))
                        result = r.ReadToEnd();

                    return JsonConvert.DeserializeObject<T>(result);
                }
            }
            catch (JsonException jex)
            {
                return JsonConvert.DeserializeObject<ET>(result);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }

            return default(T);
        }
    }
}
