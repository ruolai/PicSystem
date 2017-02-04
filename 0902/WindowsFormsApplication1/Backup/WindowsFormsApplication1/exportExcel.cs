using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace WindowsFormsApplication1
{
    class exportExcel
    {
        /// <summary>
        /// 读取Excel文件到Dataset中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        //public DataSet ToDataTable(string filePath)
        //{
        //    string connStr = "";
        //    string fileName="";
        //    string fileType = Path.GetExtension(fileName);
        //    if (string.IsNullOrEmpty(fileType)) return null;

        //    if (fileType == ".xls")
        //        connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
        //    else
        //        connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";

        //    string Sql_F = "select * from '{0}'";
        //    OleDbConnection conn = null;
        //    OleDbDataAdapter da = null;
        //    DataTable dtSheetName = null;
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        //初始化连接
        //        conn = new OleDbConnection(connStr);
        //        conn.Open();
        //        //获取数据源的表定义数据
        //        string SheetName = "";
        //        dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,new object[]{null,null,null,null,null,null,"TABLE"});
        //        //初始化适配器
        //        return ds;
        //    }
        //    catch { return null; }
        //}

        /// <summary>
        /// Excel某sheet中内容导入到DataTable中
        /// 区分xsl和xslx分别处理
        /// </summary>
        /// <param name="filePath">Excel文件路径,含文件全名</param>
        /// <param name="sheetName">此Excel中sheet名</param>
        /// <returns></returns>
        public DataTable ExcelSheetImportToDataTable(string filePath, string sheetName)
        {
            
            DataTable dt = new DataTable();

            if (Path.GetExtension(filePath).ToLower() == ".xls".ToLower())
            {//.xls
                HSSFWorkbook hssfworkbook;
                #region .xls文件处理:HSSFWorkbook
                try
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {

                        hssfworkbook = new HSSFWorkbook(file);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                ISheet sheet = hssfworkbook.GetSheet(sheetName);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);

                //一行最后一个方格的编号 即总的列数 
                for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
                {
                    //SET EVERY COLUMN NAME
                    HSSFCell cell = (HSSFCell)headerRow.GetCell(j);

                    dt.Columns.Add(cell.ToString());
                }

                while (rows.MoveNext())
                {
                    IRow row = (HSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();

                    if (row.RowNum == 0) continue;//The firt row is title,no need import

                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        if (i >= dt.Columns.Count)//cell count>column count,then break //每条记录的单元格数量不能大于表格栏位数量 20140213
                        {
                            break;
                        }

                        ICell cell = row.GetCell(i);

                        if ((i == 0) && (string.IsNullOrEmpty(cell.ToString()) == true))//每行第一个cell为空,break
                        {
                            break;
                        }

                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }

                    dt.Rows.Add(dr);
                }
                #endregion
            }
            else
            {//.xlsx
                XSSFWorkbook hssfworkbook;
                #region .xlsx文件处理:XSSFWorkbook
                try
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {

                        hssfworkbook = new XSSFWorkbook(file);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                ISheet sheet = hssfworkbook.GetSheet(sheetName);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                XSSFRow headerRow = (XSSFRow)sheet.GetRow(0);



                //一行最后一个方格的编号 即总的列数 
                for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
                {
                    //SET EVERY COLUMN NAME
                    XSSFCell cell = (XSSFCell)headerRow.GetCell(j);

                    dt.Columns.Add(cell.ToString());

                }

                while (rows.MoveNext())
                {
                    IRow row = (XSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();

                    if (row.RowNum == 0) continue;//The firt row is title,no need import

                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        if (i >= dt.Columns.Count)//cell count>column count,then break //每条记录的单元格数量不能大于表格栏位数量 20140213
                        {
                            break;
                        }

                        ICell cell = row.GetCell(i);

                        if ((i == 0) && (string.IsNullOrEmpty(cell.ToString()) == true))//每行第一个cell为空,break
                        {
                            break;
                        }

                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
                #endregion
            }
            return dt;
        }
    }
}
