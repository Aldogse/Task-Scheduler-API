using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskScheduler.Contracts
{
    public class PaginatedResponse<T>
    {
        public PaginatedResponse(int page_size, int page_count, int total_count, List<T> page_items)
        {
            this.page_size = page_size;
            this.page_count = page_count;
            this.total_count = total_count;
            this.page_items = page_items;
        }

        public int page_size { get; set; }
        public int page_count { get; set; }
        public int total_count { get; set; }
        public List<T> page_items { get; set; }
    }
}
