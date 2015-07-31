namespace XmlDynamicConsole
{
    using System;
    using System.IO;
    using Domain;
    using XmlDynamic;

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

            Console.ReadKey();
        }
    }
}
