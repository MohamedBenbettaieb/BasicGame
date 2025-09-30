using BasicGameService.Data;
using BasicGameService.DTOs;
using BasicGameService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicGame.Controllers
{
    public class GameController : Controller
    {
        private readonly AppDbContext _db;

        public GameController(AppDbContext db)  
        {
            _db = db;
        }

        // GET: /Game
        public async Task<IActionResult> Index()
        {
            var games = await _db.Games.ToListAsync();
            return View(games);
        }

        // GET: /Game/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var game = await _db.Games.FirstOrDefaultAsync(g => g.Id == id.Value);
            if (game == null) return NotFound();

            return View(game);
        }

        // GET: /Game/Create
        public IActionResult Create()
        {
            // If you need enums in a select list, the view can use Html.GetEnumSelectList<GameType>()
            return View(new GameCreateDto());
        }

        // POST: /Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var game = new Game
            {
                Name = dto.Name,
                gameType = dto.GameType
            };

            _db.Games.Add(game);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Game/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var game = await _db.Games.FindAsync(id.Value);
            if (game == null) return NotFound();

            var dto = new GameUpdateDto
            {
                Id = game.Id,
                Name = game.Name,
                GameType = game.gameType
            };

            return View(dto);
        }

        // POST: /Game/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GameUpdateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var game = await _db.Games.FindAsync(dto.Id);
            if (game == null) return NotFound();

            game.Name = dto.Name;
            game.gameType = dto.GameType;

            _db.Games.Update(game);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Game/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var game = await _db.Games.FirstOrDefaultAsync(g => g.Id == id.Value);
            if (game == null) return NotFound();

            return View(game);
        }

        // POST: /Game/DeleteConfirmed
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _db.Games.FindAsync(id);
            if (game == null) return NotFound();

            // Optional: check for related sessions and prevent delete if necessary
            var relatedSessions = await _db.Sessions.AnyAsync(s => s.GameId == id);
            if (relatedSessions)
            {
                ModelState.AddModelError("", "Cannot delete game because it is referenced by sessions.");
                return View(game);
            }

            _db.Games.Remove(game);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
