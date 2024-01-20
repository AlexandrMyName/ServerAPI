using WebServiceForClient.AWebService.Abstr;


namespace WebServiceForClient.AWebService.Models
{
    public struct WebResponse : IWebResponse
    {

        public string[] Responses { get; set; }

        public bool IsError { get; set; }
    }
}
