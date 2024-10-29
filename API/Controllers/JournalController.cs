using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using API.DTO.Journal;
using API.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("API/Journal")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public JournalController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var journal = await _context.Journals.ToListAsync();
            var journalDTO = journal.Select(s => s.ToJournalDTO());
            return Ok(journal);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var journal = await _context.Journals.FindAsync(id);

            if(journal == null)
            {
                return NotFound();
            }

            return Ok(journal.ToJournalDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJournalReqDTO JournalDTO)
        {
            var journalModel = JournalDTO.ToJournalFromCreateDTO();
            await _context.Journals.AddAsync(journalModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = journalModel.Id }, journalModel.ToJournalDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateJournalReqDTO updateDTO)
        {
            var journalModel = await _context.Journals.FirstOrDefaultAsync(x => x.Id == id);
            if(journalModel == null)
            {
                return NotFound();
            }

            journalModel.Name = updateDTO.Name;
            await _context.SaveChangesAsync();
            return Ok(journalModel.ToJournalDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var journalModel = await _context.Journals.FirstOrDefaultAsync(x => x.Id == id);
            if(journalModel == null)
            {
                return NotFound();
            }

            _context.Journals.Remove(journalModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}