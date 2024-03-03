using PriceCalculator.Api.Bll.Models;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Entities;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace PriceCalculator.Api.Bll.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private readonly IStorageRepository _storageRepository;
    
    // Коэффициент для объема за 1 куб. метр
    private const decimal _volumeRatio = 3.27m * 1_000_000;
    
    // Коэффициент для веса за 1кг
    private const decimal _weightRatio = 1.34m ;

    public PriceCalculatorService(
        IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
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

    private static decimal CalculatePriceByVolume(IReadOnlyList<GoodModel> goods, out decimal volume)
    {
        volume = goods
            .Sum(x => x.Height * x.Length * x.Width);

        return volume * _volumeRatio;
    }
    
    private static decimal CalculatePriceByWeight(IReadOnlyList<GoodModel> goods, out decimal weight)
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
}