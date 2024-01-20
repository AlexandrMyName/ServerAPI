using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;


namespace ServerService.Models
{
    [Index(nameof(EntityId))]
    [Index(nameof(ParentEntityId))]
    [Index(nameof(TreeKey))]
    [Index(nameof(MaterializedPath))]
    public class TreeItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid EntityId { get; set; }

        public Guid? ParentEntityId { get; set; }

        [Required]
        public string EntityValue { get; set; }

        [Column(TypeName = "ltree")]
        public LTree MaterializedPath { get; set; }

        public Guid TreeKey { get; set; }

        [NotMapped]
        public int Level { get; set; }
    }
}
