namespace Dynamic.Xml
{
    using System;
    using System.Collections.Generic;

    public class FieldDefinition
    {
        public FieldDefinition()
        {
            Metadata = new List<Metadata>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Type FieldType { get; set; }
        public ICollection<Metadata> Metadata { get; set; }
    }
}
