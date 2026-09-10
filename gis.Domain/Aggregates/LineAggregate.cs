using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using gis.Domain.BusinessRules.LineRules;
using gis.Domain.BusinessRules.PointRules;
using gis.Domain.Common;
using gis.Domain.Entities.Coord;
using gis.Domain.Entities.Events;
using gis.Domain.Entities.Events.LineEvents;
using gis.Domain.Entities.IDs;
using gis.Domain.Entities.Information;
using gis.Domain.Entities.Information.Line;
using gis.Domain.Entities.WKT;
using gis.Domain.ResultPattern;
using gis.Domain.ResultPattern.Errors;

namespace gis.Domain.Aggregates;

public class LineAggregate:AggregateRoot<LineID>
{
    public List<CoordinateValueObject> Coordinates  { get;  private set; }
    public LineName LineName  { get; private set; }
    public LineDescription? LineDescription  { get; private set; }
    public LineStatus LineStatus { get; private set; } = LineStatus.ACTIVE;
    public WellKnownText WKT { get; private set; }
    

    private LineAggregate(LineID id, LineName lineName, List<CoordinateValueObject> coordinates, LineDescription? lineDescription) : base(id)
    {
        LineName = lineName;
        Coordinates = coordinates;
        LineDescription = lineDescription ??  LineDescription.FromString(null).Value;
    }

    internal LineAggregate(LineID id, List<CoordinateValueObject> coordinates, LineName lineName, LineDescription? lineDescription, LineStatus lineStatus, WellKnownText wkt) : base(id)
    {
        Coordinates = coordinates;
        LineName = lineName;
        LineDescription = lineDescription;
        LineStatus = lineStatus;
        WKT = wkt;
    }

    public static LineAggregate Reconstitute(LineID id, LineName lineName, List<CoordinateValueObject> coordinates,
        LineDescription? lineDescription,WellKnownText wkt ,LineStatus status)
    {
        return new LineAggregate(id,  coordinates,lineName, lineDescription,status,wkt);
    }

    internal static Result<LineAggregate> Create(List<CoordinateValueObject> coordinates, LineName lineName,LineDescription? lineDescription,WellKnownText wkt)
    {
        var lineId = LineID.New();
        var agg = new LineAggregate(lineId,coordinates,lineName,lineDescription,LineStatus.ACTIVE,wkt);
        agg.AddDomainEvent(LineCreatedEvent.Create(lineId));
        return Result<LineAggregate>.Success(agg);
    }    
    internal static Result<LineAggregate> CreateWithID(LineID id, List<CoordinateValueObject> coordinates, LineName lineName,LineDescription? lineDescription,WellKnownText wkt)
    {
        var agg = new LineAggregate(id,coordinates,lineName,lineDescription,LineStatus.ACTIVE,wkt);
        agg.AddDomainEvent(LineCreatedEvent.Create(id));
        return Result<LineAggregate>.Success(agg);
    }




    #region ForUpdateHelperMethods [Without domainEventAdding  ]

    internal Result UpdateLineName(LineName lineName)
    {
        var checkRuleValid = CheckRuleValid(new LineNameMustNotTheSame(lineName,this));
        if (checkRuleValid.IsFailure) return Result.Failure(checkRuleValid.Error);
        LineName =  lineName;
        return Result.Success();
        
    }
    
    internal void UpdateDesc(LineDescription description)
    {
        LineDescription= description;
    }
    internal Result UpdateStatus(LineStatus status)
    {
        var checkRuleValid = CheckRuleValid(new LineMustNotBeSameStatus( status,this));
        if (checkRuleValid.IsFailure)  return Result.Failure(checkRuleValid.Error);
        LineStatus = status;
        return Result.Success();
    }
    
    internal void UpdateCoordinates(List<CoordinateValueObject> coordinates)
    {
        Coordinates = coordinates;
        
    }
    internal void UpdateWKT(WellKnownText wkt)
    {
        WKT = wkt;
    }
    
    

    #endregion

    #region ChangesWithAddingEvent

    internal Result ChangeLineName(LineName lineName)
    {
        var updateLineName = UpdateLineName(lineName);
        if (updateLineName.IsFailure) return Result.Failure(updateLineName.Error);
        
        var previousLineName = LineName;
        AddDomainEvent(LineNameChangedEvent.Create(LineName,previousLineName,Id.Value));
        return Result.Success();
    }
    
    internal void ChangeDesc(LineDescription description)
    {
        var prev = LineDescription;
        UpdateDesc(description);
        AddDomainEvent(LineDescChangedEvent.Create(LineDescription,prev,Id.Value));
    }
    internal Result ChangeStatus(LineStatus status)
    {
        var ruleset = CheckRuleValid(new LineMustNotBeSameStatus(status,this));
        if (ruleset.IsFailure) return Result.Failure(ruleset.Error);
        
        var prev =  LineStatus;
        UpdateStatus(status);
        AddDomainEvent(LineStatusChangedEvent.Create(LineStatus,prev,Id.Value));
        
        return Result.Success();
    }
    internal Result SoftDelete()
    {
        var valid = CheckRuleValid(new LineMustNotBeDeletedRule(this));
        if (valid.IsFailure) return  Result.Failure(valid.Error);
        LineStatus= LineStatus.SOFT_DELETED;
        AddDomainEvent(LineSoftDeletedEvent.Create(Id.Value));
        return Result.Success();
        
    }
    
    internal void ChangeCoordinates(List<CoordinateValueObject> coordinates ,WellKnownText wkt)
    {
        Coordinates = coordinates;
        WKT = wkt;
        AddDomainEvent(LineCoordinatesChangedEvent.Create(Id.Value));
        
    }
    
    

    #endregion

    private Result CheckRuleValid(ILineRule rule)
    {
        var execute = rule.Execute();
        return execute.IsFailure ? Result.Failure(execute.Error) : Result.Success();
    }
    
}