using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteWebApI.BLServices.Contracts;

namespace WebsiteWebApI.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileBlService fileBlService;

        public FilesController(IFileBlService fileBlService)
        {
            this.fileBlService = fileBlService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetFile(string id)
        {
            try
            {
                var fileId = new Guid(id);
                var file = this.fileBlService.GetFile(fileId);

                var fileResult = new FileContentResult(file.Data, file.File.FileExtension)
                {
                    FileDownloadName = file.File.FileName
                };

                return fileResult;
            }
            catch (Exception ex)
            {
                var s = 5;
            }

            return Ok();
        }
    }
}
