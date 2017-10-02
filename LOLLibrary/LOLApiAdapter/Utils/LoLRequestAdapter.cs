using System.Net;
using System.Threading.Tasks;

namespace LOLApiAdapter.Utils
{
    public class LoLWebAdapter
    {
        public WebResponse GetResponseFromUrl(string url)
        {
            var request = WebRequest.Create(url);
            var returnResponse = request.GetResponse();

            return returnResponse;
        }

        public async Task<WebResponse> GetResponseFromUrlAsync(string url)
        {
            var request = WebRequest.Create(url);
            var returnResponse = await request.GetResponseAsync();

            return returnResponse;
        }
    }
}