using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CharlieBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SamplesController : ControllerBase
    {
        private readonly ISampleService _sampleService;
        public SamplesController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSamplesAsync()
        {
            //try
            //{
            //    var samples = await _sampleService.GetAllAsync();
            //    return Ok(samples);
            //} catch { return StatusCode(500); }

            return StatusCode(501);
        }

        [HttpPost]
        public async Task<IActionResult> PostSampleAsync(Sample sample)
        {
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        await _sampleService.InsertAsync(sample);
            //        return Ok(sample);
            //    }
            //    catch { return StatusCode(500); }
            //}
            //else return StatusCode(422);

            return StatusCode(501);
        }

        [HttpDelete]
        public IActionResult DeleteSample(Sample sample)
        {
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _sampleService.Delete(sample);
            //        return Ok();
            //    } catch { return StatusCode(422);  }
            //}
            //else { return StatusCode(422); }

            return StatusCode(501);
        }
    }
}
