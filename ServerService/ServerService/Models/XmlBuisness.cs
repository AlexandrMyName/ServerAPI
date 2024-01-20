using System;


namespace ServerService.Models
{

    [Serializable]
    public class XmlBuisness
    {

        public Guid Id { get; set; }
        public Guid? Parrent { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
