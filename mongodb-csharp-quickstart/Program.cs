using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net.NetworkInformation;

namespace mongodb_csharp_quickstart
{
    class Program
    {
        static void Main(string[] args)
        {
            // Starting();
            // CreatingDocument();
            // ReadOperations();

            UpdatingData();
        }

        static void Starting()
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://bounty:UfQaM1vvL33LKw7x@cluster-c5s-001.lxwl3.mongodb.net/sample_mflix?retryWrites=true&w=majority");

            var dbList = dbClient.ListDatabases().ToList();

            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }
        }

        static void CreatingDocument()
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://bounty:UfQaM1vvL33LKw7x@cluster-c5s-001.lxwl3.mongodb.net/sample_mflix?retryWrites=true&w=majority");

            var database = dbClient.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var document = new BsonDocument { { "student_id", 10000 }, {
                "scores",
                new BsonArray {
                new BsonDocument { { "type", "exam" }, { "score", 88.12334193287023 } },
                new BsonDocument { { "type", "quiz" }, { "score", 74.92381029342834 } },
                new BsonDocument { { "type", "homework" }, { "score", 89.97929384290324 } },
                new BsonDocument { { "type", "homework" }, { "score", 82.12931030513218 } }
                }
                }, { "class_id", 480 }
            };

            collection.InsertOne(document);

            // If you need to do that insert asynchronously, the MongoDB C# driver is fully async compatible. The same operation could be done with:
            // await collection.InsertOneAsync(document);
        }

        static void ReadOperations()
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://bounty:UfQaM1vvL33LKw7x@cluster-c5s-001.lxwl3.mongodb.net/sample_mflix?retryWrites=true&w=majority");

            var database = dbClient.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var firstDocument = collection.Find(new BsonDocument()).FirstOrDefault();
            
            Console.WriteLine(firstDocument.ToString());

            // Reading with a Filter
            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 10000);
            var studentDocument = collection.Find(filter).FirstOrDefault();
            Console.WriteLine(studentDocument.ToString());

            // Reading All Documents
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (BsonDocument doc in documents)
            {
                Console.WriteLine(doc.ToString());
            }

            var highExamScoreFilter = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>(
                                        "scores", new BsonDocument { { "type", "exam" },
                                        { "score", new BsonDocument { { "$gte", 95 } } }
                                        });
            var highExamScores = collection.Find(highExamScoreFilter).ToList();

            var cursor = collection.Find(highExamScoreFilter).ToCursor();
            foreach (var document in cursor.ToEnumerable())
            {
                Console.WriteLine(document);
            }

            // asynchronously 
            // await collection.Find(highExamScoreFilter).ForEachAsync(document => Console.WriteLine(document));
        }

        static void UpdatingData()
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://bounty:UfQaM1vvL33LKw7x@cluster-c5s-001.lxwl3.mongodb.net/sample_mflix?retryWrites=true&w=majority");

            var database = dbClient.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 10000);
            var update = Builders<BsonDocument>.Update.Set("class_id", 483);

            collection.UpdateOne(filter, update);
        }
    }
}
