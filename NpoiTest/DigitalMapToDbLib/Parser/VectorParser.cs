using DigitalMapToDB.DigitalMapParser.MapData;
using DigitalMapToDB.DigitalMapParser.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DigitalMapToDB.DigitalMapParser.Parser
{
    internal class VectorParser
    {
        private static string TAG = "VectorParser";

        /// <summary>
        /// 根目录
        /// </summary>
        private String rootPath;

        /// <summary>
        /// 所有要解析的文件path列表
        /// </summary>
        private List<String> dataFilePathList = new List<string>();

        /// <summary>
        /// 所有解析出来的数据
        /// </summary>
        private List<Vector> vectorList = new List<Vector>();

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="rootPath"></param>
        public VectorParser(string rootPath)
        {
            this.rootPath = rootPath;

            //初始化要解析文件的List
            initDataFilePathList();

            //初始化Vector数据
            initVector();
        }

        /// <summary>
        /// 初始化要解析文件的List
        /// </summary>
        private void initDataFilePathList()
        {
            //读取index文件
            StreamReader sr = new StreamReader(rootPath + "\\index.txt");
            string lineStr;
            while ((lineStr = sr.ReadLine()) != null)
            {
                String[] args = lineStr.Split(' ');
                dataFilePathList.Add(rootPath + "\\" + args[0]);
            }
            //TODO---打印常看数据
//            foreach (string path in dataFilePathList)
//            {
//                Log.Err(TAG, path);
//            }
        }

        /// <summary>
        /// 初始化Vector数据
        /// </summary>
        public void initVector()
        {
            foreach (string path in dataFilePathList)
            {
                Log.Err(TAG, "当前解析的文件是: "+path);
                //获取当前文件
                StreamReader sr = new StreamReader(path, Encoding.Default);
                //一行行的读
                String lineStr;
                while ((lineStr = sr.ReadLine()) != null)
                {
                    //获取当前行单词提取出来的List<String>
                   // Log.Err(TAG, "转换之前：   "+lineStr);
                    lineStr = Regex.Replace(lineStr, "\\s{1,}", " ");
                    //Log.Err(TAG, "：   " + lineStr);
                    String[] args = lineStr.Split(' ');
                    List<String> strList = new List<String>();
                    foreach (string tempStr in args)
                    {
                        strList.Add(tempStr);
                    }
                    //将List<String>中的空串剔除
                    for (int i=0; i<strList.Count; i++)
                    {
                        string str = strList[i];
                        if (str.Contains(" ") || str.Length == 0)
                        {
                            strList.RemoveAt(i);
                        }
                    }
                    //添加数据
                    if (strList.Count == 3)
                    {
                        //添加一个新的Vector---(样例数据: 3 Railway 102)  
                        vectorList.Add(new Vector(strList[0], strList[1], strList[2]));
                    }
                    else if (strList.Count == 2)
                    {
                        //否则---向最后一个vector里面添加数据
                        if (vectorList.Count != 0)
                        {
                            Point point = new Point(Convert.ToDouble(strList[1]),
                                Convert.ToDouble((strList[0])));
                            vectorList[vectorList.Count - 1].addPoint(point);
                        }
                    }
                }
                sr.Close();
                //TODO---查看数据
//                Log.Err(TAG, "我现在---有这么多个： "+vectorList.Count);
//                foreach (Vector vector in vectorList)
//                {
//                    Log.Err(TAG, vector.ToString());
//                }
                Log.Err(TAG, "完成一个vector文件的解析");
            }//end of for each file
        }

        /// <summary>
        /// 获取Vecto的List
        /// </summary>
        /// <returns></returns>
        public List<Vector> getVectorList()
        {
            return vectorList;
        }
    }
}