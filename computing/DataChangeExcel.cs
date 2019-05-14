using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace computing
{
    class DataChangeExcel
    {
        public static void DataSetToExcel(DataTable dataTable,string SaveFile)
        {
            Application excel;
            _Workbook workbook;
            _Worksheet worksheet;
            object misValue = System.Reflection.Missing.Value;
            excel = new Application();
            workbook = excel.Workbooks.Add(misValue);
            worksheet = (_Worksheet)workbook.ActiveSheet;
            int rowIndex = 1;
            int colIndex = 0;
            //取得标题
            foreach(DataColumn col in dataTable.Columns)
            {
                colIndex++;
                excel.Cells[1, colIndex] = col.ColumnName;
            }
            //取得表格中的数据
            foreach(DataRow row in dataTable.Rows)
            {
                rowIndex++;
                colIndex = 0;
                foreach(DataColumn col in dataTable.Columns)
                {
                    colIndex++;
                    excel.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString().Trim();
                }
            }
            excel.Visible = false;
            workbook.SaveAs(SaveFile, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            dataTable = null;
            workbook.Close(true, misValue, misValue);
            excel.Quit();
            PublicMethod.Kill(excel);
            releaseObject(worksheet);
            releaseObject(workbook);
            releaseObject(excel);
        }

        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }catch
            {
                obj = null;
            }finally
            {
                GC.Collect();
            }
        }
    }
}
