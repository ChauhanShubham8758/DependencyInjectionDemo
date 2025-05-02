namespace DependencyInjectionDemo.Services
{
    public interface IDatabaseContext
    {
        void AddOrder(string orderDetails);
        List<string> GetOrders();
    }
}
