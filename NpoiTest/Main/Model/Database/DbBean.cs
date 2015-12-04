using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiTest.Model.Database
{
    /// <summary>
    /// 单个Marker需要获取的数据
    /// 也就是excel中一行需要有的数据
    /// </summary>
    internal class DbBean
    {
        //工程名
        public string PrjName { get; set; }
        //经度
        public string Longitude { get; set; }
        //纬度
        public string Latitude { get; set; }
        //设备类型
        public string DeviceType { get; set; }
        //公里标
        public string KilometerMark { get; set; }
        //侧向
        public string SideDirection { get; set; }
        //距线路中心距离
        public string DistanceToRail { get; set; }
        //照片路径
        public string PhotoPathName { get; set; }
        //备注
        public string Comment { get; set; }
        //杆塔类型
        public string TowerType { get; set; }
        //杆塔高度
        public string TowerHeight { get; set; }
        //天线方向1
        public string AntennaDirection1 { get; set; }
        //天线方向2
        public string AntennaDirection2 { get; set; }
        //天线方向3
        public string AntennaDirection3 { get; set; }
        //天线方向4
        public string AntennaDirection4 { get; set; }

        /// <summary>
        /// 空的构造方法
        /// </summary>
        public DbBean()
        {
        }

        /// <summary>
        /// 传入三个关键数据的构造方法
        /// </summary>
        /// <param name="prjName"></param>
        /// <param name="kilometerMark"></param>
        /// <param name="sideDirection"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="deviceType"></param>
        /// <param name="photoPathName"></param>
        public DbBean(string prjName, string kilometerMark, string sideDirection, string distanceToRail,
            double longitude, double latitude, string deviceType, string photoPathName, string comment)
        {
            PrjName = prjName;
            Longitude = longitude + "";
            Latitude = latitude + "";
            DeviceType = deviceType;
            KilometerMark = kilometerMark;
            SideDirection = sideDirection;
            DistanceToRail = distanceToRail;
            PhotoPathName = photoPathName;
            Comment = comment;
        }
    }
}