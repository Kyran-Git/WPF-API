using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO.Journal;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class JournalRepository : IJournalRepo
    {
        private readonly ApplicationDBContext _context;
        public JournalRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Journal>> GetAllAsync()
        {
            return await _context.Journals.Include(e => e.Entries).ToListAsync();
        }

        public async Task<Journal> CreateAsync(Journal journalModel)
        {
            await _context.Journals.AddAsync(journalModel);
            await _context.SaveChangesAsync();
            return journalModel;
        }

        public async Task<Journal?> GetByIdAsync(int id)
        {
            return await _context.Journals.Include(e => e.Entries).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Journal?> DeleteAsync(int id)
        {
            var journalModel = await _context.Journals.FirstOrDefaultAsync(x => x.Id == id);
            if(journalModel == null)
            {
                return null;
            }

            _context.Journals.Remove(journalModel);
            await _context.SaveChangesAsync();
            return journalModel;
        }

        public async Task<Journal?> UpdateAsync(int id, UpdateJournalReqDTO JournalDTO)
        {
            var existingJournal = await _context.Journals.FirstOrDefaultAsync(x => x.Id == id);

            if(existingJournal == null)
            {
                return null;
            }
            existingJournal.Name = JournalDTO.Name;
            await _context.SaveChangesAsync();
            return existingJournal;
        }

        public Task<bool> JournalExist(int id)
        {
            return _context.Journals.AnyAsync(s => s.Id == id);
        }
    }
}