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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEntryDTO entryDTO)
        {
            if(!await _journalRepo.JournalExist(entryDTO.JournalId))
            {
                return BadRequest("Journal does not exist");
            }

            //var entryModel = entryDTO.ToEntryFromCreate(journalId);

            Entry entry = new Entry()
            {
                Title = entryDTO.Title,
                Content = entryDTO.Content,
                JournalId = entryDTO.JournalId,
            };
            await _entryRepo.CreateAsync(entry);
            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, entry.ToEntryDTO());
        }

        [HttpPut]
        [Route("{id}")]

        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEntryReqDTO updateDTO)
        {
            var entry = await _entryRepo.UpdateAsync(id, updateDTO.ToEntryFromUpdate());
            if(entry == null)
            {
                return NotFound("Entry not found!");
            }
            
            return Ok(entry.ToEntryDTO());
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entryModel = await _entryRepo.DeleteAsync(id);
            if(entryModel == null)
            {
                return NotFound("Entry not found!");
            }

            return NoContent();
        }
    }
}