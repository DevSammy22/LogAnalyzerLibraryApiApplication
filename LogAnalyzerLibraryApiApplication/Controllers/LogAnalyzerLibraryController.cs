using LogAnalyzerLibraryCore.Service;
using LogAnalyzerLibraryModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LogAnalyzerLibraryApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogAnalyzerLibraryController : ControllerBase
    {
        private readonly ILibraryAnalyzer _libraryAnalyzer;

        public LogAnalyzerLibraryController(ILibraryAnalyzer libraryAnalyzer)
        {
            _libraryAnalyzer = libraryAnalyzer;
        }

        [HttpPost]
        [Route("SearchLogsInDirectories")]
        public IActionResult SearchLogsInDirectories([FromBody]Request request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _libraryAnalyzer.SearchLogsInDirectories(request);
                return Ok(result);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("CountUniqueErrors")]
        public IActionResult GetUniqueErrorsCount([FromBody]Request request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _libraryAnalyzer.CountUniqueErrorsAsync(request);
                return Ok(result);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }   

        [HttpPost]
        [Route("CountDuplicateErrors")]
        public IActionResult GetDuplicateErrorsCount([FromBody]Request request)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _libraryAnalyzer.CountDuplicatedErrorsAsync(request);
                return Ok(result);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("CountAllDuplicateErrors")]
        public async Task<IActionResult> GetAllDuplicatedErrorsCountAsync([FromBody]Request request)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _libraryAnalyzer.CountAllDuplicationsAsync(request);
                return Ok(result);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("DeleteArchiveFromPeriod")]
        public async Task<IActionResult> DeleteArchiveAsync(string path, string dateRange)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
               var result = await _libraryAnalyzer.DeleteArchiveFromPeriodAsync(path, dateRange);
                return Ok(result);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("ArchiveLogsFromPeriod")]
        public IActionResult ArchiveLogs([FromBody] ArchiveModel archiveModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _libraryAnalyzer.ArchiveLogsFromPeriodAsync(archiveModel);
                return Ok(result);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("UploadLogToServer")]
        public async Task<IActionResult> UploadLogToServerAsync([FromBody] UploadRequest uploadRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await  _libraryAnalyzer.UploadLogToServerAsync(uploadRequest);
                return Ok(result);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("DeleteLogsFromPeriod")]
        public IActionResult DeleteLogsFromPeriod(string directoryPath, string dateRange)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result =  _libraryAnalyzer.DeleteLogsFromPeriod(directoryPath, dateRange);
                return Ok(result);
               
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("TotalLogsAvailbale")]
        public IActionResult TotalLogsAvailbaleAsync(string directoryPath, string dateRange)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(_libraryAnalyzer.TotalLogsAvailbaleAsync(directoryPath, dateRange));

            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("SearchLogsPerSize")]
        public IActionResult SearchLogsPerSize([FromBody] SearchPerSize searchPerSize)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
               var result = _libraryAnalyzer.SearchLogsPerSize(searchPerSize);
                return Ok(result);

            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("SearchLogsPerDirectory")]
        public IActionResult SearchLogsPerDirectory([FromBody] SearchPerDirectory searchPerDirectory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _libraryAnalyzer.SearchLogsPerDirectory(searchPerDirectory);
                return Ok(result);

            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
