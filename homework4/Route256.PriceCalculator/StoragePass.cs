using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Route256.Domain.Entities;

namespace Route256.PriceCalculator;
    
internal class StoragePass : Domain.Separated.IStorageRepository
{
    public void Save(StorageEntity entity)
    {

    }

    public IReadOnlyList<StorageEntity> Query()
    {
        return new List<StorageEntity>();
    }
}
