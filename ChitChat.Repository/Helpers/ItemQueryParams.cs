using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Repository.Helpers
{
    public class ItemQueryParams
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Search { get; set; }
        public bool IsDecending { get; set; } = true;
    }
}
