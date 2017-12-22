using System;
using System.Linq;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Common;
using Nop.Services.Events;

namespace Nop.Plugin.Api.Services
{
    /// <summary>
    /// Device service
    /// </summary>
    public partial class DeviceService : IDeviceService
    {

        #region Fields

        private readonly IRepository<Device> _deviceRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="deviceRepository">Device repository</param>
        /// <param name="eventPublisher">Event publisher</param>
        public DeviceService(ICacheManager cacheManager,
            IRepository<Device> deviceRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._deviceRepository = deviceRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts/Update an device
        /// </summary>
        /// <param name="device">Device</param>
        public virtual void InsertUpdateDevice(Device device)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            device.CreatedOnUtc = DateTime.UtcNow;

            Device dv = _deviceRepository.Table.FirstOrDefault(d => d.Token == device.Token);
            if (dv != null)
            {
                dv.Cordova = device.Cordova;
                dv.Model = device.Model;
                dv.Platform = device.Platform;
                dv.UUID = device.UUID;
                dv.Version = device.Version;
                dv.Manufacturer = device.Manufacturer;
                dv.Serial = device.Serial;
                dv.Token = device.Token;
                _deviceRepository.Update(dv);
            }
            else
            {
                _deviceRepository.Insert(device);
            }

            //event notification
            _eventPublisher.EntityInserted(device);
        }

        #endregion
    }
}