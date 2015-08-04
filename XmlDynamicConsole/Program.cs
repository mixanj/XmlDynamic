namespace XmlDynamicConsole
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Data;
    using Domain;
    using XmlDynamic;

    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Test Reading Blueprints and Constructs From Xml File:");
            //TestReadingBCFromXmlFile();

            //Console.WriteLine("Test Creating Blueprints and Constructs From Code:");
            //TestCreatingBCFromCode();

            Console.WriteLine("Test Read/Write with EF:");
            TestReadWriteWithEF();

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

            PrintFactor(factor);
        }

        static void PrintFactor(Factor factor)
        {
            Console.WriteLine("Factor {0}", factor.Name);
            var definition = factor.Category.Fields;
            foreach (var field in factor.Fields)
            {
                Console.WriteLine("{0} = {1}", definition.First(f => f.Id == field.Id).Name, field.GetValue());
            }
        }

        static void TestCreatingBCFromCode()
        {
            var category = CreateCategory();
            Console.WriteLine(category.BlueprintData);

            var factor = CreateFactor(category, 1);
            Console.WriteLine(factor.ConstructData);
        }

        static Category CreateCategory()
        {
            var fd1 = new FieldDefinition { Id = new Guid("00000000-0000-0000-0000-000000000001"), Name = "Val1", FieldType = typeof(int) };
            var fd2 = new FieldDefinition { Id = new Guid("00000000-0000-0000-0000-000000000002"), Name = "Val2", FieldType = typeof(string) };
            var fd3 = new FieldDefinition { Id = new Guid("00000000-0000-0000-0000-000000000003"), Name = "Val3", FieldType = typeof(long) };

            var blueprint = new List<FieldDefinition>(3);
            blueprint.AddRange(new[] { fd1, fd2, fd3 });

            return new Category
            {
                //Id = Guid.NewGuid(),
                Name = "Category 1",
                Fields = blueprint
            };
        }

        static Factor CreateFactor(Category category, int iterator)
        {
            var f1 = new FieldValue<int>(new Guid("00000000-0000-0000-0000-000000000001"), iterator);
            var f2 = new FieldValue<string>(new Guid("00000000-0000-0000-0000-000000000002"), "Ahoj " + iterator);
            var f3 = new FieldValue<long>(new Guid("00000000-0000-0000-0000-000000000003"), 1000000000L + iterator);

            var construct = new List<FieldValue>(3);
            construct.AddRange(new FieldValue[] { f1, f2, f3 });

            return new Factor
            {
                //Id = Guid.NewGuid(),
                Name = "Factor " + iterator,
                Category = category,
                Fields = construct
            };
        }

        static void CleanupDB()
        {
            using (var context = new DMContext())
            {
                context.Database.ExecuteSqlCommand("delete from Factors");
                context.Database.ExecuteSqlCommand("delete from Categories");
                context.SaveChanges();
            }
        }

        static void TestReadWriteWithEF()
        {
            const int numberOfFactors = 1000;
            CleanupDB();
            WriteToDB(numberOfFactors);
            ReadFromDB();
            QueryDB();
        }

        static void WriteToDB(int numberOfFactors)
        {
            var stopWatch = new Stopwatch();
            var category = CreateCategory();
            var factors = new List<Factor>(numberOfFactors);
            for (int iterator = 1; iterator < numberOfFactors; iterator++)
            {
                factors.Add(CreateFactor(category, iterator));
            }
            
            stopWatch.Start();
            using (var context = new DMContext())
            {
                context.Categories.Add(category);
                context.Factors.AddRange(factors);
                context.SaveChanges();
            }
            stopWatch.Stop();

            Console.WriteLine("Writing of {0} factors to DB took {1} ms", numberOfFactors, stopWatch.ElapsedMilliseconds);
        }

        static void ReadFromDB()
        {
            var stopWatch = new Stopwatch();
            List<Factor> factors;
            stopWatch.Start();
            using (var context = new DMContext())
            {
                factors = context.Factors.Include("Category").ToList();
            }
            stopWatch.Stop();
            var numberOfFactors = factors.Count;
            Console.WriteLine("Reading of {0} factors from DB took {1} ms", numberOfFactors, stopWatch.ElapsedMilliseconds);
            //factors.Take(10).ToList().ForEach(PrintFactor);
        }

        static void QueryDB()
        {
            var stopWatch = new Stopwatch();
            List<Factor> factors;
            stopWatch.Start();
            using (var context = new DMContext())
            {
                var fieldId = context.Factors.First().Category.Fields.First(f => f.Name == "Val1").Id;
                factors = context.Factors
                    .Include("Category")
                    .ToList() // Because with xml deserialization we cant do Linq to entity
                    .Where(f => f.Fields.Any(field => field.Id == fieldId && (field as FieldValue<int>).Value < 10))
                    .ToList();
            }
            stopWatch.Stop();
            var numberOfFactors = factors.Count;
            Console.WriteLine("Linq query of {0} factors took {1} ms", numberOfFactors, stopWatch.ElapsedMilliseconds);
            //factors.ForEach(PrintFactor);
        }
    }
}
