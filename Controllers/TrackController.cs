using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBeep.DataAccess;
using SimpleBeep.Models;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SimpleBeep.Controllers
{
    [Route("api/[controller]")]
    public class TrackController : Controller
    {
        private readonly SimpleBeepContext _context;

        public TrackController(SimpleBeepContext context)
        {
            _context = context;
        }

        // 
        // GET: api/Track/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Track>>> GetTracks()
        {
            return await _context.Tracks.ToListAsync();
        }

        // 
        // GET: api/Track/{guid-id}/
        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetTrack(Guid id)
        {
            Track item = await _context.Tracks.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        //
        // POST: api/Track
        [HttpPost]
        public async Task<ActionResult<Track>> PostPlayist(Track t)
        {
            _context.Tracks.Add(t);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrack), new {id = t.ExtId}, t);
        }

        //
        // PUT: api/Track/{guid-id}
        [HttpPut]
        public async Task<IActionResult> PutTrack(Guid id, Track t)
        {
            if (id != t.ExtId)
                return BadRequest();

            _context.Entry(t).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //
        // DELETE: api/Track/{guid-id}
        [HttpPut]
        public async Task<IActionResult> DeleteTrack(Guid id)
        {
            Track item = await _context.Tracks.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Tracks.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}