using System;
using System.Net.Http;
using AwesomeAssertions;
using AwesomeAssertions.Execution;
using AwesomeAssertions.Primitives;

namespace JsonValidator.AwesomeAssertions.HttpResponse;

public static class ObjectAssertionsExtensions
{
    /// <summary>
    /// Checks whether the HTTP response's body as a JSON string matches the input object in both structure and values.
    /// </summary>
    /// <param name="instance">The <see cref="ObjectAssertions"/></param>
    /// <param name="expected">The expected value as an anonymous object</param>
    /// <param name="because">The reason the two values should match</param>
    /// <param name="becauseArgs">The parameters for the reason argument</param>
    public static AndConstraint<ObjectAssertions> HaveJsonBody(
        this ObjectAssertions instance,
        object expected,
        string because = "",
        params object[] becauseArgs)
    {
        if (instance.Subject is not HttpResponseMessage responseMessage)
        {
            throw new InvalidOperationException("This method should only be used on HttpResponseMessage objects.");
        }

        var isMatch = System.Text.Json.JsonDocument
            .Parse(responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult())
            .TryValidateMatch(expected, out var errors);

        AssertionChain
            .GetOrCreate()
            .BecauseOf(because, becauseArgs)
            .ForCondition(isMatch)
            .FailWith("Expected response body to match the JSON input, but found differences {0}.", errors);

        return new AndConstraint<ObjectAssertions>(instance);
    }
}
