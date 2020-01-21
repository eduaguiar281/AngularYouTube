using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularSPA.Models
{
    public class ResponseRoot<T>
    {
        public ResponseRoot()
        {
            Success = true;
        }
        public T Data { get; set; }

        public bool Success { get; set; }
        public string Message { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? TotalCount { get; set; }
        public int? TotalPages { get; set; }

        public bool HasPreviousPage
        {
            get { return ((PageIndex ?? 0) > 0); }
        }
        public bool HasNextPage
        {
            get { return ((PageIndex ?? 0) + 1 < (TotalPages ?? 0)); }
        }

    }
}
