using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace SearchDemo
{
    public class Program
    {
        private static void Main(string[] args)
        {
            const string searchServiceName = "YourServiceName";
            const string accesskey = "YourAccessKey";
            var serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(accesskey));

            DeleteIndexIfExists(serviceClient);

            CreateIndex(serviceClient);

            var cars = CreateCars();

            var indexClient = serviceClient.Indexes.GetClient("cars");
            var batch = IndexBatch.Upload(cars);

            indexClient.Documents.Index(batch);

            // necessary, otherwise no search result are found
            Thread.Sleep(1000);

            var searchResults = Search(indexClient);

            PrintSearchResult(searchResults);
        }

        private static void DeleteIndexIfExists(SearchServiceClient serviceClient)
        {
            if (serviceClient.Indexes.Exists("cars"))
            {
                serviceClient.Indexes.Delete("cars");
            }
        }

        private static void CreateIndex(SearchServiceClient serviceClient)
        {
            var definition = new Index
            {
                Name = "cars",
                Fields = FieldBuilder.BuildForType<Car>()
            };

            serviceClient.Indexes.Create(definition);
        }

        private static IEnumerable<Car> CreateCars()
        {
            var cars = new[]
            {
                new Car
                {
                    Id = "1",
                    Brand = "Ferrari",
                    Type = "F458",
                    HorsePower = 552
                },
                new Car
                {
                    Id = "2",
                    Brand = "Tesla",
                    Type = "S",
                    HorsePower = 770
                },
                new Car
                {
                    Id = "3",
                    Brand = "Tesla",
                    Type = "3",
                    HorsePower = 258
                }
            };

            return cars;
        }

        private static DocumentSearchResult<Car> Search(ISearchIndexClient indexClient)
        {
            var parameters =
                new SearchParameters
                {
                    // add all properties, which should be returned from the search
                    Select = new[] {"type", "brand", "id", "horsePower"}
                };

            return indexClient.Documents.Search<Car>("Tesla", parameters);
        }

        private static void PrintSearchResult(DocumentSearchResult<Car> searchResults)
        {
            foreach (var result in searchResults.Results)
            {
                Console.WriteLine($"The following car types were found {result.Document.Type}");
            }
        }
    }
}