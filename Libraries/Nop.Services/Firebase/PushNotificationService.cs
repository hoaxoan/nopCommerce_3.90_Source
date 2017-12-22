using System;
using System.Linq;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Orders;
using Nop.Services.Directory;
using Nop.Services.Events;
using FCM.Net;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Services.Firebase
{
    /// <summary>
    /// Push notification service interface
    /// </summary>
    public partial class PushNotificationService : IPushNotificationService
    {
        #region Constants
        private const string FIREBASE_API_KEY = "AAAA6aU_jGE:APA91bEp98iZ8GWvm1lwU4saGVKGlnJRRwrSJHIi1eftEUl5YaQyUOB_N2saTQ-IXJtY0gW5TMngn2EQxq0JwdOL8dUYNnczKEGsMqiKeXg-dfE3NFgqKSfTJ15lHI8qn4CqO2V8X1CR";
        private const string FIREBASE_SENDER_ID = "1003499785313";
        #endregion

        #region Fields
        private readonly IRepository<Device> _deviceRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        public PushNotificationService(ICacheManager cacheManager,
            IRepository<Device> deviceRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._deviceRepository = deviceRepository;
        }

        #endregion

        #region Methods

        public async Task SendPushNotification(Order order)
        {
            try
            {
                List<string> registrationIds = _deviceRepository.Table.Select(x => x.Token).ToList();

                using (var sender = new Sender(FIREBASE_API_KEY, FIREBASE_SENDER_ID))
                {
                    var message = new Message
                    {
                        RegistrationIds = registrationIds,
                        Notification = new Notification
                        {
                            Title = string.Format("Order No#{0} - {1:dd/MM/yyyy: HH:mm}", order.Id, order.CreatedOnUtc),
                            Body = string.Format("Order No#{0} with Total {1} VND", order.Id, order.OrderTotal),
                        }
                    };
                    await sender.SendAsync(message);
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}