using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using PriceCalculator.Api.Controllers;
using PriceCalculator.Api.Requests.V2;
using PriceCalculator.Api.Requests.V3;
using PriceCalculator.Domain.Exceptions;
using PriceCalculator.Domain.Models.PriceCalculator;
using Xunit.Abstractions;
using CalculateRequest = PriceCalculator.Api.Requests.V3.CalculateRequest;

namespace PriceCalculator.Domain.IntegrationTests;

public class BasicTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BasicTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task App_SwaggerIsWorking()
    {
        // Arrange
        var app = new AppFixture();
        var httpClient = app.CreateClient();
        
        // Act
        var response = await httpClient.GetAsync("/swagger/index.html");
        
        // Assert
        response.EnsureSuccessStatusCode();
        _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task App_V3DeliveryPrice_ShouldThrow()
    {
        // Arrange
        var app = new AppFixture();
        var calculateRequest = new GoodCalculateRequest(0, 0);
        var scope = app.Services.CreateScope();
        var controller = scope.ServiceProvider.GetRequiredService<V3DeliveryPriceController>();

        // Act, Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.Calculate(calculateRequest));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 5)]
    [InlineData(4, 7)]
    [InlineData(5, 9)]
    public async void App_V3DeliveryPrice_ShouldSuccess(int goodId, decimal distance)
    {
        // Arrange
        var app = new AppFixture();
        var calculateRequest = new GoodCalculateRequest(goodId, distance);
        var scope = app.Services.CreateScope();
        var controller = scope.ServiceProvider.GetRequiredService<V3DeliveryPriceController>();
        
        
        // Act
        var result = await controller.Calculate(calculateRequest);


        // Assert
        Assert.True(result.Price > 0);
    }
}