using Microsoft.Extensions.Options;
using PriceCalculator.Api.Bll.Models.Analytics;
using PriceCalculator.Api.Bll.Models.PriceCalculator;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Entities;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace PriceCalculator.Api.Bll.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private readonly IStorageRepository _storageRepository;
    
    // Коэффициент для объема за 1 куб. метр
    private decimal _volumeRatio;
    
    // Коэффициент для веса за 1кг
    private decimal _weightRatio;

    public PriceCalculatorService(
        IOptionsSnapshot<PriceCalculatorOptions> options,
        IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
        _volumeRatio = options.Value.VolumeToPriceRatio * 1_000_000;
        _weightRatio = options.Value.WeightToPriceRatio;
    }
    
    /// <summary>
    /// Метод, поддерживающий обратную совместимость с V2 и V1. Возвращает цену за 1км
    /// </summary>
    /// <param name="goods">Список входных данных</param>
    /// <returns>Цена</returns>
    public decimal CalculatePrice(IReadOnlyList<GoodModel> goods)
    {
        // 1000 метров = 1 км
        return CalculatePrice(goods, 1000);
    }

    private decimal CalculatePriceByVolume(IReadOnlyList<GoodModel> goods, out decimal volume)
    {
        volume = goods
            .Sum(x => x.Height * x.Length * x.Width);

        return volume * _volumeRatio;
    }
    
    private decimal CalculatePriceByWeight(IReadOnlyList<GoodModel> goods, out decimal weight)
    {
        weight = goods.Sum(x => x.Weight);

        return weight * _weightRatio;
    }

    public CalculationLogModel[] QueryLog(int take)
    {
        if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take), take, "Take должно быть больше 0");
        
        var log = _storageRepository.Query()
            .OrderByDescending(x => x.At)
            .Take(take)
            .ToArray();

        return log
            .Select(x => new CalculationLogModel(
                x.Volume, 
                x.Price,
                x.Weight,
                x.Distance))
            .ToArray();
    }

    public decimal CalculatePrice(IReadOnlyList<GoodModel> goods, decimal distance)
    {
        if (!goods.Any()) throw new ArgumentException("Список не может быть пустым!");
        if (distance <= 0) throw new ArgumentOutOfRangeException(nameof(distance), distance, "Растояние должно быть больше 0");
        
        var volumePrice = CalculatePriceByVolume(goods, out var volume);
        var weightPrice = CalculatePriceByWeight(goods, out var weight);
        
        var resultPrice = Math.Max(volumePrice, weightPrice) * (distance / 1000);
        
        _storageRepository.Save(new StorageEntity(
            volume,
            resultPrice,
            DateTime.UtcNow,
            weight,
            distance
        ));

        return resultPrice;
    }

    public void DeleteHistory()
    {
        _storageRepository.Clear();
    }


    public ReportModel GetReport()
    {
        var storage = _storageRepository.Query().ToArray();

        var maxWeight = GetMaxWeight(storage);
        var maxVolume = GetMaxVolume(storage);

        var maxDistanceForHeaviestGood = GetMaxDistanceForHeaviestGood(storage, maxWeight);
        var maxDistanceForLargestGood = GetMaxDistanceForLargestGood(storage, maxVolume);

        var wavgPrice = GetWavgPrice(storage);
        
        return new ReportModel(
            maxWeight,
            maxVolume,
            maxDistanceForHeaviestGood,
            maxDistanceForLargestGood,
            wavgPrice
            );
    }
    
    private static decimal GetMaxWeight(StorageEntity[] goods)
    {
        return goods.Max(x => x.Weight);
    }
    
    private static decimal GetMaxVolume(StorageEntity[] goods)
    {
        return goods.Max(x => x.Volume);
    }

    private static decimal GetMaxDistanceForHeaviestGood(StorageEntity[] goods, decimal maxWeight)
    {
        var distancesForHeaviestGood = goods
            .Where(x => x.Weight == maxWeight)
            .Select(x => x.Distance);

        return distancesForHeaviestGood.Max();
    }

    private static decimal GetMaxDistanceForLargestGood(StorageEntity[] goods, decimal maxVolume)
    {
        var distancesForLargestGood = goods
            .Where(x => x.Volume == maxVolume)
            .Select(x => x.Distance);

        return distancesForLargestGood.Max();
    }

    private static decimal GetWavgPrice(StorageEntity[] goods)
    {
        var fullPrice = goods.Sum(x => x.Price);
        return fullPrice / goods.Length;
    }
    
}