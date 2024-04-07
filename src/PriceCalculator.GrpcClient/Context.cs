using PriceCalculator.GrpcClient.Interfaces;

namespace PriceCalculator.GrpcClient;

public sealed class Context : IContext
{
    public string GetProjectDirectory()
    {
        return Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? "";
    }
}