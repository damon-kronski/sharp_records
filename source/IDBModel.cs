using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DK.Active_Records
{
    public interface IDBModel
    {
        object this[string name] { get; }
        int Count { get; }
        void setProperties(Dictionary<string, object> dict);
    }
}
