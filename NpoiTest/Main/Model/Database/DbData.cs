using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using DigitalMapToDB.DigitalMapParser.Utils;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.OpenXmlFormats.Dml;

namespace NpoiTest.Model.Database
{
    /// <summary>
    ///     用于存储当前打开的数据库中的一些数据
    /// </summary>
    internal class DbData
    {
        private const string TAG = "DbData";

        /// <summary>
        /// 数据库常量
        /// </summary>
        private class DbCons
        {
            //数据库中的表名
            public const string TABLE_PROJECT = "Projects";
            public const string TABLE_MARKER = "Markers";
            //Marker表的列名
            public const string COLUMN_PRJNAME = "prjName";
            public const string COLUMN_KILOMETER_MARK = "kilometer_mark";
            public const string COLUMN_SIDE_DIRECTION = "side_direction";
            public const string COLUMN_DISTANCE_TO_RAIL = "distance_to_rail";
            public const string COLUMN_LONGITUDE = "longitude";
            public const string COLUMN_LATITUDE = "latitude";
            public const string COLUMN_DEVICE_TYPE = "device_type";
            public const string COLUMN_PHOTO_PATH_NAME = "photo_path_name";
            public const string COLUMN_COMMENT = "comment";
            public const string COLUMN_TOWER_TYPE = "tower_type";
            public const string COLUMN_TOWER_HEIGHT = "tower_height";
            //TODO---新加入的数据---后续需要解析
            public const string COLUMN_ANTENNA_DIRECTION_1 = "antenna_direction_1";
            public const string COLUMN_ANTENNA_DIRECTION_2 = "antenna_direction_2";
            public const string COLUMN_ANTENNA_DIRECTION_3 = "antenna_direction_3";
            public const string COLUMN_ANTENNA_DIRECTION_4 = "antenna_direction_4";
        }

        //唯一的数据库连接
        private SQLiteConnection connection;
        private const string DATABASE_PATH = ".\\Picture\\Location.db";

        /// <summary>
        /// 传入数据库路径的构造方法
        /// </summary>
        /// <param name="zipFilePath"></param>
        public DbData(string zipFilePath)
        {
            //首先把文件解压
            file_unzip(zipFilePath, ".\\");
            //首先判断数据库文件是否存在
            if (File.Exists(DATABASE_PATH))
            {
                connection = new SQLiteConnection("Data Source=" + DATABASE_PATH);
                connection.Open();
            }
            else
            {
                MessageBox.Show("压缩文件数据格式不正确");
            }
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="fileToUnZip">解压文件目录</param>
        /// <param name="zipedFolder">解压到哪里</param>
        private static void file_unzip(string fileToUnZip, string zipedFolder)
        {
            //如果存在文件夹则删除
            if (Directory.Exists(".\\Picture"))
                Directory.Delete(".\\Picture", true);
            //解压文件
            FastZip fastZip = new FastZip();
            //fastZip.Password = null; // 压缩密码。null表示无密码。""也是一种密码。 
            fastZip.ExtractZip(fileToUnZip, zipedFolder, "");
        }

        /// <summary>
        /// 指定的数据库文件是否可用
        /// </summary>
        /// <returns></returns>
        public static bool IsDbFileValid(string dbPath)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath);
            connection.Open();
            SQLiteCommand readTableCmd = connection.CreateCommand();
            readTableCmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table'";
            SQLiteDataReader reader = readTableCmd.ExecuteReader();
            List<string> tableList = new List<string>();
            while (reader.Read())
            {
                tableList.Add(reader.GetString(0));
                Log.Err(TAG, reader.GetString(0));
            }
            //如果不包含这几个表名---返回false
            if (!tableList.Contains(DbCons.TABLE_PROJECT) || !tableList.Contains(DbCons.TABLE_MARKER))
            {
                connection.Close();
                return false;
            }
            connection.Close();
            return true;
        }

        /// <summary>
        /// 获取Project的List<String>
        /// </summary>
        public List<string> GetProjectStrList()
        {
            //首先读取project表数据(这里需要判断一下数据库文件是不是空的)
            SQLiteCommand readCmd = connection.CreateCommand();
            readCmd.CommandText = "SELECT prjName FROM Projects";
            SQLiteDataReader reader = readCmd.ExecuteReader();
            //将reader中的数据填充到list中去
            List<string> list = new List<string>();
            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
            return list;
        }

