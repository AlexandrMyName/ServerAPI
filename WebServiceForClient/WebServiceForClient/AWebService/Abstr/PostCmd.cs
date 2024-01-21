

namespace WebServiceForClient.AWebService.Abstr
{

    public struct PostCmd
    {

        public string Cmd { get; set; }

        public PostRequestType Type { get; set; }


        public PostCmd(string cmd, PostRequestType type)
        {

            Cmd = cmd;
            Type = type;
        }

        //public implicite oper 
    }
}
