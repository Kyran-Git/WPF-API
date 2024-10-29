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
    public class JournalRepository : IJournalRepo
    {
        private readonly ApplicationDBContext _context;
        public JournalRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public Task<List<Journal>> GetAllAsync()
        {
            return _context.Journals.ToListAsync();
        }
    }
}