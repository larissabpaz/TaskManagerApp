using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [Authorize]
        [HttpGet ("{id}")]
        public ActionResult<Response<string>> GetUsuario()
        {
            Response<string> response = new Response<string>();
            response.Mensagem = "Acessei";

            return Ok(response);
        }

    }