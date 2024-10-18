namespace signa.Entities;

public class SocialMediaLinkEntity
{
    public Guid Id { get; set; }
    
    public string Link { get; set; }
    
    public string SocialMediaType { get; set; }
    
    public Guid ConsumerId { get; set; }
    
}