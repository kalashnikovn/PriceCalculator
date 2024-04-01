namespace PriceCalculator.ProducerApp.Interfaces;

public interface IModelFaker<T>
{
    List<T> GenerateMany(int count);
}