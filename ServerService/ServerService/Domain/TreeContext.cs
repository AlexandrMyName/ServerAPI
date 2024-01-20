using Microsoft.EntityFrameworkCore;
using ServerService.Models;
 

namespace ServerService.Domain
{

    public class TreeContext : DbContext
    {

        public TreeContext(DbContextOptions<TreeContext> options) : base(options) { }
        public DbSet<Tree> Trees { get; set; }
        public DbSet<TreeItem> TreeItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasPostgresExtension("ltree");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TreeItem>()
                .HasIndex(treeItem => treeItem.MaterializedPath)
              .HasMethod("gist");
        }
    }
}
