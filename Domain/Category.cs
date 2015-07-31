namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using XmlDynamic;

    public class Category
    {
        public Category()
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
        public string BlueprintData { get; set; }

        [NotMapped]
        public ICollection<FieldDefinition> Fields
        {
            get { return Serializer.DeserializeBlueprint(BlueprintData); }
        }
    }
}
