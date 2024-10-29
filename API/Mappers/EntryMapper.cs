using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.Entry;
using API.Models;

namespace API.Mappers
{
    public static class EntryMapper
    {
        public static EntryDTO ToEntryDTO(this Entry entryModel)
        {
            return new EntryDTO
            {
                Id = entryModel.Id,
                Title = entryModel.Title,
                Content = entryModel.Content,
                Date = entryModel.Date,
                JournalId = entryModel.JournalId
            };
        }
    }
}