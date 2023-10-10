using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TowerinoSignaler;

/// <summary>
/// Game hub class that handles all the signalR calls
/// Blue team is represented as a 0 and red team is represented as a 1
/// </summary>
public class GameHub : Hub
{
    private static readonly Dictionary<string, string> Sessions = new();

    #region SessionManagement

    public override Task OnConnectedAsync()
    {
        Clients.Caller.SendAsync("Connected");
        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? e)
    {
        await LeaveSession();
        await Clients.Caller.SendAsync("Disconnected");
        await base.OnDisconnectedAsync(e);
    }

    public async Task CreateSession()
    {
        var sessionKey = GenerateSessionKey();
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionKey);
        await Clients.Caller.SendAsync("SessionCreated", sessionKey);
        Sessions.Add(Context.ConnectionId, sessionKey);
    }

    public async Task JoinSession(string sessionKey)
    {
        //TODO string comparison doesn't work ?
        //Iterate through all sessions
        if (Sessions.Any(session => string.Compare(session.Value, sessionKey.Replace("\u200B", ""), StringComparison.Ordinal) == 0))
        {
            sessionKey = sessionKey.Replace("\u200B", "");
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionKey);
            await Clients.Caller.SendAsync("JoinedSession", sessionKey);
            await Clients.Group(sessionKey).SendAsync("PlayerJoined", Context.ConnectionId);
            Sessions.Add(Context.ConnectionId, sessionKey);
            return;
        }

        await Clients.Caller.SendAsync("SessionNotFound", sessionKey);
    }

    public async Task LeaveSession()
    {
        var sessionKey = GetSessionKey(Context.ConnectionId);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract (LIES)
        if (sessionKey != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionKey);
            await Clients.Caller.SendAsync("LeftSession", sessionKey);
            await Clients.Group(sessionKey).SendAsync("PlayerLeft", Context.ConnectionId);
            Sessions.Remove(Context.ConnectionId);
        }
    }

    #endregion

    #region Summons

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

    #endregion

    #region TowerUpgrades

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

    #endregion

    #region UnitUpgrades

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

    #endregion

    #region Utilities

    private static string GetSessionKey(string connectionId)
    {
        return Sessions.FirstOrDefault(x => x.Key == connectionId).Value;
    }

    private string GenerateSessionKey()
    {
        string sessionKey;
        do
        {
            sessionKey = RandomString(6);
        } while (Sessions.ContainsValue(sessionKey));

        return sessionKey;
    }

    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        return new string(new char[length].Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }

    #endregion
}