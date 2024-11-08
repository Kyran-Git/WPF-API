using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.Journal;
using API.Models;

namespace API.Interfaces
{
    public interface IJournalRepo
    {
        Task<List<Journal>> GetAllAsync();
        Task<Journal?> GetByIdAsync(int id);
        Task<Journal> CreateAsync(Journal journalModel);
        Task<Journal?> UpdateAsync(int id, UpdateJournalReqDTO JournalDTO);
        Task<Journal?> DeleteAsync(int id);
        Task<bool> JournalExist(int id);
    }
}