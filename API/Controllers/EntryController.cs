using API.Mappers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("API/Entry")]
    [ApiController]
    public class EntryController : ControllerBase
    {
       private readonly IEntryRepo _entryRepo;
       public EntryController(IEntryRepo entryRepo)
       {
        _entryRepo = entryRepo;
       } 

       [HttpGet]
       public async Task<IActionResult> GetAll()
       {
            var entries = await _entryRepo.GetAllAsync();
            var entryDTO = entries.Select(s => s.ToEntryDTO());
            return Ok(entryDTO);
       }
    }
}