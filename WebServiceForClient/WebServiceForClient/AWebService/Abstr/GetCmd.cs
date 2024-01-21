

namespace WebServiceForClient.AWebService.Abstr
{

    public struct GetCmd
    {

        public string Cmd { get; set; }

        public GetRequestType Type { get; set; }


        public GetCmd(string cmd, GetRequestType type)
        {

            Cmd = cmd;
            Type = type;
        }

    }
}
