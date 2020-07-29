using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.Model.Models
{
    public class PagingResultModel<T> where T : class
    {
        public IList<T> Data { get; set; }

        public int Count { get; set; }
    }
}
