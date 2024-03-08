using System.Net;
using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.Bll.Models.PriceCalculator;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Entities;
using PriceCalculator.Api.Dal.Repositories.Interfaces;
using PriceCalculator.Api.Responses.V2;

namespace PriceCalculator.Api.Controllers.V1;

[ApiController]
[Route("goods")]
public class GoodsController : Controller
{
    private readonly IGoodsFullPriceService _goodsFullPriceService;
    private readonly IGoodsRepository _repository;
    private readonly ILogger<GoodsController> _logger;

    public GoodsController(
        IGoodsFullPriceService goodsFullPriceService,
        IGoodsRepository repository,
        ILogger<GoodsController> logger)
    {
        _goodsFullPriceService = goodsFullPriceService;
        _repository = repository;
        _logger = logger;
    }
    
    [HttpGet("all")]
    public ICollection<GoodEntity> GetAll()
    {
        return _repository.GetAll();
    }
    
    [HttpGet("calculate/{id}")]
    public CalculateResponse Calculate(
        [FromServices] IPriceCalculatorService priceCalculatorService,
        int id)
    {
        _logger.LogInformation(HttpContext.Request.Path);
        
        var good = _repository.Get(id);
        var model = new GoodModel(
            good.Height,
            good.Length,
            good.Width,
            good.Weight);
        
        var price = priceCalculatorService.CalculatePrice(new []{ model });
        return new CalculateResponse(price);
    }
    
    [HttpPost("calculate-full/{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public IActionResult CalculateFullPrice(
        int id
    )
    {
        var fullPrice = _goodsFullPriceService.GetFullPrice(id);
        return Ok(new
        {
            Price = fullPrice,
            Id = id
        });

    }
    
}