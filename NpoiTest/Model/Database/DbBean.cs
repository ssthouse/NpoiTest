using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiTest.Model.Database
{
    internal class DbBean
    {
        //三个关键数据----工程名---经度---维度
        public string PrjName { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public DbBean()
        {
            PrjName = "hahaha";
            Longitude = "a'regm";
            Latitude = "mlerkgnr";
        }

        /// <summary>
        /// 传入三个关键数据的构造方法
        /// </summary>
        /// <param name="prjName"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public DbBean(string prjName, double longitude, double latitude)
        {
            PrjName = prjName;
            Longitude = longitude + "";
            Latitude = latitude + "";
        }
    }
}