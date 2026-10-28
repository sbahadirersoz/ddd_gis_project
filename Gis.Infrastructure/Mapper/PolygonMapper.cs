using gis.Domain.Aggregates;
using Gis.Infrastructure.Entity;

namespace Gis.Infrastructure.Mapper;

public class PolygonMapper:IMapperContract<PolygonAggregate,PolygonEntity>
{
    public static PolygonAggregate ToDomain(PolygonEntity dbEntity)
    {
        throw new NotImplementedException();
    }

    public static PolygonEntity ToDbEntity(PolygonAggregate domain)
    {
        throw new NotImplementedException();
    }
    
}