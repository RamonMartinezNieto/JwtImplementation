using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestImplementaciónJWT.Entities;
using TestImplementaciónJWT.Models.Request;
using TestImplementaciónJWT.Models.Response;
using TestImplementaciónJWT.Services;

namespace TestImplementaciónJWT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]  //Meto authorize para que use JWT 
    public class JwtController : ControllerBase
    {
        private IUserService _userService;

        //Nota: userService se inyecta graciás al AddScoped en la clase StartUp que inyecta en cada petición la clase 
        public JwtController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("ping")]
        [Authorize(Roles = Role.User)]
        public IActionResult ping()
        {
            return Ok(true);
        }

        [HttpGet("testToken")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult testToken()
        {
            return Ok("BIEEEEEEEEEEEEEEEEEN");
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult GetToken([FromBody] AuthRequest model) 
        {
            Response response = new Response(); 
            var userResponse = _userService.Auth(model);

            if (userResponse == null) 
            {
                response.Exito = -1;
                response.Mensaje = "Usuario o password incorrecto";
                return BadRequest(response);
            }

            response.Exito = 1;
            response.Mensaje = "Token Obtenido";
            response.Data = userResponse;

            return Ok(response); 
        }
    }
}
