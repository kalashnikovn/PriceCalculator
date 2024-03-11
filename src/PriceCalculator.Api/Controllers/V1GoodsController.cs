using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.Bll.Models.PriceCalculator;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Entities;
using PriceCalculator.Api.Dal.Repositories.Interfaces;
using PriceCalculator.Api.Responses.V2;

namespace PriceCalculator.Api.Controllers;

[ApiController]
[Route("goods")]
public class V1GoodsController : Controller
{
    private readonly IGoodsRepository _repository;

    public V1GoodsController(
        IGoodsRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public ICollection<GoodEntity> GetAll()
    {
        return _repository.GetAll();
    }
    
    [HttpGet("calculate/{id}")]
    public CalculateResponse Calculate(
        [FromServices] IPriceCalculatorService priceCalculatorService,
        int id)
    {
        var good = _repository.Get(id);
        var model = new GoodModel(
            good.Height,
            good.Length,
            good.Width,
            good.Weight);
        
        var price = priceCalculatorService.CalculatePrice(new []{ model });
        return new CalculateResponse(price);
    }
}