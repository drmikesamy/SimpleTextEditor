using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleTextEditor.Enums
{
    [Flags]
    public enum FormatBlockTypeEnum : short
    {
        None = 0,
        B = 1,
        I = 2,
        U = 4
    }
}
