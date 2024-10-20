namespace MovieApi.Requests.Price
{
    public class UpdatePriceRequest
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public int PriceValue { get; set; }
    }
}