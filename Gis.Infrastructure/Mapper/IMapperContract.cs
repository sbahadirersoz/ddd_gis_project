namespace Gis.Infrastructure.Mapper;

public  interface IMapperContract<TDomain,TDbEntity>
{
    static abstract TDomain ToDomain(TDbEntity dbEntity);
    static abstract TDbEntity ToDbEntity(TDomain domain);
}