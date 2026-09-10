using gis.Domain.Aggregates;
using gis.InfrastructureLayer.Entity;

namespace gis.InfrastructureLayer.Mapper;

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