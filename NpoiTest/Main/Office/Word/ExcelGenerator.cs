using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using NpoiTest.Model.Database;
using NPOI.HSSF.Record.CF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;

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
        private readonly List<Model.Database.DbBean> dataList;

        /// <summary>
        /// Excel输出路径
        /// </summary>
        private readonly string outputPath;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataList">需要导出的数据</param>
        /// <param name="outputPath">输出路径</param>
        public ExcelGenerator(List<Model.Database.DbBean> dataList, string outputPath)
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
            GenerateHeaderRow(workbook, sheet);
            //然后给每行---创建单元格---添加数据
            GenerateDataRow(workbook, sheet);
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
        private void GenerateHeaderRow(HSSFWorkbook workbook, HSSFSheet sheet)
        {
            //获取第一行
            HSSFRow row = (HSSFRow) sheet.GetRow(0);
            string[] columns = {"序号", "设备类型", "公里标", "侧向", "距线路中心距离", "经度", "纬度", "备注文本"};
            for (int i = 0; i<columns.Length; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(columns[i]);
                //cell.CellStyle.ShrinkToFit = true;
                InitHeaderCellStyle(workbook, cell);
            }
        }

        /// <summary>
        /// 初始化Header的style
        /// </summary>
        /// <param name="cell"></param>
        private void InitHeaderCellStyle(HSSFWorkbook workbook, ICell cell)
        {
            ICellStyle cellStyle = cell.CellStyle;
            //字体大小
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            font.Boldweight = 1;
            cellStyle.SetFont(font);
            //横向对齐
            cellStyle.Alignment = HorizontalAlignment.Justify;
            cellStyle.ShrinkToFit = true;
            //填充style
            cell.CellStyle = cellStyle;
        }

        /// <summary>
        /// 创建数据行
        /// </summary>
        /// <param name="sheet"></param>
        private void GenerateDataRow(HSSFWorkbook workbook, HSSFSheet sheet)
        {
            for (var i = 1; i <= dataList.Count; i++)
            {
                var row = (HSSFRow) sheet.GetRow(i);
                Model.Database.DbBean dbBean = dataList[i - 1];
                //序号
                ICell cell0 = row.CreateCell(0);
                cell0.SetCellValue(i);
                cell0.CellStyle.ShrinkToFit = true;
                InitDataCellStyle(workbook, cell0);
                //设备类型
                cell0 = row.CreateCell(1);
                cell0.SetCellValue(dbBean.DeviceType);
                cell0.CellStyle.ShrinkToFit = true;
                InitDataCellStyle(workbook, cell0);
                //公里标
                cell0 = row.CreateCell(2);
                cell0.SetCellValue(dbBean.KilometerMark);
                cell0.CellStyle.ShrinkToFit = true;
                InitDataCellStyle(workbook, cell0);
                //侧向
                cell0 = row.CreateCell(3);
                cell0.SetCellValue(dbBean.SideDirection);
                cell0.CellStyle.ShrinkToFit = true;
                InitDataCellStyle(workbook, cell0);
                //距线路中心距离
                cell0 = row.CreateCell(4);
                cell0.SetCellValue(dbBean.DistanceToRail);
                cell0.CellStyle.ShrinkToFit = true;
                InitDataCellStyle(workbook, cell0);
                //经度
                double temp;
                if (double.TryParse(dbBean.Longitude, out temp))
                {
                    cell0 = row.CreateCell(5);
                    cell0.SetCellValue(Double.Parse((dbBean.Longitude)));
                    cell0.CellStyle.ShrinkToFit = true;
                    InitDataCellNumStyle(workbook, cell0);
                }
                else
                {
                    cell0 = row.CreateCell(5);
                    cell0.SetCellValue((dbBean.Longitude));
                    cell0.CellStyle.ShrinkToFit = true;
                    InitDataCellNumStyle(workbook, cell0);
                }
                //纬度
                if (double.TryParse(dbBean.Latitude, out temp))
                {
                    cell0 = row.CreateCell(6);
                    cell0.SetCellValue(double.Parse(dbBean.Latitude));
                    cell0.CellStyle.ShrinkToFit = true;
                    InitDataCellNumStyle(workbook, cell0);
                }
                else
                {
                    cell0 = row.CreateCell(6);
                    cell0.SetCellValue(dbBean.Latitude);
                    cell0.CellStyle.ShrinkToFit = true;
                    InitDataCellNumStyle(workbook, cell0);
                }
                //备注文本
                cell0 = row.CreateCell(7);
                cell0.SetCellValue(dbBean.Comment);
                cell0.CellStyle.ShrinkToFit = true;
                InitDataCellStyle(workbook, cell0);
            }
        }

        /// <summary>
        /// 初始化数据Cell的style
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="cell"></param>
        private void InitDataCellStyle(HSSFWorkbook workbook, ICell cell)
        {
            ICellStyle cellStyle = cell.CellStyle;  
            //字体大小
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "宋体";
            cellStyle.SetFont(font);
            //横向对齐
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.ShrinkToFit = true;
            //填充style
            cell.CellStyle = cellStyle;
        }

        /// <summary>
        /// 初始化数据Cell为数字的style
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="cell"></param>
        private void InitDataCellNumStyle(HSSFWorkbook workbook, ICell cell)
        {
            ICellStyle cellStyle = cell.CellStyle;
            //字体大小
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 11;
            cellStyle.SetFont(font);
            //横向对齐
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.ShrinkToFit = true;
            //填充style
            cell.CellStyle = cellStyle;
        }
    }
}