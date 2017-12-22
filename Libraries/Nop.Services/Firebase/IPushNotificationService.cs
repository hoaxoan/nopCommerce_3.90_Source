
using Nop.Core.Domain.Orders;
using System.Threading.Tasks;

namespace Nop.Services.Firebase
{
    /// <summary>
    /// Push notification service interface
    /// </summary>
    public partial interface IPushNotificationService
    {
        /// <summary>
        /// Send push notification
        /// </summary>
        /// <param name="order">Order</param>
        Task SendPushNotification(Order order);
    }
}