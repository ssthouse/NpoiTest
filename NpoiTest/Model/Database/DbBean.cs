using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiTest.Model.Database
{
    class DbBean
    {
        public string PrjName { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public DbBean()
        {
            PrjName = "hahaha";
            Longitude = "a'regm";
            Latitude = "mlerkgnr";
        }
    }
}
