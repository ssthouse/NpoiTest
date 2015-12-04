using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DigitalMapToDB.DigitalMapParser.Utils;

namespace NpoiTest.ExcelInputToXml
{
    class ExcelToXmlConverter
    {
        private const string TAG = "ExcelToXmlConverter";

        /// <summary>
        /// 输出的xml文件中的属性
        /// </summary>
        private const string ATTRIBUTE_VALUE = "value";

        /// <summary>
        /// 保存从Excel中解析出来的数据
        /// </summary>
        private List<Data> dataList = new List<Data>();

        /// <summary>
        /// 输出路径
        /// </summary>
        private string filePath;


        /// <summary>
        /// 输入一个文件路径的构造方法
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        public ExcelToXmlConverter(string filePath)
        {
            this.filePath = filePath;
            //解析文件
            parseExcel();
        }

        /// <summary>
        /// 检查Excel中解析出来的数据是否正确
        /// </summary>
        /// <returns></returns>
        public bool IsDataValid()
        {
            Log.Err(TAG, "我的数据一共有：\t"+dataList.Count);
            bool result = true;
            foreach (Data data in dataList)
            {
                if (data.Kmmark.Length == 0 || data.Lateral.Length == 0 || data.DistanceToRail.Length == 0)
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 解析Excel文件 填充数据到dataList中
        /// </summary>
        private void parseExcel()
        {
            XSSFWorkbook wk = null;
            using (FileStream fs = File.Open(filePath, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite))
            {
                //把xls文件读入workbook变量里，之后就可以关闭了
                wk = new XSSFWorkbook(fs);
                fs.Close();
            }
            //获取第一个表格
            ISheet sheet = wk.GetSheetAt(0);
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                Data data = new Data();
                //填充数据
                data.Devicetype = row.GetCell(1).ToString();
                //如果连设备类型都没有---就直接跳过好了
                if (data.Devicetype.Length == 0)
                {
                    continue;
                }
                data.Kmmark = row.GetCell(2).ToString();
                data.Lateral = row.GetCell(3).ToString();
                data.DistanceToRail = row.GetCell(4).ToString();
                data.Longitude = row.GetCell(5).ToString();
                data.Latitude = row.GetCell(6).ToString();
                data.Comment = row.GetCell(7).ToString();
                dataList.Add(data);
            }
        }

        /// <summary>
        /// 将解析出的数据保存在xml文件中
        /// </summary>
        /// <param name="outputPath">输出路径</param>
        /// <param name="dataList">待输出的路径</param>
        public void ExportXmlFile(string outputPath)
        {
            XmlDocument save = new XmlDocument();
            XmlDeclaration decl = save.CreateXmlDeclaration("1.0", "utf-8", "");
            XmlElement xmlData = save.CreateElement("data");
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].Devicetype != "")
                {
                    //创建标签对
                    XmlElement devicetype = save.CreateElement("devicetype");
                    XmlElement kmmark = save.CreateElement("kmmark");
                    XmlElement lateral = save.CreateElement("lateral");
                    XmlElement distanceToRail = save.CreateElement("distanceToRail");
                    XmlElement longitude = save.CreateElement("longitude");
                    XmlElement latitude = save.CreateElement("latitude");
                    XmlElement comment = save.CreateElement("comment");
                    //将数据写在 "value" 属性中
                    devicetype.SetAttribute(ATTRIBUTE_VALUE, dataList[i].Devicetype);
                    kmmark.SetAttribute(ATTRIBUTE_VALUE, dataList[i].Kmmark);
                    lateral.SetAttribute(ATTRIBUTE_VALUE, dataList[i].Lateral);
                    distanceToRail.SetAttribute(ATTRIBUTE_VALUE, dataList[i].DistanceToRail);
                    comment.SetAttribute(ATTRIBUTE_VALUE, dataList[i].Comment);
                    if (!(dataList[i].Longitude == "" || dataList[i].Latitude == ""))
                    {
                        longitude.SetAttribute(ATTRIBUTE_VALUE, dataList[i].Longitude);
                        latitude.SetAttribute(ATTRIBUTE_VALUE, dataList[i].Latitude);
                    }
                    //添加子标签
                    devicetype.AppendChild(kmmark);
                    devicetype.AppendChild(lateral);
                    devicetype.AppendChild(distanceToRail);
                    devicetype.AppendChild(longitude);
                    devicetype.AppendChild(latitude);
                    devicetype.AppendChild(comment);
                    //添加一个数据根标签
                    xmlData.AppendChild(devicetype);
                }
            }
            save.AppendChild(decl);
            save.AppendChild(xmlData);
            //输出文件
            save.Save(outputPath);
        }
    }
}
