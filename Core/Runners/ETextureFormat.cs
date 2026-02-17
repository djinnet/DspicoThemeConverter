using EnumStringValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DspicoThemeForms.Core.Runners;

public enum ETextureFormat : int
{
    None = 0,
    [StringValue("a3i5")]
    A3I5 = 1,
    [StringValue("a5i3")]
    A5I3 = 2,
    [StringValue("palette16")]
    Palette16 = 3,
    [StringValue("palette256")]
    Palette256 = 4,
    [StringValue("rgba8888")]
    RGBA8888 = 5,
    [StringValue("rgb565")]
    RGB565 = 6,
    [StringValue("direct")]
    Direct = 7
}
