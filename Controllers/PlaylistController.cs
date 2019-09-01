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
        public async Task<ActionResult<Playlist>> GetPlaylist(Guid id)
        {
            Playlist item = await _context.Playlists.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        //
        // POST: api/Playlist
        [HttpPost]
        public async Task<ActionResult<Playlist>> PostPlayist(Playlist p)
        {
            _context.Playlists.Add(p);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlaylist), new {id = p.ExtId}, p);
        }

        //
        // PUT: api/Playlist/{guid-id}
        [HttpPut]
        public async Task<IActionResult> PutPlaylist(Guid id, Playlist p)
        {
            if (id != p.ExtId)
                return BadRequest();

            _context.Entry(p).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //
        // DELETE: api/Playlist/{guid-id}
        [HttpPut]
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