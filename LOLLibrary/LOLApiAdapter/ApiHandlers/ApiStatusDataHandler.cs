using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOLApiAdapter.ApiAdapters;
using LOLApiAdapter.ApiResponseStructures;
using LOLApiAdapter.CommonDefinitions.Interfaces;
using LOLApiAdapter.Utils;

namespace LOLApiAdapter.ApiHandlers
{
    public class ApiStatusDataHandler : LoLApiDataHandlerBase
    {
        public ApiStatusDataHandler(string apiKey, string region) : base(apiKey, region)
        {
        }

        public ILoLResponse GetApiStatus()
        {
            var queryUrl = ApiStatusAdapter.GetApiStatus(Region);
            queryUrl = AppendApiKey(queryUrl);

            return WebUtilFunctions.GetEndpointResponseFromUrl<ApiStatusResponse, ErrorResponse>(Handler, queryUrl);
        }

        public async Task<ILoLResponse> GetApiStatusAsync()
        {
            var queryUrl = ApiStatusAdapter.GetApiStatus(Region);
            queryUrl = AppendApiKey(queryUrl);

            return await WebUtilFunctions.GetEndpointResponseFromUrlAsync<ApiStatusResponse, ErrorResponse>(Handler, queryUrl);
        }
    }
}
