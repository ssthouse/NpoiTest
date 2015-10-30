using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMapToDB.DigitalMapParser
{
    internal class PrjConstant
    {
        // 工程根目录下面的文件目录名称
        public const String CLUTTER = "clutter";
        public const String HEIGHTS = "heights";
        public const String TEXT = "text";
        public const String VECTOR = "vector";
        public const String PROJECT_FILE_NAME = "projection.txt";

        /// <summary>
        /// 一些会被运行中改变的变量数据
        /// </summary>
        private static double centralLongitude;

        public static double getCentralLongitude()
        {
            return centralLongitude;
        }

        public static void setCentralLongitude(double centralLongitude)
        {
            PrjConstant.centralLongitude = centralLongitude;
        }
    }
}