using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPF.DTO.Entry;

namespace WPF.DTO.Journal
{
    public class JournalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<EntryDTO> Entries { get; set; }
    }
}