namespace IDemandApp.Domain;

public abstract class BaseEntity : Notifiable<Notification>
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }  
    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
