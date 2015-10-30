using DigitalMapToDB.DigitalMapParser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMapToDB.DigitalMapParser.MapData
{
    class Point
    {
        /// <summary>
        /// latitude,longitude 值
        /// </summary>
        private double latitude;
        private double longitude;

        /// <summary>
        /// 构造方法---传入的数据是---大地坐标
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public Point(double longitude, double latitude)
        {
            double[] BL = CoordinateConverter.UTMWGSXYtoBL(longitude, latitude);
            this.latitude = BL[0];
            this.longitude = BL[1];
        }

        //getter---and---setter-----------------------
        public double getLatitude()
        {
            return latitude;
        }

        public void setLatitude(double latitude)
        {
            this.latitude = latitude;
        }

        public double getLongitude()
        {
            return longitude;
        }

        public void setLongitude(double longitude)
        {
            this.longitude = longitude;
        }
    }
}
