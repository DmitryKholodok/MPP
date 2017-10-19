using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5.Creator.Manager
{
    public interface IManager
    {
        bool WasMentioned(Type type);
        void MarkType(Type type);
    }
}
