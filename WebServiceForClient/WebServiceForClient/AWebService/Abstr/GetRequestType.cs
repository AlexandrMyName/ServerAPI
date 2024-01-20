

namespace WebServiceForClient.AWebService.Abstr
{

    public enum GetRequestType : byte
    {

        GetParentTreeItems = 0,
        GetChildTreeItems = 1,
        GetTreeItem = 2,
        None = 4,
    }
}
