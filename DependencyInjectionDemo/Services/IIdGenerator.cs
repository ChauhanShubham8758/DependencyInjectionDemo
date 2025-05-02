namespace DependencyInjectionDemo.Services
{
    public interface IIdGenerator
    {
        Guid NewId { get; }
    }

    public class TransientIdGenerator : IIdGenerator
    {
        public Guid NewId => Guid.NewGuid();
    }
}
