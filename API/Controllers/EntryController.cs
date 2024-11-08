using API.Mappers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.DTO.Entry;
using API.Repository;
using API.Models;

namespace API.Controllers
{
    [Route("API/Entry")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly IEntryRepo _entryRepo;
        private readonly IJournalRepo _journalRepo;
        public EntryController(IEntryRepo entryRepo, IJournalRepo journalRepo)
        {
            _entryRepo = entryRepo;
            _journalRepo = journalRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entries = await _entryRepo.GetAllAsync();
            var entryDTO = entries.Select(s => s.ToEntryDTO());
            return Ok(entryDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var entry = await _entryRepo.GetByIdAsync(id);

            if (entry == null)
            {
                return NotFound();
            }

            return Ok(entry.ToEntryDTO());
        }

        [HttpPost("{journalId}")]
        public async Task<IActionResult> Create([FromRoute] int journalId, CreateEntryDTO entryDTO)
        {
            if(!await _journalRepo.JournalExist(journalId))
            {
                return BadRequest("Journal does not exist");
            }

            var entryModel = entryDTO.ToEntryFromCreate(journalId);
            await _entryRepo.CreateAsync(entryModel);
            return CreatedAtAction(nameof(GetById), new { id = entryModel }, entryModel.ToEntryDTO());
        }
    }
}