using LOLApiAdapter.CommonDefinitions;
using LOLApiAdapter.CommonDefinitions.Interfaces;

namespace LOLApiAdapter.ApiResponseStructures
{
    public class ErrorResponse : ILoLResponse
    {
        private Status  status      { get; set; }
    }

    public class Status
    {
        public string   message     { get; set; }
        public int      status_code { get; set; }
    }
}
