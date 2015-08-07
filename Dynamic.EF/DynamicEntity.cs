namespace Dynamic.EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dynamic.Xml;

    public abstract class Construct : Entity
    {
        [Column(TypeName = "xml")]
        public string ConstructData { get; set; }

        [NotMapped]
        public abstract Blueprint Blueprint { get; set; }

        [NotMapped]
        public ICollection<FieldValue> Fields
        {
            get { return Serializer.DeserializeConstruct(ConstructData, Blueprint.Fields); }
            set { ConstructData = Serializer.SerializeConstruct(value); }
        }
    }

    public class Blueprint : Entity
    {
        [Column(TypeName = "xml")]
        public string BlueprintData { get; set; }

        [NotMapped]
        public ICollection<FieldDefinition> Fields
        {
            get { return Serializer.DeserializeBlueprint(BlueprintData); }
            set { BlueprintData = Serializer.SerializeBlueprint(value); }
        }
    }
}
