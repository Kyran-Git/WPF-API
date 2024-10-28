using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.Journal;
using API.Models;

namespace API.Mappers
{
    public static class JournalMapper
    {
        public static JournalDTO ToJournalDTO(this Journal journalModel)
        {
            return new JournalDTO
            {
                Id = journalModel.Id,
                Name = journalModel.Name,
            };
        }

        public static Journal ToJournalFromCreateDTO(this CreateJournalReqDTO JournalDTO)
        {
            return new Journal
            {
                Name = JournalDTO.Name,
            };
        }
    }
}