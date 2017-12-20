using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Plugin.Api.DTOs.Orders;
using Nop.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Api.MappingExtensions
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        public static IList<OrderStatusDto> ToSelectList<TEnum>(this TEnum enumObj,
           bool markCurrentAsSelected = true, int[] valuesToExclude = null, bool useLocalization = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("An Enumeration type is required.", "enumObj");

            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         where valuesToExclude == null
                         select new { ID = Convert.ToInt32(enumValue), Name = useLocalization ? enumValue.GetLocalizedEnum(localizationService, workContext) : CommonHelper.ConvertEnum(enumValue.ToString()) };
            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);

            IList<OrderStatusDto> orderStatusDtos = new List<OrderStatusDto>();
            foreach (var value in values)
            {
                orderStatusDtos.Add(new OrderStatusDto(Convert.ToString(value.ID), value.Name));
            }
            return orderStatusDtos;
        }
    }
}
