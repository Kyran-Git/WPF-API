using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.Entry;
using API.Models;

namespace API.Interfaces
{
    public interface IEntryRepo
    {
        Task<List<Entry>> GetAllAsync();
        Task<Entry?> GetByIdAsync(int id);
        Task<Entry> CreateAsync(Entry entryModel);
        Task<Entry?> UpdateAsync(int id, Entry entryModel);
        Task<Entry?> DeleteAsync(int id);
    }
}