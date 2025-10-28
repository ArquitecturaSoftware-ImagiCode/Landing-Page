using backend.Db;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class WorkerService
    {
        private readonly AppDbContext _context;

        public WorkerService(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los workers
        public async Task<List<Worker>> GetAllWorkersAsync()
        {
            return await _context.Workers.ToListAsync();
        }

        // Obtener un worker por ID
        public async Task<Worker?> GetWorkerByIdAsync(int id)
        {
            return await _context.Workers.FindAsync(id);
        }

        // Crear un worker
        public async Task<Worker> CreateWorkerAsync(Worker worker)
        {
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();
            return worker;
        }

        // Actualizar un worker
        public async Task<Worker?> UpdateWorkerAsync(Worker worker)
        {
            var existing = await _context.Workers.FindAsync(worker.Id);
            if (existing == null) return null;

            existing.Name = worker.Name;
            existing.Role = worker.Role;
            existing.Country = worker.Country;
            existing.Numero = worker.Numero;
            existing.Correo = worker.Correo;

            await _context.SaveChangesAsync();
            return existing;
        }

        // Eliminar un worker
        public async Task<bool> DeleteWorkerAsync(int id)
        {
            var existing = await _context.Workers.FindAsync(id);
            if (existing == null) return false;

            _context.Workers.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
