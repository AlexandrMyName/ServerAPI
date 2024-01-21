

namespace WebServiceForClient.AWebService.Abstr
{
    public interface IWebResponse
    {

        string[] Responses { get; }

        bool IsError { get; }
    }
}
