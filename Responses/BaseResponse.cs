using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Responses
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public object? Data { get; set; }
    }
}