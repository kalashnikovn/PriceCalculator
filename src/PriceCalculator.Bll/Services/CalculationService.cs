using PriceCalculator.Bll.Models;
using PriceCalculator.Bll.Services.Interfaces;
using PriceCalculator.Dal.Entities;
using PriceCalculator.Dal.Models;
using PriceCalculator.Dal.Repositories.Interfaces;

namespace PriceCalculator.Bll.Services;

public class CalculationService : ICalculationService
{
    public const decimal VolumeToPriceRatio = 3.27m;
    public const decimal WeightToPriceRatio = 1.37m;
    
    private readonly ICalculationsRepository _calculationsRepository;
    private readonly IGoodsRepository _goodsRepository;

    public CalculationService(
        ICalculationsRepository calculationsRepository,
        IGoodsRepository goodsRepository)
    {
        _calculationsRepository = calculationsRepository;
        _goodsRepository = goodsRepository;
    }
    
    public decimal CalculatePriceByVolume(GoodModel[] goods, out double volume)
    {
        volume = goods
            .Sum(x => x.Height * x.Length * x.Width);

        return (decimal)volume * VolumeToPriceRatio;
    }

    public decimal CalculatePriceByWeight(GoodModel[] goods, out double weight)
    {
        weight = goods
            .Sum(x => x.Weight);

        return (decimal)weight * WeightToPriceRatio;
    }

    public async Task<long> SaveCalculation(SaveCalculationModel data, CancellationToken cancellationToken)
    {
        var goods = data.Goods
            .Select(x => new GoodEntityV1
            {
                UserId = data.UserId,
                Height = x.Height,
                Weight = x.Weight,
                Length = x.Length,
                Width = x.Width
            })
            .ToArray();

        var calculation = new CalculationEntityV1
        {
            UserId = data.UserId,
            Price = data.Price,
            TotalVolume = data.TotalVolume,
            TotalWeight = data.TotalWeight,
            At = DateTimeOffset.UtcNow
        };

        using var transaction = _calculationsRepository.CreateTransactionScope();
        var goodsId = await _goodsRepository.Add(goods, cancellationToken);

        calculation = calculation with { GoodsId = goodsId };
        var calculationIds = await _calculationsRepository.Add(new[] { calculation }, cancellationToken);
        transaction.Complete();

        return calculationIds.Single();

    }

    public async Task<QueryCalculationModel[]> QueryCalculations(QueryCalculationFilter query, CancellationToken token)
    {
        var queryModel = new GetHistoryQueryModel(
            UserId: query.UserId,
            Limit: query.Limit,
            Offset: query.Offset
        );

        var calculationEntities = await _calculationsRepository.Query(queryModel, token);

        return calculationEntities
            .Select(x => new QueryCalculationModel(
                Id: x.Id,
                UserId: x.UserId,
                TotalVolume: x.TotalVolume,
                TotalWeight: x.TotalWeight,
                Price: x.Price,
                GoodIds: x.GoodsId
                ))
            .ToArray();
    }

    public async Task<int> RemoveCalculations(QueryCalculationModel[] calculations, CancellationToken cancellationToken)
    {
        var calculationIds = calculations
            .Select(x => x.Id)
            .ToArray();
        
        var goodIds = calculations
            .Select(x => x.GoodIds)
            .ToArray()
            .SelectMany(x => x)
            .ToArray();

        using var transaction = _calculationsRepository.CreateTransactionScope();
        var rowsAffected = await _calculationsRepository.Remove(calculationIds, cancellationToken);
        await _goodsRepository.Remove(goodIds, cancellationToken);
        transaction.Complete();

        return rowsAffected;


    }
}