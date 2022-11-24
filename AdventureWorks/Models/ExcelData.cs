using System.Data;

namespace AdventureWorks.Models
{
    public class ExcelData
    {
        internal DataSet Get_DataPrivate(string path)
        {
            DataSet result = new DataSet();

            OfficeOpenXml.ExcelPackage xlsPackage = new OfficeOpenXml.ExcelPackage(new FileInfo(path));
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            OfficeOpenXml.ExcelWorkbook workBook = xlsPackage.Workbook;

            try
            {
                for (int count = 1; count <= workBook.Worksheets.Count; count++)
                {
                    OfficeOpenXml.ExcelWorksheet wsworkSheet = workBook.Worksheets[0];

                    DataTable tbl = new DataTable();

                    foreach (var firstRowCell in wsworkSheet.Cells[1, 1, 1, wsworkSheet.Dimension.End.Column]) tbl.Columns.Add(firstRowCell.Text);

                    for (int rowNum = 2; rowNum <= wsworkSheet.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = wsworkSheet.Cells[rowNum, 1, rowNum, wsworkSheet.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }

                    tbl.TableName = wsworkSheet.Name;

                    result.Tables.Add(tbl);
                }
                return result;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, $"Repository.ReadExcel : {ex.Message}");
                return null;
            }
        }
    }
}
