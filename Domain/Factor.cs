namespace Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dynamic.EF;
    
    public class Factor : Construct
    {
        [MaxLength(50)]
        public string Name { get; set; }
        
        public virtual Category Category { get; set; }

        [NotMapped]
        public override Blueprint Blueprint
        {
            get { return Category; }
            set { Category = (Category)value; }
        }
    }
}
