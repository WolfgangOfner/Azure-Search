using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace SearchDemo
{
    [SerializePropertyNamesAsCamelCase]
    public class Car
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string Id { get; set; }

        [IsSearchable]
        public string Brand { get; set; }
        
        [IsSearchable, IsFilterable, IsSortable, IsFacetable]
        public string Type { get; set; }

        [IsFilterable, IsSortable, IsFacetable]
        public int HorsePower { get; set; }
    }
}