using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPF.Models
{
    public class Entry
    {
        public int? JournalId { get; set; }
        public Journal? Journal { get; set; }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string Content { get; set; } = string.Empty;
    }
}