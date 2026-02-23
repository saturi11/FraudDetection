using FraudDetection.BuildingBlocks.Domain;

namespace FraudDetection.Modules.Customers.Domain.ValueObjects;

public sealed class Country : ValueObject
{
    public string Code { get; }

    private Country(string code)
    {
        Code = code;
    }

    public static Country Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Country code cannot be empty.");

        if (code.Length != 2)
            throw new ArgumentException("Country code must be ISO 2 letters.");

        return new Country(code.ToUpperInvariant());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
