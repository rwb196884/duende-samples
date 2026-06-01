// Copyright (c) Duende Software. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Duende.IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAutoRendering.Api;

[ApiController]
[Route("api/[controller]")]
public class ThingController : ControllerBase
{
    private readonly ThingService _ThingService;

    public ThingController(ThingService thingService)
    {
        _ThingService = thingService;
    }

    [HttpGet("", Name = nameof(Things))]
    [ProducesResponseType(typeof(IEnumerable<Thing>), StatusCodes.Status200OK)]
    public IActionResult Things()
    {
        return Ok(_ThingService.Things);
    }

    [HttpGet("{thingId:int}", Name = nameof(ThingGetById))]
    [ProducesResponseType(typeof(Thing), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ThingGetById(int thingId)
    {
        Thing? t = _ThingService.Things.SingleOrDefault(z => z.ThingId == thingId);
        if( t == null)
        {
            return NotFound();
        }
        return Ok(t);
    }

    [HttpPost("", Name = nameof(ThingPost))]
    [ProducesResponseType(typeof(Thing), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult ThingPost([FromBody] Thing thing)
    {
        if(thing.ThingId != 0 && _ThingService.Things.Any(z => z.ThingId == thing.ThingId))
        {
            return Conflict();
        }
        _ThingService.Add(thing);
        return CreatedAtAction(nameof(ThingGetById), new { thingId = thing.ThingId }, thing);
    }

    [HttpPut("", Name = nameof(ThingPut))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status304NotModified)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ThingPut([FromBody] Thing thing)
    {
        Thing? t = _ThingService.Things.SingleOrDefault(z => z.ThingId == thing.ThingId);
        if (t == null)
        {
            return NotFound();
        }
        if(thing.ThingName == t.ThingName)
        {
            return Problem(null, null, StatusCodes.Status304NotModified);
        }
        t.ThingName = thing.ThingName;
        return Ok();
    }

    [HttpDelete("{thingId:int}", Name = nameof(ThingDelete))]
    [ProducesResponseType(typeof(Thing), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ThingDelete(int thingId)
    {
        Thing? t = _ThingService.Things.SingleOrDefault(z => z.ThingId == thingId);
        if (t == null)
        {
            return NotFound();
        }
        _ThingService.Delete(thingId);
        return NoContent();
    }

}
