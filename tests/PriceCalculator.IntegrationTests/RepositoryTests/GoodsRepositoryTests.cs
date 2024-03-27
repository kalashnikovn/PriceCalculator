using System.Runtime.InteropServices;
using FluentAssertions;
using PriceCalculator.Dal.Repositories.Interfaces;
using PriceCalculator.IntegrationTests.Fixtures;
using TestingInfrastructure.Creators;
using TestingInfrastructure.Fakers;
using Xunit;
using Xunit.Abstractions;

namespace PriceCalculator.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class GoodsRepositoryTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly double _requiredDoublePrecision = 0.00001d;

    private readonly IGoodsRepository _goodsRepository;

    public GoodsRepositoryTests(
        TestFixture fixture,
        ITestOutputHelper testOutputHelper
    
    )
    {
        _testOutputHelper = testOutputHelper;
        _goodsRepository = fixture.GoodsRepository;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public async Task Add_Goods_Success(int count)
    {
        // Arrange
        var userId = Create.RandomId();
        
        var goods = GoodEntityV1Faker.Generate(count)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        // Act
        var goodIds = await _goodsRepository.Add(goods, default);

        // Asserts
        goodIds.Should().HaveCount(count);
        goodIds.Should().OnlyContain(x => x > 0);
    }
    
    [Fact]
    public async Task Query_GoodsByUserId_Success()
    {
        // Arrange
        var userId = Create.RandomId();
        
        var goods = GoodEntityV1Faker.Generate(10)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        var goodIds = (await _goodsRepository.Add(goods, default))
            .ToHashSet();

        // Act
        var foundGoods = await _goodsRepository.Query(userId, default);

        // Assert
        foundGoods.Should().NotBeEmpty();
        foundGoods.Should().OnlyContain(x => x.UserId == userId);
        foundGoods.Should().OnlyContain(x => goodIds.Contains(x.Id));
    }
    
    [Fact]
    public async Task Query_SingleGoodByUserId_Success()
    {
        // Arrange
        var userId = Create.RandomId();
        
        var goods = GoodEntityV1Faker.Generate()
            .Select(x => x.WithUserId(userId))
            .ToArray();
        var expected = goods.Single();

        var goodId = (await _goodsRepository.Add(goods, default))
            .Single();

        // Act
        var foundGoods = await _goodsRepository.Query(userId, default);

        // Assert
        foundGoods.Should().HaveCount(1);
        var good = foundGoods.Single();

        good.Id.Should().Be(goodId);
        good.UserId.Should().Be(expected.UserId);
        good.Height.Should().BeApproximately(expected.Height, _requiredDoublePrecision);
        good.Width.Should().BeApproximately(expected.Width, _requiredDoublePrecision);
        good.Length.Should().BeApproximately(expected.Length, _requiredDoublePrecision);
        good.Weight.Should().BeApproximately(expected.Weight, _requiredDoublePrecision);
    }
    
    [Fact]
    public async Task Query_GoodsForWrongUser_ReturnsEmpty()
    {
        // Arrange
        var userId = Create.RandomId();
        var anotherUserId = Create.RandomId();
        
        var goods = GoodEntityV1Faker.Generate(10)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        await _goodsRepository.Add(goods, default);

        // Act
        var foundGoods = await _goodsRepository.Query(anotherUserId, default);

        // Assert
        foundGoods.Should().BeEmpty();
    }

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    public async Task Remove_GoodsByIds_Success(int count)
    {
        // Arrange
        var userId = Create.RandomId();
        var goods = GoodEntityV1Faker.Generate(count)
            .Select(x => x.WithUserId(userId))
            .ToArray();
        
        var ids = await _goodsRepository.Add(goods, default);
        
        // Act
        var rowsAffected = await _goodsRepository.Remove(ids, default);
        var foundGoods = await _goodsRepository.Query(userId, default);

        // Assert
        _testOutputHelper.WriteLine($"Rows affected: {rowsAffected}");
        Assert.True(rowsAffected > 0);
        rowsAffected.Should().Be(goods.Length);
        foundGoods.Should().BeEmpty();

    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Remove_GoodsByUserId_Success(int count)
    {
        // Arrange
        var userId = Create.RandomId();
        var goods = GoodEntityV1Faker.Generate(count)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        var goodIds = await _goodsRepository.Add(goods, default);

        // Act
        var rowsAffected = await _goodsRepository.Remove(userId, default);
        var foundGoods = await _goodsRepository.Query(userId, default);
        
        // Assert
        _testOutputHelper.WriteLine($"Rows affected: {rowsAffected}");
        Assert.True(rowsAffected > 0);
        rowsAffected.Should().Be(goodIds.Length);
        foundGoods.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Remove_UserIdSpecified_ShouldRemoveAllGoodsOnlyForSpecifiedUser()
    {
        // Arrange
        var userId = Create.RandomId();
        var anotherUserId = Create.RandomId();
        
        var specifiedUserGoods = GoodEntityV1Faker.Generate(10)
            .Select(x => x.WithUserId(userId))
            .ToArray();
        
        var anotherUserGoods = GoodEntityV1Faker.Generate(10)
            .Select(x => x.WithUserId(anotherUserId))
            .ToArray();
        
        // Act
        await _goodsRepository.Add(specifiedUserGoods, default);
        var anotherUserGoodIds = await _goodsRepository.Add(anotherUserGoods, default);

        await _goodsRepository.Remove(userId, default);
        
        var foundSpecifiedUserGoods = await _goodsRepository.Query(userId, default);
        var foundAnotherUserGoods = await _goodsRepository.Query(anotherUserId, default);
        
        // Assert
        foundSpecifiedUserGoods.Should().BeEmpty();
        foundAnotherUserGoods.Should().OnlyContain(x => anotherUserGoodIds.Contains(x.Id));
    }
    
    [Fact]
    public async Task Remove_GoodsIdsSpecified_ShouldRemoveOnlySpecifiedGoods()
    {
        // Arrange
        var userId = Create.RandomId();
        
        var goods = GoodEntityV1Faker.Generate(35)
            .Select(x => x.WithUserId(userId))
            .ToArray();
        
        // Act
        var goodsIds = await _goodsRepository.Add(goods, default);
        var goodsIdsForDelete = goodsIds.Skip(7).ToArray();
        var remainingGoodsIds = goodsIds.Except(goodsIdsForDelete).ToArray();
        
        await _goodsRepository.Remove(goodsIdsForDelete, default);
        
        var foundGoods = await _goodsRepository.Query(userId, default);
        var foundGoodsIds = foundGoods.Select(x => x.Id).ToArray();
        
        // Assert
        foundGoodsIds.Should().BeEquivalentTo(remainingGoodsIds);
    }
}
