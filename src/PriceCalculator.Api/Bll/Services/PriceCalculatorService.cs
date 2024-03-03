using PriceCalculator.Api.Bll.Models;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Entities;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace PriceCalculator.Api.Bll.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private readonly IStorageRepository _storageRepository;
    
    private const double _volumeRatio = 3.27d;

    public PriceCalculatorService(
        IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }
    
    public double CalculatePrice(GoodModel[] goods)
    {
        var volume = goods
            .Sum(x => x.Height * x.Length * x.Width);

        var volumePrice = volume * _volumeRatio;
        
        _storageRepository.Save(new StorageEntity(
            volume,
            volumePrice,
            DateTime.UtcNow
            ));

        return volumePrice;

    }

    public CalculationLogModel[] QueryLog(int take)
    {
        var log = _storageRepository.Query()
            .OrderByDescending(x => x.At)
            .Take(take)
            .ToArray();

        return log
            .Select(x => new CalculationLogModel(
                x.Volume, 
                x.Price))
            .ToArray();
    }
}