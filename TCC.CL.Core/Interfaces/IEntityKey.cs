namespace TCC.CL.Core.Infraestrutura
{
    public interface IEntityKey<TKey>
    {
        TKey Id { get; }
    }
}
