using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class EntryRepository : IEntryRepo
    {
        private readonly ApplicationDBContext _context;
        public EntryRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Entry>> GetAllAsync()
        {
            return await _context.Entries.ToListAsync();
        }
    }
}