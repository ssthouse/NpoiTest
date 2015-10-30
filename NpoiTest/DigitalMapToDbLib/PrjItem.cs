using DigitalMapToDB.DigitalMapParser.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMapToDB.DigitalMapParser
{
    class PrjItem
    {
        private const string TAG = "PrjItem";

        /**
         * 工程根目录
         */
        private String prjPath;

        /**
         * 各个文件夹的处理工具类
         */
        private TextParser textParser;
        private VectorParser vectorParser;

        public PrjItem(String prjPath)
        {
            this.prjPath = prjPath;

            //初始化一些工程数据
            initProjectConstant();

            //初始化文件目录
            initFile();
        }

        /// <summary>
        /// 初始化一些常量数据
        /// </summary>
        private void initProjectConstant()
        {
            //初始化一些工程Constant数据
            StreamReader sr = new StreamReader(prjPath + "\\heights\\" + PrjConstant.PROJECT_FILE_NAME);
                String lineStr;
                while ((lineStr = sr.ReadLine()) != null)
                {
                    String[] strArray = lineStr.Split(' ');
                    if (strArray.Length > 1)
                    {
                        //TODO---这里其实比较冒险---不知道是不是有多个数据的一行--第二个就是中央经线数据
                        PrjConstant.setCentralLongitude(Convert.ToDouble(strArray[1]));
                    }
                }
        }


 

        /// <summary>
        /// 初始化文件目录
        /// </summary>
        private void initFile()
        {
            //TODO
            //解析文字数据---保存在里textParser里面的List中
            textParser = new TextParser(prjPath + "\\text");

            //解析矢量数据----保存在VectorParser里面的List中
            vectorParser = new VectorParser(prjPath + "\\vector");
        }

        /// <summary>
        /// 获取文本解析器
        /// </summary>
        /// <returns></returns>
        public TextParser getTextParser()
        {
            return textParser;
        }

        /// <summary>
        /// 获取Vector的解析器
        /// </summary>
        /// <returns></returns>
        public  VectorParser getVectorParser()
        {
            return vectorParser;
        }
    }
}
