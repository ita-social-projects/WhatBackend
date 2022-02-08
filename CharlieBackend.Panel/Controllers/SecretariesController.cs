using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CharlieBackend.Panel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class SecretariesController : Controller
    {
        private readonly ISecretaryService _secretaryService;

        public SecretariesController(ISecretaryService secretaryService)
        {
            _secretaryService = secretaryService;
        }

        public async Task<IActionResult> GetAllSecretaries()
        {
            var secretaries = await _secretaryService.GetAllSecretariesAsync();

            return View(secretaries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EnableSecretary(long id)
        {
            await _secretaryService.EnableSecretaryAsync(id);

            return RedirectToAction("GetAllSecretaries", "Secretaries");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DisableSecretary(long id)
        {
            var disabledSecretary = await _secretaryService.DisableSecretaryAsync(id);

            return RedirectToAction("GetAllSecretaries", "Secretaries");
        }
    }
}

