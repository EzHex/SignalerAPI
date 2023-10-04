using Microsoft.AspNetCore.SignalR;

namespace TowerinoSignaler;

public class GameHub : Hub
{
    private static readonly Dictionary<string, string> Sessions = new();
    
    public async Task CreateSession()
    {
        string sessionKey = GenerateSessionKey();
        await Clients.Caller.SendAsync("SessionCreated", sessionKey);
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionKey);
        Sessions.Add(Context.ConnectionId, sessionKey);
    }

    public async Task JoinSession(string sessionKey)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionKey);
        await Clients.Caller.SendAsync("JoinedSession", sessionKey);
        await Clients.Group(sessionKey).SendAsync("PlayerJoined", Context.ConnectionId);
        Sessions.Add(Context.ConnectionId, sessionKey);
    }

    public async Task LeaveSession()
    {
        string sessionKey = GetSessionKey(Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionKey);
        await Clients.Caller.SendAsync("LeftSession", sessionKey);
        await Clients.Group(sessionKey).SendAsync("PlayerLeft", Context.ConnectionId);
        Sessions.Remove(Context.ConnectionId);
    }

    // Helper method to generate a unique 6-character/digit session key
    private string GenerateSessionKey()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        return new string(new char[6].Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }
    
    //Add color to all methods as parameter (color is represented by boolean 0 is blue 1 is red)
    public async Task SummonKnight(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("SummonKnight", color);
    }
    
    public async Task SummonTank(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("SummonTank", color);
    }
    
    public async Task SummonSpeedo(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("SummonSpeedo", color);
    }
    
    public async Task UpgradeDamage(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeDamage", color);
    }
    
    public async Task UpgradeSpeed(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeSpeed", color);
    }
    
    public async Task UpgradeHealth(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeHealth", color);
    }
    
    public async Task UpgradeTower(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeTower", color);
    }
    
    public async Task UpgradeKnightAttack(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeKnightAttack", color);
    }
    
    public async Task UpgradeKnightHealth(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeKnightHealth", color);
    }
    
    public async Task UpgradeKnightSpeed(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeKnightSpeed", color);
    }
    
    public async Task UpgradeTankAttack(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeTankAttack", color);
    }
    
    public async Task UpgradeTankHealth(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeTankHealth", color);
    }
    
    public async Task UpgradeTankSpeed(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeTankSpeed", color);
    }
    
    public async Task UpgradeSpeedoAttack(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeSpeedoAttack", color);
    }
    
    public async Task UpgradeSpeedoHealth(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeSpeedoHealth", color);
    }
    
    public async Task UpgradeSpeedoSpeed(bool color)
    {
        await Clients.Group(GetSessionKey(Context.ConnectionId)).SendAsync("UpgradeSpeedoSpeed", color);
    }
    
    private static string GetSessionKey(string connectionId)
    {
        return Sessions.FirstOrDefault(x => x.Key == connectionId).Value;
    }
    
    //create enum for red blue team
    
}