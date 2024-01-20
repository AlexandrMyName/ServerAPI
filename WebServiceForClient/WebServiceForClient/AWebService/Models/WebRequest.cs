
using WebServiceForClient.AWebService.Abstr;


namespace WebServiceForClient.AWebService.Models
{

    public struct WebRequest : IWebRequest
    {

        public PostCmd PostCmd { get; set; }

        public GetCmd GetCmd { get; set; }

    }
}
