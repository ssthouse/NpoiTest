using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiTest.ExcelInputToXml
{
    class Data
    {
        //设备类型
        public string Devicetype { get; set; }
        //公里标
        public string Kmmark { get; set; }
        //侧向
        public string Lateral { get; set; }
        //距线路中心距离
        public string DistanceToRail { get; set; }
        //经度
        public string Longitude { get; set; }
        //纬度
        public string Latitude { get; set; }
        //备注文本
        public string Comment { get; set; }
    }
}
