using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBeep.DataAccess;
using SimpleBeep.Models;
using SimpleBeep.ViewModels;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SimpleBeep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : Controller
    {
        private readonly SimpleBeepContext _context;

        public TrackController(SimpleBeepContext context)
        {
            _context = context;
        }

        // 
        // GET: api/Track/
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Track>>> GetTracks()
        // {
        //     return await _context.Tracks.ToListAsync();
        // }

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
        public async Task<ActionResult<Track>> PostPlayist(TrackViewModel tvm)
        {
            Track t = new Track() {
                Name = tvm.Name.Trim(),
                Description = tvm.Description.Trim(),
                Id = Guid.NewGuid(),
                Playlist = _context.Playlists.Find(Guid.Parse(tvm.Playlist))
            };

            _context.Tracks.Add(t);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrack), new {id = t.Id}, t);
        }

        //
        // PUT: api/Track/{guid-id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrack(Guid id, TrackViewModel tvm)
        {
            if (id != Guid.Parse(tvm.Id))
                return BadRequest();

            Track t = _context.Tracks.Find(id);
            t.Name = tvm.Name;
            t.Description = tvm.Description;
            t.PlaylistId = Guid.Parse(tvm.Playlist);

            _context.Entry(t).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //
        // DELETE: api/Track/{guid-id}
        [HttpDelete("{id}")]
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