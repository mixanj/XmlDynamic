namespace Data
{
    using System.Data.Entity;
    using Domain;

    public class DMContext : DbContext
    {
        public DMContext() : base("name=dm")
        {
        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Factor> Factors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
    }
}
