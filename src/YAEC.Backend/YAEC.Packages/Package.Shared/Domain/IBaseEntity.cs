namespace Package.Shared.Domain;

public interface IBaseEntity<TKey>
{
    TKey Id { get; set; }
    
    long AutoId { get; set; }
}