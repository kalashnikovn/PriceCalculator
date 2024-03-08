using PriceCalculator.Api.Dal.Entities;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace PriceCalculator.Api.Dal.Repositories;

public class GoodsRepository : IGoodsRepository
{
    private readonly Dictionary<int, GoodEntity> _store = new ();
    
    public void AddOrUpdate(GoodEntity entity)
    {
        if (_store.ContainsKey(entity.Id))
            _store.Remove(entity.Id);
        
        _store.Add(entity.Id, entity);
    }

    public ICollection<GoodEntity> GetAll()
    {
        return _store.Select(x => x.Value).ToArray();
    }

    public GoodEntity Get(int id)
    {
        return _store[id];
    }
}