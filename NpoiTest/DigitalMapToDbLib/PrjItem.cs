using DigitalMapToDB.DigitalMapParser.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalMapToDB.DigitalMapParser.Utils;
using NPOI.Util;

namespace DigitalMapToDB.DigitalMapParser
{
    internal class PrjItem
    {
        private const string TAG = "PrjItem";

        /// <summary>
        /// 工程根目录
        /// </summary>
        private String prjPath;

        /// <summary>
        /// 各个文件夹的处理工具类
        /// </summary>
        private TextParser textParser;
        private VectorParser vectorParser;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="prjPath"></param>
        public PrjItem(String prjPath)
        {
            //文件路径
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
            //初始化一些工程Constant数据(前提是---height文件夹中必须要有project.txt文件)
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
        /// 当前给定的路径是否可用
        /// </summary>
        /// <returns></returns>
        public static bool isDirectoryValid(string path)
        {
            string[] files = Directory.GetDirectories(path);
            ArrayList fileList = Arrays.AsList(files);
            foreach (string str in fileList)
            {
                Log.Err(TAG, "当前文件是:" + str);
            }
            Log.Err(TAG, "我需要的是:   " + path + "\\text");
            Log.Err(TAG, "我需要的是:   " + path + "\\vector");
            if (!fileList.Contains(path + "\\text") || !fileList.Contains(path + "\\vector"))
            {
                return false;
            }
            return true;
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
        public VectorParser getVectorParser()
        {
            return vectorParser;
        }
    }
}