using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Options
{
    public class PaginationResponse<T>
    {
        public int TotalCount { get; set; }
        public List<T> DataList { get; set; } = new List<T>();
        public PaginationResponse(int totalCount, List<T> dataList)
        {
          this.TotalCount = totalCount;
            this.DataList = dataList ?? new List<T>();
        }
}
}
