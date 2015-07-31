namespace XmlDynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class Serializer
    {
        private const string XElement_Field = "field";
        private const string XElement_Value = "value";
        private const string XAttribute_Id = "id";
        private const string XAttribute_Name = "name";
        private const string XAttribute_Type = "type";

        /* Construct xml structure to deserialize
         * <construct>
         *	<field id="">
         *		<value></value>
         *	</field>
         *	<field id="">
         *		<value></value>
         *	</field>
         * </construct>
         */
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
            var stringValue = field.Element(XElement_Value).Value;
            var value = Convert.ChangeType(stringValue, fieldType);
            var fieldValue = (FieldValue)Activator.CreateInstance(
                typeof(FieldValue<>).MakeGenericType(new [] { fieldType }),
                id, value);
            return fieldValue;
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
    }
}
