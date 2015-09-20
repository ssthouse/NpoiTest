using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiTest.Model
{
    class dgBean : INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private String name;

        public dgBean()
        {
            name = "hahaha";
        }
    }
}
