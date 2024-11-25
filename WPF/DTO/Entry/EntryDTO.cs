 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPF.Models;

namespace WPF.DTO.Entry
{
    public class EntryDTO
    {
        public int? JournalId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Content { get; set; } = string.Empty;
    }
}