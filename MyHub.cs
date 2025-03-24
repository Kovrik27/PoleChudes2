using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using WebApplication1;

internal class MyHub : Hub
{
    public MyHub(Rooms rooms)
    {
        this.rooms = rooms;
        rooms.SetStart(async (List<string> nicks, string gameId) =>
        {
            await clientsByNickname[nicks[0]].SendAsync("opponent", nicks, gameId);
            await clientsByNickname[nicks[1]].SendAsync("opponent", nicks, gameId);
            await clientsByNickname[nicks[2]].SendAsync("opponent", nicks, gameId);
            await clientsByNickname[nicks[3]].SendAsync("opponent", nicks, gameId);
            await clientsByNickname[nicks[0]].SendAsync("maketurn", 0);
        });
    }
    static Dictionary<string, ISingleClientProxy> clientsByNickname = new();
    private readonly Rooms rooms;
    public override Task OnConnectedAsync()
    {
        Clients.Caller.SendAsync("hello", "Придумай ник, ты же ничего больше не умеешь");
        Console.WriteLine("Глупи бейби");
        return base.OnConnectedAsync();
    }

    public void Nickname(string nickname)
    {
        var check = clientsByNickname.Keys.FirstOrDefault(s => s == nickname);
        if (check != null)
        {
            Clients.Caller.SendAsync("hello", "Придумай другой ник, дебилдурак");
            return;
        }
        else
        {
            clientsByNickname.Add(nickname, Clients.Caller);
            rooms.AddNewClient(nickname);
        }
    }

    public async void MakeTurn(Turn turn)
    {
        bool check = rooms.CheckChar(turn);
        if (check == true)
            await Clients.All.SendAsync("maketurn");
        else
        {
            string next = rooms.GetNextPlayer(turn);
        }
            
        
    }
}
