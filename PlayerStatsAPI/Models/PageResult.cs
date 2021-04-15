using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI.Models
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemsForm { get; set; }
        public int ItemTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PageResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            ItemsForm = pageSize * (pageNumber - 1) + 1;
            ItemTo = ItemsForm + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalCount /(double) pageSize);
        }
    }
}
