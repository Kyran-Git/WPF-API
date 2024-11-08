using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using API.Controllers;
using Microsoft.EntityFrameworkCore;
using API.DTO.Entry;

namespace API.Repository
{
    public class EntryRepository : IEntryRepo
    {
        
        private readonly ApplicationDBContext _context;
        public EntryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Entry> CreateAsync(Entry entryModel)
        {
            await _context.Entries.AddAsync(entryModel);
            await _context.SaveChangesAsync();
            return entryModel;
        }

        public async Task<List<Entry>> GetAllAsync()
        {
            return await _context.Entries.ToListAsync();
        }

        public async Task<Entry?> GetByIdAsync(int id)
        {
            return await _context.Entries.FindAsync(id);
        }


        public async Task<Entry?> UpdateAsync(int id, Entry entryModel)
        {
            var existingEntry = await _context.Entries.FindAsync(id);

            if (existingEntry == null)
            {
                return null;
            }

            existingEntry.Title = entryModel.Title;
            existingEntry.Content = entryModel.Content;
            await _context.SaveChangesAsync();
            return existingEntry;
        }

        public async Task<Entry?> DeleteAsync(int id)
        {
            var entryModel = await _context.Entries.FirstOrDefaultAsync(x => x.Id == id);
            if(entryModel == null)
            {
                return null;
            }

            _context.Entries.Remove(entryModel);
            await _context.SaveChangesAsync();
            return entryModel;
        }
    }
}