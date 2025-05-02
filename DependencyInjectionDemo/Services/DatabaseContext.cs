namespace DependencyInjectionDemo.Services
{
    public class DatabaseContext : IDatabaseContext, IDisposable
    {
        private readonly List<string> _orders = new();

        public void AddOrder(string orderDetails)
        {
            _orders.Add(orderDetails);
            Console.WriteLine($"Order added: {orderDetails}");
        }

        public List<string> GetOrders() => _orders;

        public void Dispose() => Console.WriteLine("DatabaseContext disposed");
    }
}
