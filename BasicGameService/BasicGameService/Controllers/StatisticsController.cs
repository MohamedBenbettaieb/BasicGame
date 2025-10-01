using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicGameService.Data;
using BasicGameService.Models.Stats;

namespace BasicGameService.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly AppDbContext _db;

        public StatisticsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Statistics
        public async Task<IActionResult> Index()
        {
            var devices = await _db.Devices.ToListAsync();
            var games = await _db.Games.ToListAsync();
            var sessions = await _db.Sessions.ToListAsync();

            var deviceStats = devices.Select(d => new DeviceStat
            {
                DeviceId = d.Id,
                DeviceName = d.Name,
                TimesUsed = sessions.Count(s => s.DeviceId == d.Id),
                TotalTimeMinutes = sessions
                    .Where(s => s.DeviceId == d.Id && s.EndTime.HasValue)
                    .Sum(s => (s.EndTime!.Value - s.StartTime).TotalMinutes)
            })
            .OrderByDescending(ds => ds.TimesUsed)
            .ToList();

            var gameStats = games.Select(g => new GameStat
            {
                GameId = g.Id,
                GameName = g.Name,
                TimesPlayed = sessions.Count(s => s.GameId == g.Id)
            })
            .OrderByDescending(gs => gs.TimesPlayed)
            .ToList();

            var vm = new StatisticsViewModel
            {
                DeviceStats = deviceStats,
                GameStats = gameStats
            };

            return View(vm);
        }
    }
}
