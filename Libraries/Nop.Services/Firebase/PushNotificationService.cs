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
using Nop.Services.Catalog;
using Nop.Services.Localization;

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
        private readonly IPriceFormatter _priceFormatter;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        public PushNotificationService(ICacheManager cacheManager,
            IRepository<Device> deviceRepository,
            IPriceFormatter priceFormatter,
            ILocalizationService localizationService,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._deviceRepository = deviceRepository;
            this._priceFormatter = priceFormatter;
            this._localizationService = localizationService;
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
                            Title = string.Format(_localizationService.GetResource("Notification.TitleFormat"), order.Id, order.ShippedDateUtc),
                            Body = string.Format(_localizationService.GetResource("Notification.BodyFormat"), order.Id, _priceFormatter.FormatPrice(order.OrderTotal)),
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