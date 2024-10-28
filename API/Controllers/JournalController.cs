using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;
using API.DTO.Journal;
using API.Mappers;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var journal = _context.Journals.ToList()
            .Select(s => s.ToJournalDTO());
            return Ok(journal);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var journal = _context.Journals.Find(id);

            if(journal == null)
            {
                return NotFound();
            }

            return Ok(journal.ToJournalDTO());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateJournalReqDTO JournalDTO)
        {
            var journalModel = JournalDTO.ToJournalFromCreateDTO();
            _context.Journals.Add(journalModel);
            _context.SaveChanges();
            return CreatedAtAction("GetById", new { id = journalModel.Id }, journalModel.ToJournalDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateJournalReqDTO updateDTO)
        {
            var journalModel = _context.Journals.FirstOrDefault(x => x.Id == id);
            if(journalModel == null)
            {
                return NotFound();
            }

            journalModel.Name = updateDTO.Name;
            _context.SaveChanges();
            return Ok(journalModel.ToJournalDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var journalModel = _context.Journals.FirstOrDefault(x => x.Id == id);
            if(journalModel == null)
            {
                return NotFound();
            }

            _context.Journals.Remove(journalModel);
            _context.SaveChanges();
            return NoContent();
        }
    }
}