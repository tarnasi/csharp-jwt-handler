using GymMate.Models;
using GymMate.TokenGeneration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymMate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _Config;

        public HomeController(IConfiguration configuration)
        {
            _Config = configuration;
        }

        [HttpGet]
        [Authorize]
        public List<Athlete> Index()
        {
            Random random = new();
            List<string> names = new()
            {
                "Rylee",
                "Kadin",
                "Carolina",
                "Melanie",
                "Destiny",
            };
            List<string> families = new()
            {
                "Sparks",
                "Lucas",
                "Frederick",
                "Baird",
                "Potter",
            };
            var athletes = Enumerable.Range(1, 5).Select<int, Athlete>(index =>
            {
                return new Athlete
                {
                    Name = names[random.Next(0, 4)],
                    Family = families[random.Next(0, 4)],
                    Age = random.Next(1, 99)
                };
            }).ToList();
            return athletes;
        }

        [HttpPost("login")]
        public IActionResult CaliamIssueAsync()
        {
            ClaimTokenIssuer Cti = new(_Config);
            var token = Cti.GenerateJwtToken();
            return Ok(new { Token = token });
        }
    }
}
