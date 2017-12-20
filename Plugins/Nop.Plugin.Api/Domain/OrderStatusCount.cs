using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Api.Domain
{
    public class OrderStatusCount : BaseEntity
    {
        public Int32 OrderStatusId { get; set; }
        public String OrderStatusName { get; set; }
        public Int32 OrderCount { get; set; }
    }
}
