using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Api.Delta;
using Nop.Plugin.Api.Factories;
using Nop.Plugin.Api.Helpers;
using Nop.Plugin.Api.JSON.ActionResults;
using Nop.Plugin.Api.ModelBinders;
using Nop.Plugin.Api.Serializers;
using Nop.Plugin.Api.Services;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Plugin.Api.DTOs.Devices;
using Nop.Core.Domain.Common;

namespace Nop.Plugin.Api.Controllers
{
    public class DevicesController : BaseApiController
    {
        private readonly IDeviceService _deviceService;
        private readonly IDTOHelper _dtoHelper;
        private readonly IStoreContext _storeContext;
        private readonly IFactory<Order> _factory;
        
        public DevicesController(IDeviceService deviceService,
            IJsonFieldsSerializer jsonFieldsSerializer,
            IAclService aclService,
            ICustomerService customerService,
            IStoreMappingService storeMappingService,
            IStoreService storeService,
            IDiscountService discountService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            IFactory<Order> factory,
            IStoreContext storeContext,
            IPictureService pictureService,
            IDTOHelper dtoHelper)
            : base(jsonFieldsSerializer, aclService, customerService, storeMappingService,
                 storeService, discountService, customerActivityService, localizationService, pictureService)
        {
            _deviceService = deviceService;
            _factory = factory;
            _storeContext = storeContext;
            _dtoHelper = dtoHelper;
        }

        [HttpPost]
        [ResponseType(typeof(DevicesRootObject))]
        public IHttpActionResult CreateDevice([ModelBinder(typeof(JsonModelBinder<DeviceDto>))] Delta<DeviceDto> deviceDelta)
        {
            var device = new Device();
            device.Cordova = deviceDelta.Dto.Cordova;
            device.Model = deviceDelta.Dto.Model;
            device.Platform = deviceDelta.Dto.Platform;
            device.UUID = deviceDelta.Dto.UUID;
            device.Version = deviceDelta.Dto.Version;
            device.Manufacturer = deviceDelta.Dto.Manufacturer;
            device.Serial = deviceDelta.Dto.Serial;
            device.Token = deviceDelta.Dto.Token;

            _deviceService.InsertUpdateDevice(device);

            var devicesRootObject = new DevicesRootObject();
            devicesRootObject.Devices.Add(deviceDelta.Dto);

            var json = _jsonFieldsSerializer.Serialize(devicesRootObject, string.Empty);

            return new RawJsonActionResult(json);
        }

    }
}