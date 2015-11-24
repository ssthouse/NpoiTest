using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalMapToDB.DigitalMapParser.Utils;

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
        private string typeInMap;
        public string TypeInMap
        {
            get { return typeInMap; }
        }

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
        /// <param typeInMap="number"></param>
        /// <param typeInMap="typeInMap"></param>
        /// <param typeInMap="code"></param>
        public Vector(string number, string typeInMap, string code)
        {
            this.code = code;
            this.typeInMap = typeInMap;
            this.number = number;
            pointList = new List<Point>();
            //Log.Err(TAG, "我的typeInMap是: "+typeInMap);
            //.Err(TAG, "number: " + number);
            //Log.Err(TAG, "code: " + code);
        }

        /// <summary>
        /// 添加点
        /// </summary>
        /// <param typeInMap="point"></param>
        public void addPoint(Point point)
        {
            pointList.Add(point);
        }

        /// <summary>
        /// 描述语句
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("我的数据是:" + "\n");
            sb.Append("" + typeInMap + "\t" + number + "\t" + code + "\n");
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
            return number + "" + typeInMap + code;
        }
    }
}
