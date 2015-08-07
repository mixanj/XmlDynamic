namespace StreamingFromSqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Xml;

    class Program
    {
        // Replace the connection string if needed, for instance to connect to SQL Express: @"Server=(local)\SQLEXPRESS;Database=Demo;Integrated Security=true"
        private const string connectionString = @"Server=.\SQLEXPRESS;Database=XmlDynamicModel;Integrated Security=true";

        static void Main(string[] args)
        {
            PrintXmlValues().Wait();

            Console.WriteLine("Done");
        }

        // Application transferring a large Xml Document from SQL Server in .NET 4.5
        private static async Task PrintXmlValues()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT [id], [ConstructData] FROM [Factors]", connection))
                {

                    // The reader needs to be executed with the SequentialAccess behavior to enable network streaming
                    // Otherwise ReadAsync will buffer the entire Xml Document into memory which can cause scalability issues or even OutOfMemoryExceptions
                    using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine("{0}: ", reader.GetGuid(0));

                            if (await reader.IsDBNullAsync(1))
                            {
                                Console.WriteLine("\t(NULL)");
                            }
                            else
                            {
                                using (XmlReader xmlReader = reader.GetXmlReader(1))
                                {
                                    int depth = 1;
                                    // NOTE: The XmlReader returned by GetXmlReader does NOT support async operations
                                    // See the example below (PrintXmlValuesViaNVarChar) for how to get an XmlReader with asynchronous capabilities
                                    while (xmlReader.Read())
                                    {
                                        switch (xmlReader.NodeType)
                                        {
                                            case XmlNodeType.Element:
                                                Console.WriteLine("{0}<{1}>", new string('\t', depth), xmlReader.Name);
                                                depth++;
                                                break;
                                            case XmlNodeType.Text:
                                                Console.WriteLine("{0}{1}", new string('\t', depth), xmlReader.Value);
                                                break;
                                            case XmlNodeType.EndElement:
                                                depth--;
                                                Console.WriteLine("{0}</{1}>", new string('\t', depth), xmlReader.Name);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}