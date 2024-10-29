using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using API.DTO.Journal;
using API.Mappers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("API/Journal")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IJournalRepo _journalRepo;
        public JournalController(ApplicationDBContext context, IJournalRepo journalRepo)
        {
            _journalRepo = journalRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var journal = await _journalRepo.GetAllAsync();
            var journalDTO = journal.Select(s => s.ToJournalDTO());
            return Ok(journal);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var journal = await _journalRepo.GetByIdAsync(id);

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
            await _journalRepo.CreateAsync(journalModel);
            return CreatedAtAction("GetById", new { id = journalModel.Id }, journalModel.ToJournalDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateJournalReqDTO updateDTO)
        {
            var journalModel = await _journalRepo.UpdateAsync(id, updateDTO);
            if(journalModel == null)
            {
                return NotFound();
            }

            return Ok(journalModel.ToJournalDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var journalModel = await _journalRepo.DeleteAsync(id);
            if(journalModel == null)
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}