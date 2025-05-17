namespace Package.Shared.Domain;

public interface ISoftDeleteEntity<TKey>
{
    DateTime? DeletedAt { get; set; }
    
    TKey? DeletedBy { get; set; }
}