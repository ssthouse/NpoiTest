using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMapToDB.DigitalMapParser.MapData
{
    class Vector
    {
        private const String TAG = "Vector";

        /// <summary>
        /// 坐标的序号
        /// </summary>
        private string number;

        /// <summary>
        /// vector类别名称
        /// </summary>
        private String name;

        /// <summary>
        /// 右边的一个数字(暂时不知道是干嘛的
        /// </summary>
        private string code;

        /// <summary>
        /// 当前矢量包含的点
        /// </summary>
        private List<Point> pointList;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="number"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        public Vector(string number, String name, string code)
        {
            this.code = code;
            this.name = name;
            this.number = number;
            pointList = new List<Point>();
        }

        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="point"></param>
        public void addPoint(Point point)
        {
            pointList.Add(point);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("我的数据是:" + "\n");
            sb.Append("" + name + "\t" + number + "\t" + code + "\n");
            foreach (Point point in pointList)
            {
                sb.Append(point.getLongitude() + "\t" + point.getLatitude() + "\n");
            }
            return sb.ToString();
        }

        //获取Point的List
        public List<Point> getPointList()
        {
            return pointList;
        } 

        //getter----and----setter------------------------------
        public string getNumber()
        {
            return number;
        }

        public void setNumber(string number)
        {
            this.number = number;
        }

        public string getCode()
        {
            return code;
        }

        public void setCode(string code)
        {
            this.code = code;
        }
        public void setPointList(List<Point> pointList)
        {
            this.pointList = pointList;
        }

        public String getContent()
        {
            return number + "" + name + code;
        }
    }
}
