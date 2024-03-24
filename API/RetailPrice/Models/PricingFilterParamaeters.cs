using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetailPrice.Models
{
    public class GridParameters
    {
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public List<ColumnModel>? RowGroupCols { get; set; }
        private string _filterModelJson;

        [JsonPropertyName("FilterModel")]
        public string FilterModelJson
        {
            get => _filterModelJson;
            set
            {
                _filterModelJson = value;

                if (!string.IsNullOrEmpty(_filterModelJson)) {
                    FilterModel = DeserializeFilterModel(value);
                }
                else
                {
                    FilterModel = new List<FilterModel>();
                }

            }
        }

         private List<FilterModel> DeserializeFilterModel(string json)
    {
        if (string.IsNullOrEmpty(json))
            return null;

        try
        {
            // First, unescape the JSON string
            string unescapedJson = System.Text.RegularExpressions.Regex.Unescape(json);

            // Trim the leading and trailing quotes from the unescaped string
            unescapedJson = unescapedJson.Trim('"');

            // Then, deserialize the JSON string as usual
            return JsonSerializer.Deserialize<List<FilterModel>>(unescapedJson);
        }
        catch (JsonException ex)
        {
            // Handle or log the exception as needed
            Console.WriteLine($"JSON deserialization error: {ex.Message}");
            return new List<FilterModel>();
        }
    }

        [JsonIgnore]
        public List<FilterModel>? FilterModel { get; set; }=null;

       
        public SortModel? SortModel { get; set; }
   
    }


    public class PagedResult<T>
    {
        public List<T> Items { get; set; } // The items for the current page
        public int TotalCount { get; set; } // Total count of items across all pages

        public PagedResult(List<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;           
        }
    }

    public class ColumnModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Field { get; set; }
        public string Type { get; set; } // Can be used for determining aggregation type etc.
    }

    public class FilterModel
    {
        public string filterType { get; set; }
        public string colId { get; set; }
        public string type { get; set; }
        public dynamic? filter { get; set; }


    }

    public class Condition
    {
        public string FilterType { get; set; } // e.g., "text", "number"
        public string ColId { get; set; }
        public string Type { get; set; } // e.g., "contains", "lessThan"
        public string Filter { get; set; }

        public string FilterTo { get; set; }
    }


    public class SortModel
    {
        public string Sort { get; set; } // "asc" or "desc"
        public string ColId { get; set; }
    }

}
