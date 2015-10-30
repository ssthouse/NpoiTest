using DigitalMapToDB.DigitalMapParser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMapToDB.DigitalMapParser.MapData
{
    class TextPoint
    {

        /**
         * 大地坐标
         */
        private double longitude;
        private double latitude;

        /**
         * 显示的数据
         */
        private string content;

        /**
         * 构造方法
         *
         * @param longitude
         * @param latitude
         * @param content
         */
        public TextPoint(double longitude, double latitude, string content)
        {
            //进行坐标转换
            double[] BL = CoordinateConverter.UTMWGSXYtoBL(longitude, latitude);
            this.latitude = BL[0];
            this.longitude = BL[1];
            this.content = content;
        }

        /// <summary>
        /// 返回描述的文字
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "我的数据是:\t" + latitude + "\t" + longitude + "\t" + content;
        }

        //getter------------and--------------setter

        public double getLongitude()
        {
            return longitude;
        }

        public void setLongitude(double longitude)
        {
            this.longitude = longitude;
        }

        public double getLatitude()
        {
            return latitude;
        }

        public void setLatitude(double latitude)
        {
            this.latitude = latitude;
        }

        public string getContent()
        {
            return content;
        }

        public void setContent(string content)
        {
            this.content = content;
        }
    }
}
