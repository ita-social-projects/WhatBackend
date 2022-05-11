using CharlieBackend.Panel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CharlieBackend.Core.DTO.Secretary;

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

        public async Task<IActionResult> AllSecretaries()
        {
            var secretaries = await _secretaryService.GetAllSecretariesAsync();

            return View(secretaries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EnableSecretary(long id)
        {
            await _secretaryService.EnableSecretaryAsync(id);

            return RedirectToAction("AllSecretaries", "Secretaries");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DisableSecretary(long id)
        {
            var disabledSecretary = await _secretaryService.DisableSecretaryAsync(id);

            return RedirectToAction("AllSecretaries", "Secretaries");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateSecretary(long id)
        {
            var secretary = await _secretaryService.GetSecretaryByIdAsync(id);

            ViewBag.Secretary = secretary;

            return View("UpdateSecretary");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateSecretary(long id, UpdateSecretaryDto data)
        {            
            var updatedStudent = await _secretaryService.UpdateSecretaryAsync(id, data);

            return RedirectToAction("AllSecretaries", "Secretaries");
        }
    }
}

