﻿using FluentValidation;
using PriceCalculator.Domain.Entities;
using PriceCalculator.Domain.Exceptions;
using PriceCalculator.Domain.Models.PriceCalculator;
using PriceCalculator.Domain.Separated;
using PriceCalculator.Domain.Services.Interfaces;
using PriceCalculator.Domain.Validators;

namespace PriceCalculator.Domain.Services;

internal class PriceCalculatorService : IPriceCalculatorService
{
    private readonly decimal _volumeToPriceRatio;
    private readonly decimal _weightToPriceRatio;
    private const decimal DistanceToPriceRatio = 1.0m;
    
    private readonly IStorageRepository _storageRepository;
    

    public PriceCalculatorService(
        PriceCalculatorOptions options,
        IStorageRepository storageRepository)
    {
        _weightToPriceRatio = options.WeightToPriceRatio;
        _volumeToPriceRatio = options.VolumeToPriceRatio;
        _storageRepository = storageRepository;
    }

    public decimal CalculatePrice(IReadOnlyList<GoodModel> goods)
    {
        try
        {
            return CalculatePriceUnsafe(goods);
        }
        catch (ValidationException e)
        {
            throw new DomainException("Incorrect input", e);
        }
        catch (DivideByZeroException e)
        {
            throw new DomainException("Incorrect input", e);
        }
    }
    
    public decimal CalculatePrice(CalculateRequestModel request)
    {
        try
        {
            return CalculatePriceUnsafe(request);
        }
        catch (ValidationException e)
        {
            throw new DomainException("Incorrect input", e);
        }
        catch (DivideByZeroException e)
        {
            throw new DomainException("Incorrect input", e);
        }
    }
    
    private decimal CalculatePriceUnsafe(IReadOnlyList<GoodModel> goods)
    {
        var validator = new GoodsValidator();
        validator.ValidateAndThrow(goods);

        var volumePrice = CalculatePriceByVolume(goods, out var volume);
        var weightPrice = CalculatePriceByWeight(goods, out var weight);

        var resultPrice = Math.Max(volumePrice, weightPrice);
        
        _storageRepository.Save(new StorageEntity(
            DateTime.UtcNow,
            volume,
            weight,
            0,
            resultPrice));

        return resultPrice;
    }
    
    private decimal CalculatePriceUnsafe(CalculateRequestModel request)
    {
        var goods = request.Goods;

        var validator = new GoodsValidator();
        validator.ValidateAndThrow(goods);

        var volumePrice = CalculatePriceByVolume(goods, out var volume);
        var weightPrice = CalculatePriceByWeight(goods, out var weight);

        var resultPrice = ApplyDistanceToPrice(Math.Max(volumePrice, weightPrice), request.Distance);

        _storageRepository.Save(new StorageEntity(
            DateTime.UtcNow,
            volume,
            weight,
            request.Distance,
            resultPrice));
        
        return resultPrice;
    }
    
    private decimal CalculatePriceByVolume(
        IReadOnlyList<GoodModel> goods,
        out decimal volume)
    {
        volume = goods
            .Select(x => x.Height * x.Width * x.Length / 1000)
            .Sum();

        return volume * _volumeToPriceRatio;
    }

    private decimal CalculatePriceByWeight(
        IReadOnlyList<GoodModel> goods,
        out decimal weight)
    {
        weight = goods
            .Select(x => x.Weight / 1000)
            .Sum();

        return weight * _weightToPriceRatio;
    }
    

    public CalculationLogModel[] QueryLog(int take)
    {
        if (take == 0)
        {
            return Array.Empty<CalculationLogModel>();
        }

        var log = _storageRepository.Query()
            .OrderByDescending(x => x.At)
            .Take(take)
            .ToArray();

        return log
            .Select(x => new CalculationLogModel(
                x.Volume,
                x.Weight,
                x.Distance,
                x.Price))
            .ToArray();
    }

    private static decimal ApplyDistanceToPrice(decimal price, decimal distance)
    {
        if (distance == 0)
            return price;

        return price * distance * DistanceToPriceRatio;
    }
    
}