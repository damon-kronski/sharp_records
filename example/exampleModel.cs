using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DK.Active_Records;

namespace DK.Example.Models
{
    public class ExampleItem : DBModel<ExampleItem> , IDBModel
    {
        public ExampleItem() : base() { }
    }
}
