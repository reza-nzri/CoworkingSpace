using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoworkingSpaceAPI.Dtos.Company.Request
{
    public class UpdateCompanyDetailsDto
    {
        public string Name { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string AddressTypeName { get; set; } = string.Empty;
        public string AddressDescription { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        [JsonConverter(typeof(NullableDateOnlyConverter))]
        public DateOnly? FoundedDate { get; set; }
    }
}

public class NullableDateOnlyConverter : JsonConverter<DateOnly?>
{
    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String && DateOnly.TryParse(reader.GetString(), out var date))
        {
            return date;
        }
        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}