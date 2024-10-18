using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Responses.Genre
{
    public class GenreResponse
    {
        public string? Id { get; set; }
        
        public string? Name { get; set; }

        public string? CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }
    }
}