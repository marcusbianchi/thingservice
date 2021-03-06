using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using securityfilter;
using thingservice.Data;
using thingservice.Model;

namespace thingservice.Controllers {
    [Route ("api/[controller]")]
    public class ThingsController : Controller {
        private readonly ApplicationDbContext _context;

        public ThingsController (ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        [SecurityFilter ("things__allow_read")]
        [ResponseCache (CacheProfileName = "thingscache")]
        public async Task<IActionResult> Get ([FromQuery] int startat, [FromQuery] int quantity) {

            if (quantity == 0)
                quantity = 50;
            var things = await _context.Things.Where (x => x.enabled == true).OrderBy (x => x.thingId).Skip (startat).Take (quantity).ToListAsync ();
            return Ok (things);
        }

        [HttpGet ("list/")]
        [SecurityFilter ("things__allow_read")]
        public async Task<IActionResult> GetList ([FromQuery] int[] thingId) {
            var things = await _context.Things
                .Where (x => thingId.Contains (x.thingId))
                .ToListAsync ();
            return Ok (things);
        }

        [HttpGet ("{id}")]
        [SecurityFilter ("things__allow_read")]
        [ResponseCache (CacheProfileName = "thingscache")]
        public async Task<IActionResult> Get (int id) {
            var thing = await _context.Things
                .OrderBy (x => x.thingId)
                .Where (x => x.thingId == id)
                .FirstOrDefaultAsync ();;
            if (thing == null)
                return NotFound ();

            return Ok (thing);
        }

        [HttpPost]
        [SecurityFilter ("things__allow_update")]
        public async Task<IActionResult> Post ([FromBody] Thing thing) {
            thing.thingId = 0;
            thing.parentThingId = 0;
            thing.childrenThingsIds = new int[0];
            thing.physicalConnection = thing.physicalConnection != null ? thing.physicalConnection.ToLower () : null;
            if (ModelState.IsValid) {
                await _context.AddAsync (thing);
                await _context.SaveChangesAsync ();

                return Created ($"api/things/{thing.thingId}", thing);
            }
            return BadRequest (ModelState);
        }

        [HttpPut ("{id}")]
        [SecurityFilter ("things__allow_update")]
        public async Task<IActionResult> Put (int id, [FromBody] Thing thing) {
            if (ModelState.IsValid) {
                var curThing = await _context.Things.AsNoTracking ().Where (x => x.thingId == id).FirstOrDefaultAsync ();
                thing.childrenThingsIds = curThing.childrenThingsIds;
                thing.physicalConnection = thing.physicalConnection != null ? thing.physicalConnection.ToLower () : null;
                thing.parentThingId = curThing.parentThingId;
                if (id != thing.thingId) {
                    return NotFound ();
                }
                _context.Things.Update (thing);
                await _context.SaveChangesAsync ();
                return NoContent ();

            }
            return BadRequest (ModelState);
        }

        [HttpDelete ("{id}")]
        [SecurityFilter ("things__allow_update")]
        public async Task<IActionResult> Delete (int id) {
            var thing = await _context.Things.Where (x => x.thingId == id).FirstOrDefaultAsync ();
            if (thing != null) {
                thing.enabled = false;
                _context.Entry (thing).State = EntityState.Modified;
                await _context.SaveChangesAsync ();
                return NoContent ();
            }
            return NotFound ();
        }
    }

}