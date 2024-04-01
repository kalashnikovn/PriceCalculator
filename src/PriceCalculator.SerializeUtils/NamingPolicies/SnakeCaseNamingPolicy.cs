using System.Text.Json;
using PriceCalculator.SerializeUtils.Extensions;

namespace PriceCalculator.SerializeUtils.NamingPolicies;

public sealed class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) 
        => name.ToSnakeCase();
}