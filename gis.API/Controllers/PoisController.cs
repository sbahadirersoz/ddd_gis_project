using gis.API.EndPoints;
using gis.ApplicationLayer.Features.Poi.Commands;
using gis.ApplicationLayer.Features.Poi.Commands.Create;
using gis.ApplicationLayer.Features.Poi.Commands.Delete;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.FindInRangePoisByName;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.FindNearestCountPoiByName;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.FindNearestPoiByName;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.Id.FindInRangePoisById;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindClosestPoiByLonLat;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindInRangePoisByLonLat;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.LonLat.FindMostClosesCountPoiByLonLat;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindInRangePoisByWkt;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindMostClosesCountPoiByWkt;
using gis.ApplicationLayer.Features.Poi.Queries.Distance.Wkt.FindNearestPoiByWkt;
using gis.ApplicationLayer.Features.Poi.Queries.FindNearestPoi;
using gis.ApplicationLayer.Features.Poi.Queries.FindPoiById;
using gis.ApplicationLayer.Features.Poi.Queries.FindPoisInGivenRange;
using gis.ApplicationLayer.Features.Poi.Queries.GetAllPois;
using gis.Domain.Entities.IDs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace gis.API.Controllers
{
    [ApiController]
    [Route(Endpoints.Pois.Root)]
    public class PoisController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PoisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Crud Methods
        

        [HttpPost(Endpoints.Pois.Create)]
        [ProducesResponseType(typeof(CreatePoiCommandResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreatePoiCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Value.Id.Value }, // result.Value içindeki Guid Id alanı
                result.Value);
        }

        [HttpGet(Endpoints.Pois.GetById)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var fromGuid = PointID.FromGuid(id);
            var command = new FindPoiByIdQuery(fromGuid.Value);
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete(Endpoints.Pois.Delete)]
        public async Task<IActionResult> DeleteById([FromRoute]Guid id,CancellationToken cancellationToken)
        {
            var pointId = PointID.FromGuid(id);
            if (!pointId.IsFailure)
            {
                var comm = new DeletePoiCommand(pointId.Value);
                var result  = await _mediator.Send(comm, cancellationToken);
                if (result.IsFailure) return BadRequest(result.Error);
                return Ok(result.Value);
            }

            return BadRequest(pointId.Error);
        }

        [HttpPatch(Endpoints.Pois.Update)]
        public async Task<IActionResult> Update( [FromBody] UpdatePoiCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsFailure)return BadRequest(result.Error);
            return Ok(result.Value);
            }
            
            [HttpGet(Endpoints.Pois.List)]
            public async Task<IActionResult> GetAll( )
            {
                var query = new GetAllPoisQuery();
                var result = await _mediator.Send(query);
                return (result.IsFailure) ? BadRequest(result.Error) : Ok(result.Value);
            }
            
            

        #endregion

        #region  Distance Calc
        

        #region Id Based Endpoints
        


        [HttpGet(Endpoints.Pois.inRange)]
        public async Task<IActionResult> FindInRangePoints([FromQuery] FindInRangePoisByIdQuery query)
        {
            
            
            var result = await _mediator.Send(query);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
        [HttpGet(Endpoints.Pois.MostClosesByCountID)]
        public async Task<IActionResult> FindClosesCountById([FromQuery] FindMostClosesCountByIdQuery query)
        {
            
            var result = await _mediator.Send(query);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
        [HttpGet(Endpoints.Pois.FindClosestById)]
        public async Task<IActionResult> FindClosestById([FromQuery] FindClosestByIdQuery query)
        {
            if (query.id  == Guid.Empty) return BadRequest("Invalid ID");
            var result = await _mediator.Send(query);
            return (result.IsFailure)?  BadRequest(result.Error):Ok(result.Value);
        }
        

        #endregion
        

        #region  Name Based Endpoints
        
        [HttpGet(Endpoints.Pois.inRangeByName)]
        public async Task<IActionResult> FindInRangeByName([FromQuery] FindInRangePoisByNameQuery query)
        {
            var result = await _mediator.Send(query);
            return (result.IsFailure) ?  BadRequest(result.Error) : Ok(result.Value);
        }
        [HttpGet(Endpoints.Pois.MostClosesByCountName)]
        public async Task<IActionResult> FindMostClosesByName([FromQuery] FindMostClosesCountByNameQuery query)
        {
            var result = await _mediator.Send(query);
            return (result.IsFailure) ?  BadRequest(result.Error) : Ok(result.Value);
        }
        [HttpGet(Endpoints.Pois.FindClosestByName)]
        public async Task<IActionResult> FindClosest([FromQuery] FindClosestPoiByNameQuery query)
        {
            var result = await _mediator.Send(query);
            return (result.IsFailure) ?  BadRequest(result.Error) : Ok(result.Value);
        }
        #endregion

        #region WKT BASED Endpoints

            [HttpPost(Endpoints.Pois.FindClosestByWkt)]
            public async Task<IActionResult> FindClosestByWkt([FromBody] FindClosestPoiByWktQuery query)
            {
                var result = await _mediator.Send(query);
                return (result.IsFailure)?  BadRequest(result.Error):Ok(result.Value);
            }
            [HttpPost(Endpoints.Pois.MostClosesByCountWkt)]
            public async Task<IActionResult> FindMostClosesCountByWkt([FromBody] FindMostClosesCountPoiByWktQuery query)
            {
                var result = await _mediator.Send(query);
                return (result.IsFailure)?  BadRequest(result.Error):Ok(result.Value);
            }

            [HttpPost(Endpoints.Pois.inRangeByWkt)]
            public async Task<IActionResult> FindInRangeByWkt([FromBody] FindInRangePoisByWktQuery query)
            {
                var result = await _mediator.Send(query);
                return (result.IsFailure)?  BadRequest(result.Error):Ok(result.Value);
            }

            

        #endregion
        

        #region  Lan Lot 
        
        [HttpGet(Endpoints.Pois.FindClosestByLatLon)]
        public async Task<IActionResult> FindClosestByLonLat([FromQuery] FindClosestPoiByLonLatQuery query)
        {
            var result = await _mediator.Send(query);
            return (result.IsFailure)?  BadRequest(result.Error):Ok(result.Value);
        }
        [HttpGet(Endpoints.Pois.MostClosesByCountLatLon)]
        public async Task<IActionResult> FindMostClosesCountByLonLat([FromQuery] FindMostClosesCountPoiByLonLatQuery query)
        {
            var result = await _mediator.Send(query);
            return (result.IsFailure)?  BadRequest(result.Error):Ok(result.Value);
        }

        [HttpGet(Endpoints.Pois.inRangeByLatLon)]
        public async Task<IActionResult> FindInRangeByLonLat([FromQuery] FindInRangePoisByLonLatQuery query)
        {
            var result = await _mediator.Send(query);
            return (result.IsFailure)?  BadRequest(result.Error):Ok(result.Value);
        }



        

        #endregion
        
        #endregion
        
    }



}