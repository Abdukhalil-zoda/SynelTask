using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using CsvHelper.TypeConversion;

namespace SynelTask.Helpers;
public class CustomDateTimeConverter : ITypeConverter
{

    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (DateTime.TryParse(text, out var date))
        {
            return date;
        }
        throw new TypeConverterException(this, memberMapData, text, row.Context, $"Invalid date format: '{text}'");
    }

    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        return ((DateTime)value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    }
}
