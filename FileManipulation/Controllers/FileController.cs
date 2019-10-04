using FileManipulation.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace FileManipulation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileManipulation _fileManipulation;

        public FileController(IFileManipulation fileManipulation)
        {
            _fileManipulation = fileManipulation;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(_fileManipulation.GetAllFiles());
        }

        [HttpPost]
        public IActionResult Post([FromQuery] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);
            }

            var result = _fileManipulation.IsValid(fileName);

            return result ? Ok() : StatusCode(500);
        }
    }
}