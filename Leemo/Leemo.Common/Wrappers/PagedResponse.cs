using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents Leemo common project
/// </summary>
namespace TPSS.Common.Wrappers
{
    public class PagedResponse<T> : ApiResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, string message, bool succeeded, Enums.HttpStatusCode responseCode)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = message;
            this.Succeeded = succeeded;
            this.ResponseCode = responseCode;
        }

        public static PagedResponse<T> PagedList(IEnumerable<T> source, string message, bool succeeded, Enums.HttpStatusCode responseCode, int pageNumber = 0, int pageSize = 0)
        {
            if (pageNumber > 0 && pageSize > 0)
            {
                var data = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                return new PagedResponse<T>(data, pageNumber, pageSize, message, succeeded, responseCode);
            }

            return new PagedResponse<T>(source, pageNumber, pageSize, message, succeeded, responseCode);
        }
    }
}
