namespace XmlDynamicConsole
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Domain;
    using XmlDynamic;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test Reading Blueprints and Constructs From Xml File:");
            TestReadingBCFromXmlFile();

            Console.WriteLine("Test Creating Blueprints and Constructs From Code:");
            TestCreatingBCFromCode();

            Console.ReadKey();
        }

        static void TestReadingBCFromXmlFile()
        {
            var blueprint = File.ReadAllText("blueprint.xml");
            var construct = File.ReadAllText("construct.xml");

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Category 1",
                BlueprintData = blueprint
            };
            var factor = new Factor
            {
                Id = Guid.NewGuid(),
                Name = "Factor 1",
                Category = category,
                ConstructData = construct
            };

            var customFactorFields = factor.Fields;

            var definition = factor.Category.Fields;
            foreach (var field in factor.Fields)
            {
                Console.WriteLine("{0} = {1}", definition.First(f => f.Id == field.Id).Name, field.GetValue());
            }
        }

        static void TestCreatingBCFromCode()
        {
            var fd1 = new FieldDefinition { Id = new Guid("00000000-0000-0000-0000-000000000001"), Name = "Val1", FieldType = typeof(int) };
            var fd2 = new FieldDefinition { Id = new Guid("00000000-0000-0000-0000-000000000002"), Name = "Val2", FieldType = typeof(string) };
            
            var blueprint = new List<FieldDefinition>(2);
            blueprint.AddRange(new [] { fd1, fd2 });

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Category 1",
                Fields = blueprint
            };

            Console.WriteLine(category.BlueprintData);

            var f1 = new FieldValue<int> (new Guid("00000000-0000-0000-0000-000000000001"),  100);
            var f2 = new FieldValue<string>(new Guid("00000000-0000-0000-0000-000000000002"), "Ahoj");

            var construct = new List<FieldValue>(2);
            construct.AddRange(new FieldValue [] { f1, f2 });

            var factor = new Factor
            {
                Id = Guid.NewGuid(),
                Name = "Factor 1",
                Category = category,
                Fields = construct
            };

            Console.WriteLine(factor.ConstructData);
        }
    }
}
