namespace Shop.Domain.Constraints;
public static class CartConstrains
{
    public static class Expiration
    {
        public static TimeSpan AbsoluteExpireTime => TimeSpan.FromMinutes(60);
        public static TimeSpan UnusedExpireTime => TimeSpan.FromMinutes(20);
    }
}
