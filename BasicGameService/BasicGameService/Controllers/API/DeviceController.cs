using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicGameService.Data;
using BasicGameService.DTOs;
using BasicGameService.Models;

namespace BasicGameService.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DevicesController(AppDbContext db) => _db = db;

        // GET: /api/devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> Get()
        {
            var devices = await _db.Devices
                .Include(d => d.InstalledGames)
                .Include(d => d.CurrentSession)
                .ToListAsync();

            var dtos = devices.Select(d => new DeviceDto
            {
                Id = d.Id,
                Name = d.Name,
                Type = d.Type.ToString(),
                Description = d.Description,
                IsAvailable = d.IsAvailable,
                CurrentSessionId = d.CurrentSession?.Id
            });

            return Ok(dtos);
        }

        // GET: /api/devices/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DeviceDto>> Get(int id)
        {
            var d = await _db.Devices
                .Include(x => x.InstalledGames)
                .Include(x => x.CurrentSession)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (d == null) return NotFound();

            var dto = new DeviceDto
            {
                Id = d.Id,
                Name = d.Name,
                Type = d.Type.ToString(),
                Description = d.Description,
                IsAvailable = d.IsAvailable,
                CurrentSessionId = d.CurrentSession?.Id
            };
            return Ok(dto);
        }

        // POST: /api/devices
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DeviceCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var device = new Device
            {
                Name = dto.Name,
                Type = dto.Type,
                Description = dto.Description,
                IsAvailable = true
            };

            if (dto.InstalledGameIds != null && dto.InstalledGameIds.Any())
            {
                var games = await _db.Games.Where(g => dto.InstalledGameIds.Contains(g.Id)).ToListAsync();
                device.InstalledGames = games;
            }

            _db.Devices.Add(device);
            await _db.SaveChangesAsync();

            var resultDto = new DeviceDto
            {
                Id = device.Id,
                Name = device.Name,
                Type = device.Type.ToString(),
                Description = device.Description,
                IsAvailable = device.IsAvailable
            };

            return CreatedAtAction(nameof(Get), new { id = device.Id }, resultDto);
        }

        // PUT: /api/devices/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] DeviceUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var device = await _db.Devices
                .Include(d => d.InstalledGames)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (device == null) return NotFound();

            device.Name = dto.Name;
            device.Type = dto.Type;
            device.Description = dto.Description;
            device.IsAvailable = dto.IsAvailable;

            // update installed games
            device.InstalledGames.Clear();
            if (dto.InstalledGameIds != null && dto.InstalledGameIds.Any())
            {
                var games = await _db.Games.Where(g => dto.InstalledGameIds.Contains(g.Id)).ToListAsync();
                foreach (var g in games) device.InstalledGames.Add(g);
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: /api/devices/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _db.Devices.FindAsync(id);
            if (device == null) return NotFound();

            _db.Devices.Remove(device);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
