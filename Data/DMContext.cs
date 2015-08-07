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

    //public static class CustomFieldsQueryExtensions
    //{
    //    public static ICollection<Factor> WhereDm(this DbSet<Factors> factors, Predicate<Queue<Factor>> path)
    //    {
    //        //predicate will be array of <field id, type, condition>
    //        // eg [<000-00-001, int, (x > 10 & x < 30)>]
    //        // have a store proc that will return factors (with Includes virtual props) that fulfill this condition using XQuery
    //    }
    //}
}
