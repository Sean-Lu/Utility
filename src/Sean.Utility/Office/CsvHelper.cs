using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Sean.Utility.Office
{
    /// <summary>  
    /// Csv文件操作（支持解析逗号、双引号）
    /// </summary> 
    public class CsvHelper
    {
        /// <summary>
        /// 字符编码
        /// </summary>
        public static Encoding Encoding = Encoding.Default;

        private CsvHelper() { }

        /// <summary>
        /// DataTable转Csv
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <param name="csvFilePath">Csv文件路径</param>
        /// <param name="isColumnWritten">DataTable的列名是否要写入</param>
        /// <returns></returns>
        public static void ToCsv(DataTable dt, string csvFilePath, bool isColumnWritten)
        {
            if (!csvFilePath.ToLower().EndsWith(".csv")) csvFilePath += ".csv";

            using (StreamWriter sw = new StreamWriter(csvFilePath, false, Encoding))
            {
                string strRow;
                string parsing;
                bool bFlag;

                //写标题
                if (isColumnWritten)
                {
                    strRow = "";
                    bFlag = true;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        parsing = ParseField(dc.Caption);
                        if (bFlag)
                        {
                            strRow = parsing;
                            bFlag = false;
                            continue;
                        }
                        strRow += "," + parsing;
                    }
                    sw.WriteLine(strRow);
                }

                //写内容
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strRow = "";
                    bFlag = true;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        parsing = ParseField(dt.Rows[i][j].ToString());
                        if (bFlag)
                        {
                            strRow = parsing;
                            bFlag = false;
                            continue;
                        }
                        strRow += "," + parsing;
                    }
                    sw.WriteLine(strRow);
                }
            }
        }
        /// <summary>
        /// 将指定文件转换为标准Csv文件
        /// </summary>
        /// <param name="fromFilePath">待转换的文件路径</param>
        /// <param name="toFilePath">转换后的标准Csv文件保存路径</param>
        /// <param name="separator">源文件的字段分隔符</param>
        public static void ToCsv(string fromFilePath, string toFilePath, string separator)
        {
            if (!File.Exists(fromFilePath)) throw new Exception(string.Format("文件【{0}】不存在.", fromFilePath));
            if (string.IsNullOrWhiteSpace(separator)) throw new Exception("分隔符不能为空.");

            if (!toFilePath.ToLower().EndsWith(".csv")) toFilePath += ".csv";

            using (StreamReader sr = new StreamReader(fromFilePath, Encoding))
            using (StreamWriter sw = new StreamWriter(toFilePath, false, Encoding))
            {
                string strLine;
                while ((strLine = sr.ReadLine()) != null)
                {
                    string[] arrayLine = strLine.Split(new string[] { separator }, StringSplitOptions.None);
                    bool bFirst = true;
                    string strRow = "";
                    foreach (string str in arrayLine)
                    {
                        string parsing = ParseField(str);
                        if (bFirst)
                        {
                            strRow = parsing;
                            bFirst = false;
                        }
                        else
                            strRow += "," + parsing;
                    }
                    sw.WriteLine(strRow);
                }
            }
        }

        /// <summary>
        /// Csv转DataTable
        /// </summary>
        /// <param name="csvFilePath">Csv文件路径</param>
        /// <param name="isFirstRowColumn">第一行数据是DataTable的列名还是内容。true表示列名；false表示内容，且列名(字段名)格式为：Col1、Col2...</param>
        /// <returns></returns>
        public static DataTable ToDataTable(string csvFilePath, bool isFirstRowColumn)
        {
            DataTable dt = new DataTable();
            using (FileStream fs = new FileStream(csvFilePath, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs, Encoding))
            {
                string strLine = "";
                //记录每行记录中的各字段内容
                string[] aryLine;
                //标示列数
                int columnCount = 0;
                //标示是否是读取的第一行
                bool IsFirst = true;

                //逐行读取CSV中的数据
                while ((strLine = sr.ReadLine()) != null)
                {
                    aryLine = ParseString(strLine);
                    if (aryLine == null) continue;

                    if (IsFirst)
                    {
                        IsFirst = false;
                        columnCount = aryLine.Length;
                        for (int i = 0; i < columnCount; i++)
                        {
                            string strColumnName = aryLine[i];
                            DataColumn dc = new DataColumn(isFirstRowColumn && !dt.Columns.Contains(strColumnName) ? strColumnName
                                                                                                                   : string.Format("Col{0}", Convert.ToString(i + 1)));
                            dt.Columns.Add(dc);
                        }

                        if (!isFirstRowColumn)
                        {
                            DataRow dr = dt.NewRow();
                            for (int j = 0; j < columnCount; j++)
                            {
                                dr[j] = aryLine[j];
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 插入标题（在第一行添加一条数据）
        /// </summary>
        /// <param name="csvFilePath">Csv文件路径</param>
        /// <param name="insert">插入数据</param>
        public static bool InsertTittle(string csvFilePath, string[] insert)
        {
            if (insert == null || insert.Length <= 0 || !File.Exists(csvFilePath)) return false;

            string strTmp = string.Empty;
            using (FileStream fs = new FileStream(csvFilePath, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs, Encoding))
            {
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                strTmp = sr.ReadToEnd();
            }

            using (FileStream fs = new FileStream(csvFilePath, FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs, Encoding))
            {
                string strRow = string.Empty;
                bool bFlag = true;
                foreach (string str in insert)
                {
                    string parsing = ParseField(str);
                    if (bFlag)
                    {
                        strRow = parsing;
                        bFlag = false;
                        continue;
                    }
                    strRow += "," + parsing;
                }
                strTmp = string.IsNullOrWhiteSpace(strTmp) ? strRow : $"{strRow}{Environment.NewLine}{strTmp}";

                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                sw.Write(strTmp);
                sw.Flush();
            }
            return true;
        }

        /// <summary>
        /// 解析字段（逗号、双引号）
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private static string ParseField(string field)
        {
            if (!string.IsNullOrWhiteSpace(field))
            {
                //如果字段中有逗号（,），该字段使用双引号（"）括起来；
                //如果该字段中有双引号，该双引号前要再加一个双引号，然后把该字段使用双引号括起来。
                if (field.Contains("\"")) field = string.Format("\"{0}\"", field.Replace("\"", "\"\""));
                if (field.Contains(",") && !field.Contains("\"")) field = string.Format("\"{0}\"", field);
            }
            return field;
        }
        /// <summary>
        /// 解析字符串（逗号、双引号）
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string[] ParseString(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;

            string[] arrayLine = null;
            if (!line.Contains("\""))
            {
                arrayLine = line.Split(new char[] { ',' }, StringSplitOptions.None);
            }
            else
            {
                var strItem = "";
                var count = 0;
                var arrayList = new List<string>();
                for (int i = 0; i < line.Length; i++)
                {
                    string strTmp = line.Substring(i, 1);
                    if (strTmp == "\"") count++;
                    if (count >= 2) count = 0;

                    if (strTmp == "," && count == 0)
                    {
                        arrayList.Add(GetOriginalString(strItem));
                        strItem = "";
                    }
                    else
                    {
                        strItem += strTmp;
                    }
                }
                if (strItem.Length > 0)
                {
                    arrayList.Add(GetOriginalString(strItem));
                }
                if (line.EndsWith(",")) arrayList.Add("");
                arrayLine = arrayList.ToArray();
            }
            return arrayLine;
        }
        /// <summary>
        /// 获取原始字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string GetOriginalString(string str)
        {
            if (str.StartsWith("\"") && str.EndsWith("\"") && str.Length >= 2)
                str = str.Substring(1, str.Length - 2).Replace("\"\"", "\"");

            return str;
        }
    }
}
