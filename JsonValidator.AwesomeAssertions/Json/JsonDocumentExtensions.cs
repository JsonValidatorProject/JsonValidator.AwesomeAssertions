using System.Text.Json;

namespace JsonValidator.AwesomeAssertions.Json;

public static class JsonDocumentExtensions
{
    public static JsonDocumentAssertions Should(this JsonDocument instance) => new(instance);
}
