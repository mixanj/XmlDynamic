namespace XmlDynamicConsole
{
    using System;
    using System.IO;
    using System.Linq;
    using Domain;

    class Program
    {
        static void Main(string[] args)
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

            Console.ReadKey();
        }
    }
}
