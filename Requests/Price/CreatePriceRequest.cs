using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Requests.Price
{
    public class CreatePriceRequest
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public int PriceValue { get; set; }
    }
}