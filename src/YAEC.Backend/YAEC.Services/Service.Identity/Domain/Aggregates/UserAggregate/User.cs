using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Package.Shared.Domain;

namespace Service.Identity.Domain.Aggregates.UserAggregate;

public class User : IBaseEntity<string>, IAuditableEntity<string>, ISoftDeleteEntity<string>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    
    public long AutoId { get; set; }
    
    public string FullName { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? CreatedAt { get; set; }
    
    [MaxLength(128)]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CreatedBy { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? ModifiedAt { get; set; }
    
    [MaxLength(128)]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ModifiedBy { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? DeletedAt { get; set; }
    
    [MaxLength(128)]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? DeletedBy { get; set; }
}