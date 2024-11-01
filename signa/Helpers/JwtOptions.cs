namespace signa.Helpers;

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    
    public int ExpireHours { get; set; }
}