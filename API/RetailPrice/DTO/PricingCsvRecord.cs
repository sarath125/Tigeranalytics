using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace RetailPrice.DTO
{
    public class PricingCsvRecord
    {
        public int StoreID { get; set; }
        public int SKU { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }


    public sealed class PricingCsvRecordMap : ClassMap<PricingCsvRecord>
    {
        public PricingCsvRecordMap()
        {
            Map(m => m.StoreID).TypeConverter<NullableInt32Converter>();
            Map(m => m.SKU).TypeConverter<NullableInt32Converter>();
            Map(m => m.Price).TypeConverter<NullableDecimalConverter>();
            Map(m => m.Date).Name("Date").TypeConverterOption.Format("dd-MM-yyyy"); // Keep as string
        }
    }

    public class NullableInt32Converter : Int32Converter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default(int?); // Return null for nullable int
            }

            return base.ConvertFromString(text, row, memberMapData);
        }
    }


    public class NullableDecimalConverter : DecimalConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default(decimal?); // Return null for nullable decimal
            }

            return base.ConvertFromString(text, row, memberMapData);
        }
    }

}
