using System.Collections.Generic;


namespace WebServiceForClient.AWebService.Models
{

    internal static class LocalMemoryDataModel
    {

        internal static List<RootTreeResponse> Resposes { get; set; }

        internal static Dictionary<string, bool?> Activities { get; set; }
    }
}
