using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiTest.ExcelInputToXml
{
    class Data
    {
        private String devicetype;
        private String kmmark;
        private String lateral;
        private String longitude;
        private String latitude;

        public string Devicetype
        {
            get
            {
                return devicetype;
            }

            set
            {
                devicetype = value;
            }
        }

        public string Kmmark
        {
            get
            {
                return kmmark;
            }

            set
            {
                kmmark = value;
            }
        }

        public string Lateral
        {
            get
            {
                return lateral;
            }

            set
            {
                lateral = value;
            }
        }

        public string Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
            }
        }

        public string Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
            }
        }
    }
}