        /// <summary>
        /// 获取数据库中---所有的---的核心数据
        /// </summary>
        /// <returns></returns>
        public List<DbBean> GetDbData()
        {
            //读取所有的Markers表中的数据
            SQLiteCommand readCmd = connection.CreateCommand();
            readCmd.CommandText = "SELECT * FROM Markers";
            SQLiteDataReader reader = readCmd.ExecuteReader();
            //将数据填充到List中
            List<DbBean> list = new List<DbBean>();
            while (reader.Read())
            {
                //填充数据
                DbBean bean = new DbBean();
                bean.PrjName = reader[DbCons.COLUMN_PRJNAME] + "";
                bean.Longitude = reader[DbCons.COLUMN_LONGITUDE] + "";
                bean.Latitude = reader[DbCons.COLUMN_LATITUDE] + "";
                bean.DeviceType = reader[DbCons.COLUMN_DEVICE_TYPE] + "";
                bean.KilometerMark = reader[DbCons.COLUMN_KILOMETER_MARK] + "";
                bean.SideDirection = reader[DbCons.COLUMN_SIDE_DIRECTION] + "";
                bean.DistanceToRail = reader[DbCons.COLUMN_DISTANCE_TO_RAIL] + "";
                bean.PhotoPathName = reader[DbCons.COLUMN_PHOTO_PATH_NAME] + "";
                bean.Comment = reader[DbCons.COLUMN_COMMENT] + "";
                bean.TowerType = reader[DbCons.COLUMN_TOWER_TYPE] + "";
                bean.TowerHeight = reader[DbCons.COLUMN_TOWER_HEIGHT] + "";
                bean.AntennaDirection1 = reader[DbCons.COLUMN_ANTENNA_DIRECTION_1] + "";
                bean.AntennaDirection2 = reader[DbCons.COLUMN_ANTENNA_DIRECTION_2] + "";
                bean.AntennaDirection3 = reader[DbCons.COLUMN_ANTENNA_DIRECTION_3] + "";
                bean.AntennaDirection4 = reader[DbCons.COLUMN_ANTENNA_DIRECTION_4] + "";
                //添加数据
                list.Add(bean);
            }
            return list;
        }

        /// <summary>
        /// 获取指定prjName的核心数据
        /// </summary>
        /// <param name="prjName"></param>
        /// <returns></returns>
        public List<DbBean> GetSpecificDbData(string prjName)
        {
            //读取所有的Markers表中的数据
            SQLiteCommand readCmd = connection.CreateCommand();
            readCmd.CommandText = "SELECT * FROM Markers WHERE prjName=" + "'" + prjName + "'";
            SQLiteDataReader reader = readCmd.ExecuteReader();
            //将数据填充到List中
            List<DbBean> list = new List<DbBean>();
            while (reader.Read())
            {
                //填充数据
                DbBean bean = new DbBean();
                bean.PrjName = reader[DbCons.COLUMN_PRJNAME] + "";
                bean.Longitude = reader[DbCons.COLUMN_LONGITUDE] + "";
                bean.Latitude = reader[DbCons.COLUMN_LATITUDE] + "";
                bean.DeviceType = reader[DbCons.COLUMN_DEVICE_TYPE] + "";
                bean.KilometerMark = reader[DbCons.COLUMN_KILOMETER_MARK] + "";
                bean.SideDirection = reader[DbCons.COLUMN_SIDE_DIRECTION] + "";
                bean.DistanceToRail = reader[DbCons.COLUMN_DISTANCE_TO_RAIL] + "";
                bean.PhotoPathName = reader[DbCons.COLUMN_PHOTO_PATH_NAME] + "";
                bean.Comment = reader[DbCons.COLUMN_COMMENT] + "";
                bean.TowerType = reader[DbCons.COLUMN_TOWER_TYPE] + "";
                bean.TowerHeight = reader[DbCons.COLUMN_TOWER_HEIGHT] + "";
                bean.AntennaDirection1 = reader[DbCons.COLUMN_ANTENNA_DIRECTION_1] + "";
                bean.AntennaDirection2 = reader[DbCons.COLUMN_ANTENNA_DIRECTION_2] + "";
                bean.AntennaDirection3 = reader[DbCons.COLUMN_ANTENNA_DIRECTION_3] + "";
                bean.AntennaDirection4 = reader[DbCons.COLUMN_ANTENNA_DIRECTION_4] + "";
                //添加数据
                list.Add(bean);
            }
            return list;
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseDb()
        {
            connection.Close();
        }
    }
}