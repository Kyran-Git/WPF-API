using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Journal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Entry> Entries { get; set; } = new List<Entry>();
    }
}