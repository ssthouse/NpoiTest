using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using NpoiTest.Model.Database;
using NPOI.HSSF.UserModel;

namespace NpoiTest.Office.Word
{
    /// <summary>
    /// Excel文件输出工具
    /// </summary>
    internal class ExcelGenerator
    {
        /// <summary>
        /// 数据源
        /// </summary>
        private readonly List<DbBean> dataList;
        /// <summary>
        /// Excel输出路径
        /// </summary>
        private readonly string outputPath;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataList">需要导出的数据</param>
        /// <param name="outputPath">输出路径</param>
        public ExcelGenerator(List<DbBean> dataList, string outputPath)
        {
            this.dataList = dataList;
            this.outputPath = outputPath;
        }

        /// <summary>
        ///     导出WOrd文件
        /// </summary>
        public void Generate()
        {
            //首先在内存中创建一个Excel文件
            var fileStream = new FileStream(outputPath, FileMode.OpenOrCreate);
            var workbook = new HSSFWorkbook();
            //创建一个表(sheet)--并获取他
            var sheet = (HSSFSheet) workbook.CreateSheet("华中科技大学工程");
            //首先把需要多少个cell创建好----有多少个数据就有多少行--每行有3个数据
            for (var i = 0; i <= dataList.Count; i++)
            {
                sheet.CreateRow(i);
            }
            //创建表头
            GenerateHeaderRow(sheet);
            //然后给每行---创建单元格---添加数据
            GenerateDataRow(sheet);
            try
            {
                //获取目标文件流--写入文件数据
                workbook.Write(fileStream);
                fileStream.Close();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                MessageBox.Show("文件导出失败", "失败");
                return;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            MessageBox.Show("Excel文件导出成功", "完成");
        }

        /// <summary>
        /// 创建Excel的表头
        /// </summary>
        private void GenerateHeaderRow(HSSFSheet sheet)
        {
            //获取第一行
            HSSFRow row = (HSSFRow)sheet.GetRow(0);
            //填入数据
            row.CreateCell(0).SetCellValue("序号");
            row.CreateCell(1).SetCellValue("设备类型");
            row.CreateCell(2).SetCellValue("公里标");
            row.CreateCell(3).SetCellValue("侧向");
            row.CreateCell(4).SetCellValue("经度");
            row.CreateCell(5).SetCellValue("纬度");
            row.CreateCell(6).SetCellValue("备注文本");
        }

        /// <summary>
        /// 创建数据行
        /// </summary>
        /// <param name="sheet"></param>
        private void GenerateDataRow(HSSFSheet sheet)
        {
            for (var i = 1; i < dataList.Count; i++)
            {
                var row = (HSSFRow)sheet.GetRow(i);
                DbBean dbBean = dataList[i - 1];
                //序号
                row.CreateCell(0).SetCellValue(i);
                //设备类型
                row.CreateCell(1).SetCellValue(dbBean.DeviceType);
                //公里标
                row.CreateCell(2).SetCellValue(dbBean.KilometerMark);
                //侧向
                row.CreateCell(3).SetCellValue(dbBean.SideDirection);
                //经度
                double temp;
                if (double.TryParse(dataList[i].Longitude, out temp))
                {
                    row.CreateCell(4).SetCellValue(Double.Parse((dataList[i].Longitude)));
                }
                else
                {
                    row.CreateCell(4).SetCellValue((dataList[i].Longitude));
                }
                //纬度
                if (double.TryParse(dataList[i].Latitude, out temp))
                {
                    row.CreateCell(5).SetCellValue(double.Parse(dataList[i].Latitude));
                }
                else
                {
                    row.CreateCell(5).SetCellValue(dataList[i].Latitude);
                }
                //备注文本
                row.CreateCell(6).SetCellValue("备注文本");
            }
        }
    }
}