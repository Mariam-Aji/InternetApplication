using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task RegisterUser(string userId, string role)
        {
            if (role == "Citizen")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Citizen_{userId}");
            }
            else if (role == "GovernmentAgency")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Gov_{userId}");
            }
        }

        public async Task SendNotificationToCitizen(int userId, string message, int complaintId)
        {
            await Clients.Group($"Citizen_{userId}").SendAsync("ReceiveNotification", new
            {
                ComplaintId = complaintId,
                Message = message
            });
        }

        public async Task SendNotificationToGovernment(int govId, string message, int complaintId)
        {
            await Clients.Group($"Gov_{govId}").SendAsync("ReceiveNotification", new
            {
                ComplaintId = complaintId,
                Message = message
            });
        }
    }
}
