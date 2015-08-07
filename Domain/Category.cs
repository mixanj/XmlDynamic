namespace Domain
{
    using System.ComponentModel.DataAnnotations;
    using Dynamic.EF;

    public class Category : Blueprint
    {
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
