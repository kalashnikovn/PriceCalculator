using PriceCalculator.Api.Bll.Models.Analytics;
using PriceCalculator.Api.Bll.Models.PriceCalculator;

namespace PriceCalculator.Api.Bll.Services.Interfaces;

public interface IPriceCalculatorService
{
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods);
    CalculationLogModel[] QueryLog(int take);
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods, decimal distance);
    void DeleteHistory();
    ReportModel GetReport();
}