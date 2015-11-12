using DigitalMapToDB.DigitalMapParser.MapData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalMapToDB.DigitalMapParser.Utils;

namespace DigitalMapToDB.DigitalMapParser.Parser
{
    internal class TextParser
    {
        private const string TAG = "TextParser";

        /// <summary>
        /// 根目录
        /// </summary>
        private String rootPath;

        /// <summary>
        /// 保存所有要解析的文件的路径
        /// </summary>
        private List<String> dataFilePathList = new List<String>();

        /// <summary>
        /// 保存所有文件解析出来的数据
        /// </summary>
        private List<TextPoint> textPointList = new List<TextPoint>();

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="rootPath"></param>
        public TextParser(String rootPath)
        {
            this.rootPath = rootPath;

            //初始化要解析文件的List
            initDataFilePathList();

            //初始化文字信息
            initTextPoint();
        }

        /// <summary>
        /// 获取textPoint表
        /// </summary>
        /// <returns></returns>
        public List<TextPoint> getTextPointList()
        {
            return textPointList;
        }

        /// <summary>
        /// 读取所有文件中的TextPoint的数据
        /// </summary>
        private void initTextPoint()
        {
            foreach (string path in dataFilePathList)
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
                //获取每一行的数据---如果是匹配的---提取出来一个TextPoint
                String lineStr;
                while ((lineStr = sr.ReadLine()) != null)
                {
                    String[] args = lineStr.Split(' ');
                    if (args.Length > 2)
                    {
                        TextPoint textPoint = new TextPoint(Convert.ToDouble(args[1]),
                            Convert.ToDouble(args[0]), args[2]);
                        textPointList.Add(textPoint);
                    }
                }
                sr.Close();
                //TODO---打印数据
//                foreach (TextPoint textPoint in textPointList)
//                {
//                    Log.Err(TAG, textPoint.ToString());
//                }
            }
        }

        /// <summary>
        /// 将index。txt文件中的文件列表扫描出来
        /// </summary>
        private void initDataFilePathList()
        {
            StreamReader sr = new StreamReader(rootPath + "\\index.txt", Encoding.Default);
            string lineStr;
            while ((lineStr = sr.ReadLine()) != null)
            {
                string fileName = lineStr.Split(' ')[0];
                //添加一个扫描到的文件名称
                dataFilePathList.Add(rootPath + "\\" + fileName);
            }
        }
    }
}