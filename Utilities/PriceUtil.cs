namespace MovieApi.Utilities
{
    public class PriceUtil
    {
        public string GetIDRCurrency(int price)
        {
            return "Rp. " + price.ToString("N0");
        }
    }
}