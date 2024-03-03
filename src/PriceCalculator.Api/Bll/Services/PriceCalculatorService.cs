using PriceCalculator.Api.Bll.Models;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Entities;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace PriceCalculator.Api.Bll.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private readonly IStorageRepository _storageRepository;
    
    private const double _volumeRatio = 3.27d;
    private const double _weightRatio = 1.34d;

    public PriceCalculatorService(
        IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }
    
    public double CalculatePrice(GoodModel[] goods)
    {
        if (!goods.Any()) throw new ArgumentException("Список не может быть пустым!");
        
        var volumePrice = CalculatePriceByVolume(goods, out var volume);

        var weightPrice = CalculatePriceByWeight(goods, out var weight);

        var resultPrice = Math.Max(volumePrice, weightPrice);
        
        _storageRepository.Save(new StorageEntity(
            volume,
            resultPrice,
            DateTime.UtcNow,
            weight
            ));

        return resultPrice;

    }

    private static double CalculatePriceByVolume(GoodModel[] goods, out double volume)
    {
        volume = goods
            .Sum(x => x.Height * x.Length * x.Width);

        return volume * _volumeRatio;
    }
    
    private static double CalculatePriceByWeight(GoodModel[] goods, out double weight)
    {
        weight = goods.Sum(x => x.Weight);

        return weight * _volumeRatio;
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
                x.Weight))
            .ToArray();
    }
}