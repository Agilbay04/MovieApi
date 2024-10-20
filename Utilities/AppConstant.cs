namespace MovieApi.Utilities
{
    public class AppConstant
    {
        public enum StatusActive
        {
            Active = 1,
            NotActive = 0
        }

        public enum StatusDeleted
        {
            Deleted = 1,
            NotDeleted = 0
        }

        public enum StatusDelete
        {
            Deleted = 1,
            NotDeleted = 0
        }

        public enum StatusPublished
        {
            Published = 1,
            NotPublished = 0
        }

        public const string ROLE_ADMIN = "admin";
        
        public const string ROLE_CUSTOMER = "customer";

        public const int DISPLAY_LENGTH = 10;
    }
}