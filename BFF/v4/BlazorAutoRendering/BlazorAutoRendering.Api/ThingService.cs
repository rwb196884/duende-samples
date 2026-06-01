// Copyright (c) Duende Software. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace BlazorAutoRendering.Api;

public class Thing
{
    public int ThingId { get; set; }
    public string ThingName { get; set; }
}

public class ThingService
{
    private readonly Dictionary<int, Thing> _Things;
    private readonly object _Lock;

    public ThingService()
    {
        _Lock = new object();
        lock (_Lock)
        {
            _Things = new Dictionary<int, Thing>() {
            { 1, new Thing()
                {
                    ThingId = 1,
                    ThingName = "The very first Thing"
                }
            }
        };
        }
    }

    public IEnumerable<Thing> Things { get { return _Things.Values; } }

    public Thing Add(Thing thing)
    {
        lock (_Lock)
        {
            int id = _Things.Select(z => z.Value.ThingId).DefaultIfEmpty(0).Max();
            id++;
            thing.ThingId = id;
            _Things.Add(id, thing);
        }
        return thing;
    }

    public void Delete(int thingId)
    {
        lock (_Lock)
        {
            _Things.Remove(thingId);
        }
    }
}
