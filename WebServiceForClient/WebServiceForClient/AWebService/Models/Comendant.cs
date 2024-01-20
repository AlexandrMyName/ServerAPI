using System;
using System.Collections.Generic;
using WebServiceForClient.AWebService.Abstr;


namespace WebServiceForClient.AWebService.Models
{


    public class Comendant : IDisposable
    {

        public List<ComendantNode> NodesCmd = new();

        public void Dispose() =>  NodesCmd.ForEach(NodeCmd => NodeCmd.Dispose());

        public PostCmd FindPostCommand(PostRequestType type) => NodesCmd.Find(s => s.Request.PostCmd.Type == type).Request.PostCmd;

        public GetCmd FindGetCommand(GetRequestType type) => NodesCmd.Find(s => s.Request.GetCmd.Type == type).Request.GetCmd;


        private Comendant() { }


        public Comendant Create() => InitComendant();


        private Comendant InitComendant()
        {

            Comendant commendant = new Comendant();

            var commendantPostCreateTree = new ComendantNode("", PostRequestType.CreateTree, GetRequestType.None);

            var commendantPostCreateTreeItem = new ComendantNode("", PostRequestType.CreateTreeItem, GetRequestType.None);

            var commendantPostGetTreeItemsFlatTree = new ComendantNode("", PostRequestType.GetTreeItemsFlatTree, GetRequestType.None);

            var commendantGetParentTreeItems = new ComendantNode("", PostRequestType.None, GetRequestType.GetParentTreeItems);

            var commendantGetChildTreeItems = new ComendantNode("", PostRequestType.None, GetRequestType.GetChildTreeItems);

            var commendantGetTreeItem = new ComendantNode("", PostRequestType.None, GetRequestType.GetTreeItem);
             
            commendant.NodesCmd.Add(commendantPostCreateTree);
            commendant.NodesCmd.Add(commendantPostCreateTreeItem);
            commendant.NodesCmd.Add(commendantPostGetTreeItemsFlatTree);

            commendant.NodesCmd.Add(commendantGetParentTreeItems);
            commendant.NodesCmd.Add(commendantGetChildTreeItems);
            commendant.NodesCmd.Add(commendantGetTreeItem);

            return commendant;
        }

    }


    public class ComendantNode : IDisposable
    {

        public WebRequest Request;
        public WebResponse Response;


        public ComendantNode(string cmdUrl, PostRequestType postType, GetRequestType getType)
        {

            Request = new WebRequest();
            var postHTTP = new PostCmd();
            var getHTTP = new GetCmd();

                      
                postHTTP.Cmd = cmdUrl;
                postHTTP.Type = postType;
            
                getHTTP.Cmd = cmdUrl;
                getHTTP.Type = getType;
             
        }


        public void Dispose()
        {
            
        }


    }
}
