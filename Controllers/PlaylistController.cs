using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBeep.DataAccess;
using SimpleBeep.Models;
using SimpleBeep.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SimpleBeep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : Controller
    {
        private readonly SimpleBeepContext _context;

        public PlaylistController(SimpleBeepContext context)
        {
            _context = context;
        }

        // 
        // GET: api/Playlist/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
        {
            return await _context.Playlists.ToListAsync();
        }

        // 
        // GET: api/Playlist/{guid-id}/
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistTrackViewModel>> GetPlaylist(Guid id)
        {
            Playlist item = await _context.Playlists.Include(p => p.Tracks).SingleAsync(p => p.Id == id);

            if (item == null)
                return NotFound();

            PlaylistTrackViewModel ptvm = new PlaylistTrackViewModel()
            {
                Name = item.Name,
                Composer = item.Composer,
                Id = id.ToString(),
                Tracks = item.Tracks.Select(t => new TrackViewModel()
                {
                    Name = t.Name,
                    Description = t.Description,
                    Id = t.Id.ToString(),
                    Playlist = id.ToString(),
                }).ToList()
            };

            return ptvm;
        }

        //
        // POST: api/Playlist
        [HttpPost]
        public async Task<ActionResult<PlaylistViewModel>> PostPlayist(PlaylistViewModel pvm)
        {
            Playlist p = new Playlist() {
                Name = pvm.Name,
                Composer = pvm.Composer,
                Id = Guid.NewGuid()
            };

            _context.Playlists.Add(p);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlaylist), new {id = p.Id}, p);
        }

        //
        // PUT: api/Playlist/{guid-id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaylist(Guid id, PlaylistViewModel pvm)
        {
            if (id != Guid.Parse(pvm.Id))
                return BadRequest();

            Playlist p = _context.Playlists.Find(id);
            p.Name = pvm.Name;
            p.Composer = pvm.Composer;

            _context.Entry(p).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //
        // DELETE: api/Playlist/{guid-id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(Guid id)
        {
            Playlist item = await _context.Playlists.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Playlists.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}