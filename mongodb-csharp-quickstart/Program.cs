using System;
using MongoDB.Driver;

namespace mongodb_csharp_quickstart
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://bounty:UfQaM1vvL33LKw7x@cluster-c5s-001.lxwl3.mongodb.net/sample_mflix?retryWrites=true&w=majority");

            var dbList = dbClient.ListDatabases().ToList();

            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }
        }
    }
}
