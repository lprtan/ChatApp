using ChatAppServer.WepAPI.Context;
using ChatAppServer.WepAPI.Dtos;
using ChatAppServer.WepAPI.Models;
using GenericFileService.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.WepAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public sealed class AuthController(ApplicationDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDto request, CancellationToken cancellationToken)
        {
            bool isNameExists = await context.Users.AnyAsync(p => p.Name == request.Name, cancellationToken);
            if (isNameExists)
            {
                return BadRequest(new { Message = "Bu kullanıcı adı daha önce kullanılmış" });
            }

            string avatar = FileService.FileSaveToServer(request.File, "wwwroot/avatar/");

            User user = new()
            {
                Name = request.Name,
                Avatar = avatar
            };

            await context.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string name, CancellationToken cancellationToken)
        {
            User? user = await context.Users.FirstOrDefaultAsync(p => p.Name == name, cancellationToken); 

            if (user is null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunmadı" });
            }

            user.Status = "online";

            await context.SaveChangesAsync(cancellationToken);

            return Ok(user);
        }
    }
}
