using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ClaudeBot.Controllers
{
    [Route("api")]
    [ApiController]
    public class ClaudeController : ControllerBase
    {
        [HttpPost("[action]")]
        [Consumes(MediaTypeNames.Text.Plain)]
        public async Task<IActionResult> GetAnswerFromAI([FromBody] string question)
        {
            Console.WriteLine(question);
            var result = await HttpClientExample.Send(question);
            return Ok(result);
        }
    }
}
