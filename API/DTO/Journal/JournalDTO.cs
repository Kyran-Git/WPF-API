using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.Entry;

namespace API.DTO.Journal
{
    public class JournalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<EntryDTO> Entries { get; set; }
    }
}