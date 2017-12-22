using Nop.Core.Domain.Common;

namespace Nop.Data.Mapping.Common
{
    public partial class DeviceMap : NopEntityTypeConfiguration<Device>
    {
        public DeviceMap()
        {
            this.ToTable("Device");
            this.HasKey(a => a.Id);
        }
    }
}
