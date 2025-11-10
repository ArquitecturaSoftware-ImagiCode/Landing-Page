using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly WorkerService _service;

        public WorkerController(WorkerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Worker>>> GetWorkers()
        {
            var workers = await _service.GetAllWorkersAsync();
            return Ok(workers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Worker>> GetWorker(int id)
        {
            var worker = await _service.GetWorkerByIdAsync(id);
            if (worker == null) return NotFound();
            return Ok(worker);
        }

        [HttpPost]
        public async Task<ActionResult<Worker>> CreateWorker(Worker worker)
        {
            var created = await _service.CreateWorkerAsync(worker);
            return CreatedAtAction(nameof(GetWorker), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Worker>> UpdateWorker(int id, Worker worker)
        {
            if (id != worker.Id) return BadRequest();

            var updated = await _service.UpdateWorkerAsync(worker);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorker(int id)
        {
            var deleted = await _service.DeleteWorkerAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
