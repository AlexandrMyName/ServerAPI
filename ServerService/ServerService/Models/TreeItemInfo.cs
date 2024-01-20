using System.ComponentModel.DataAnnotations;
using System;
 

namespace ServerService.Models
{

    public class TreeItemInfo
    {

        [Required]
        public Guid Id { get; set; }

        public Guid?  ParentId { get; set; }

        [Required]
        public string Value { get; set; }
 
    }
}
