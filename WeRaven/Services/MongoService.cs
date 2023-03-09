using WeRaven.Providers;

namespace WeRaven.Services;

public class MongoService
{
    public ChatProvider Chat { get; set; } = new();
    public AccountProvider Account { get; set; } = new();
}