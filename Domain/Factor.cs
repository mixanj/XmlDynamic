namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using XmlDynamic;

    public class Factor
    {
        public Factor()
        {
            CreatedOn = DateTimeOffset.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "xml")]
        public string ConstructData { get; set; }

        [NotMapped]
        public ICollection<FieldValue> Fields
        {
            get { return Serializer.DeserializeConstruct(ConstructData, Category.Fields); }
        }

        public virtual Category Category { get; set; }
    }
}
