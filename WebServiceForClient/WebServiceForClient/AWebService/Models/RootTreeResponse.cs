using System;


namespace WebServiceForClient.AWebService.Models
{

    [Serializable]
    public record class RootTreeResponse
    {

        public Guid EntityId { get; set; }

        public Guid? ParentEntityId { get; set; }

        public string EntityValue { get; set; }

        public string Path { get; set; }

        public int? Level { get; set; }
    }
}
