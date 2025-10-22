using System;
using System.Threading.Tasks;
using backend.Db;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly OrganizationService _orgService;
        private readonly HttpClient _httpClient;


        public AuthController(AppDbContext db, OrganizationService orgService, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _orgService = orgService;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // ✅ Verificar si el usuario ya existe
            var existingUser = _db.Users.FirstOrDefault(u => u.ClerkUserId == request.ClerkUserId);
            if (existingUser != null)
            {
                return BadRequest(new { message = "El usuario ya está registrado." });
            }

            // ✅ Crear usuario en DB
            var user = new User
            {
                ClerkUserId = request.ClerkUserId,
                Email = request.Email
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // ✅ Guardar la organización usando el ID de Clerk que viene desde el front
            var org = await _orgService.AddOrganizationAsync(request.ClerkOrgId, request.OrganizationName, user.Id);

            //enviar la plaza al backend:

            try
            {
                var crearPlazaDto = new
                {
                    nombre = org.Name ?? "Plaza de prueba",
                    rut = "12345678-9",
                    direccion = "Av. Principal 123, Bogotá",
                    telefono = "+57 3001234567",
                    emailContacto = user.Email ?? "contacto@plaza.com",
                    representanteLegal = "Juan Pérez"
                };

                // Endpoint del backend admin
                var adminUrl = "http://host.docker.internal:8085/api/admin/plazas/public";

                var response = await _httpClient.PostAsJsonAsync(adminUrl, crearPlazaDto);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error al registrar la plaza en backend admin: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con backend admin: {ex.Message}");
            }


            return Ok(new
            {
                message = "Usuario y organización registrados correctamente",
                organizationId = request.ClerkOrgId
            });
        }
    }

    public class RegisterRequest
    {
        public string ClerkUserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string OrganizationName { get; set; } = string.Empty;
        public string ClerkOrgId { get; set; } = string.Empty; // 👈 Se pasa desde el front
    }
}
