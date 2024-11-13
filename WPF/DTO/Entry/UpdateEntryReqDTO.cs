using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPF.DTO.Entry
{
    public class UpdateEntryReqDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}