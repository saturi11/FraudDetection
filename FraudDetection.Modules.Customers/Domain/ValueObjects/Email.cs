using FraudDetection.BuildingBlocks.Domain;
using System.Text.RegularExpressions;

namespace FraudDetection.Modules.Customers.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty.");

        if (!IsValid(value))
            throw new ArgumentException("Invalid email format.");

        return new Email(value.ToLowerInvariant());
    }

    private static bool IsValid(string email)
    {
        return Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
