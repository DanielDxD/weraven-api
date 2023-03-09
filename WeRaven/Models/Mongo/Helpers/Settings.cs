namespace WeRaven.Models.Mongo.Helpers;

public class Settings
{
    // Account
    public string Country { get; set; } = "BR";
    public string Lang { get; set; } = "en_us";
    public string? Gender { get; set; } = null;
    
    // Post and security
    public bool PublishSensitiveContent { get; set; } = false;
    public bool ShowSensitiveContent { get; set; } = false;
    public bool ShowSensitiveContentOnSearch { get; set; } = false;
    public bool AllowMessagesFromAnybody { get; set; } = false;
    public bool AllowFindByEmail { get; set; } = true;
    public bool PersonalizedAds { get; set; } = true;
    public string? Coordinates { get; set; } = null;
}