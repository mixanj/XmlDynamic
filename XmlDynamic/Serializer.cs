namespace Dynamic.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    public class Serializer
    {
        private const string XElement_Field = "field";
        private const string XAttribute_Id = "id";
        private const string XAttribute_Name = "name";
        private const string XAttribute_Type = "type";
        private const string XElement_Construct = "construct";
        private const string XElement_Blueprint = "blueprint";

        /* Construct xml structure to deserialize
         * <construct>
         *	<field id="">100</field>
         *	<field id="">Hello</field>
         * </construct>
         */
        public static ICollection<FieldValue> DeserializeConstruct(XmlReader constructData, ICollection<FieldDefinition> fieldDefs)
        {
            var fields = new List<FieldValue>(fieldDefs.Count);
            var construct = new XDocument(constructData).Elements(XElement_Field);
            fields.AddRange(construct.Select(xmlField => DeserializeField(xmlField, fieldDefs)));
            return fields;
        }
        
        public static ICollection<FieldValue> DeserializeConstruct(string constructData, ICollection<FieldDefinition> fieldDefs)
        {
            var fields = new List<FieldValue>(fieldDefs.Count);
            var construct = XElement.Parse(constructData).Elements(XElement_Field);
            fields.AddRange(construct.Select(xmlField => DeserializeField(xmlField, fieldDefs)));
            return fields;
        }

        private static FieldValue DeserializeField(XElement field, IEnumerable<FieldDefinition> fieldDefs)
        {
            var id = Guid.Parse(field.Attribute(XAttribute_Id).Value);
            var fieldType = fieldDefs.Single(fd => fd.Id == id).FieldType;
            var value = Convert.ChangeType(field.Value, fieldType);
            var fieldValue = (FieldValue)Activator.CreateInstance(
                typeof(FieldValue<>).MakeGenericType(new [] { fieldType }),
                id, value);
            return fieldValue;
        }

        public static string SerializeConstruct(ICollection<FieldValue> fields)
        {
            var construct = new XElement(XElement_Construct);
            foreach (var field in fields)
            {
                var node = new XElement(XElement_Field, new XAttribute(XAttribute_Id, field.Id));
                node.Value = field.GetValue().ToString();
                construct.Add(node);
            }
            return construct.ToString();
        }

        /* Blueprint xml structure to deserialize
         * <blueprint>
         *	<field id="" name="" type="System.Int32">
         *		<metadata></metadata>
         *	</field>
         *	<field id="" name="" type="System.String">
         *		<metadata></metadata>
         *	</field>
         * </blueprint>
         */
        public static ICollection<FieldDefinition> DeserializeBlueprint(XmlReader blueprintData)
        {
            var fields = new List<FieldDefinition>();
            var blueprint = new XDocument(blueprintData).Elements(XElement_Field);
            fields.AddRange(blueprint.Select(DeserializeFieldDef));
            return fields;
        }

        public static ICollection<FieldDefinition> DeserializeBlueprint(string blueprintData)
        {
            var fields = new List<FieldDefinition>();
            var blueprint = XElement.Parse(blueprintData).Elements(XElement_Field);
            fields.AddRange(blueprint.Select(DeserializeFieldDef));
            return fields;
        }

        private static FieldDefinition DeserializeFieldDef(XElement field)
        {
            var id = Guid.Parse(field.Attribute(XAttribute_Id).Value);
            var name = field.Attribute(XAttribute_Name).Value;
            var typeNme = field.Attribute(XAttribute_Type).Value;
            return new FieldDefinition
            {
                Id = id,
                Name = name,
                FieldType = Type.GetType(typeNme)
            };
        }

        public static string SerializeBlueprint(ICollection<FieldDefinition> fields)
        {
            var blueprint = new XElement(XElement_Blueprint);
            foreach (var field in fields)
            {
                var node = new XElement(XElement_Field,
                    new XAttribute(XAttribute_Id, field.Id),
                    new XAttribute(XAttribute_Name, field.Name),
                    new XAttribute(XAttribute_Type, field.FieldType.ToString()));
                
                // add metadata
                blueprint.Add(node);
            }
            return blueprint.ToString();
        }
    }
}
