using ChatAppServer.WepAPI.Context;
using ChatAppServer.WepAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppServer.WepAPI.Hubs
{
    public sealed class ChatHub(ApplicationDbContext context) : Hub
    {
        public static Dictionary<string, Guid> Users = new();
        public async Task Connect(Guid userId)
        {
            Users.Add(Context.ConnectionId, userId);
            User? user = await context.Users.FindAsync(userId);

            if (user is not null) 
            {
                user.Status = "online";
                context.SaveChangesAsync();

                await Clients.All.SendAsync("Users", user);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Guid userId;
            Users.TryGetValue(Context.ConnectionId, out userId);
            Users.Remove(Context.ConnectionId);
            User? user = await context.Users.FindAsync(userId);

            if (user is not null)
            {
                user.Status = "offline";
                context.SaveChangesAsync();

                await Clients.All.SendAsync("Users", user);
            }

        }
    }
}
