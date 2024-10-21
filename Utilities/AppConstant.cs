namespace MovieApi.Utilities
{
    public class AppConstant
    {
        public const string ROLE_ADMIN = "admin";
        
        public const string ROLE_CUSTOMER = "customer";

        public const int DISPLAY_LENGTH = 10;

        public const string PRICE_WEEKEND = "weekend";

        public const string PRICE_WEEKDAY = "weekday";

        public const string PRICE_PRIMETIME = "primetime";

        public const string ORDER_FROM_ADMIN = "web-admin";

        public const string ORDER_FROM_CUSTOMER = "web-customer";

        public const string PAYMENT_TYPE_OTS = "ots";

        public const string PAYMENT_TYPE_TRANSFER = "transfer";

        public enum PaymentStatus
        {
            CANCEL = -1,
            UNPAID = 0,
            PAID = 1
        }

        public enum StatusBooking
        {
            CANCEL = -1,
            NEW = 0,
            ON_CONFIRMATION = 1,
            DONE = 2
        }
    }
}