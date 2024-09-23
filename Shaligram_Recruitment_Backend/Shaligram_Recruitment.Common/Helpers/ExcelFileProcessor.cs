using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using Shaligram_Recruitment.Model.ViewModels.ExcelModel;
namespace Shaligram_Recruitment.Common.Helpers
{
    public sealed class ExcelFileProcessor
    {
        private readonly IList<string> _requiredColumns;

        public ExcelFileProcessor(IList<string> requiredColumns)
        {
            _requiredColumns = requiredColumns;
        }

        public DataTable ProcessFile(IFormFile excelFile)
        {
            DataTable dtExcelTable = new DataTable();
            List<ExcelInsertExployeeModel> list = new List<ExcelInsertExployeeModel>();
            ExcelInsertExployeeModel model = new ExcelInsertExployeeModel();
            try
            {
                var fileExtension = Path.GetExtension(excelFile.FileName);
                if (fileExtension.ToLower() == ".csv")
                {
                    using (var csvReader = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(excelFile.OpenReadStream()), true))
                    {
                        dtExcelTable.Load((IDataReader)csvReader);
                    }

                    if (_requiredColumns.Count != dtExcelTable.Columns.Count)
                    {
                        dtExcelTable.TableName = ErrorMessages.NumberOfColumnsInvalid;
                        return dtExcelTable;
                    }
                    if (IsMissingRequiredColumns(dtExcelTable))
                    {
                        dtExcelTable.TableName = ErrorMessages.ColumnNameMismatch;
                        return dtExcelTable;
                    }
                    if (IsColumnSequenceMissing(dtExcelTable))
                    {
                        dtExcelTable.TableName = ErrorMessages.ColumnSequenceMissing;
                        return dtExcelTable;
                    }
                    for (var i = 0; i < dtExcelTable.Rows.Count; i++)
                    {
                        if (!new EmailAddressAttribute().IsValid(dtExcelTable.Rows[i].ItemArray[1].ToString()))
                        {
                            model.EmployeeName = dtExcelTable.Rows[i].ItemArray[0].ToString();
                            model.EmailId = dtExcelTable.Rows[i].ItemArray[1].ToString();
                            list.Add(model);
                        }
                    }
                    if (list != null && list.Count > 0) // Returning List if Emails are Invalid in Sheet
                    {
                        dtExcelTable.Rows.Clear();
                        var dr = dtExcelTable.NewRow();
                        for (var k = 0; k < list.Count(); k++)
                        {
                            dr[0] = list[k].EmployeeName;
                            dr[1] = list[k].EmailId;
                        }
                        dtExcelTable.Rows.Add(dr);
                        dtExcelTable.TableName = ErrorMessages.InvalidEmailAddress;
                        return dtExcelTable;
                    }
                }
                else
                {
                    var sh = GetFileStream(excelFile);
                    //var dtExcelTable = new DataTable();
                    dtExcelTable.Rows.Clear();
                    dtExcelTable.Columns.Clear();
                    var headerRow = sh.GetRow(0);
                    int colCount = headerRow.LastCellNum;
                    for (var c = 0; c < colCount; c++)
                        dtExcelTable.Columns.Add(headerRow.GetCell(c).ToString().Trim());
                    if (_requiredColumns.Count != dtExcelTable.Columns.Count)
                    {
                        dtExcelTable.TableName = ErrorMessages.NumberOfColumnsInvalid;
                        return dtExcelTable;
                    }
                    if (IsMissingRequiredColumns(dtExcelTable))
                    {
                        dtExcelTable.TableName = ErrorMessages.ColumnNameMismatch;
                        return dtExcelTable;
                    }
                    if (IsColumnSequenceMissing(dtExcelTable))
                    {
                        dtExcelTable.TableName = ErrorMessages.ColumnSequenceMissing;
                        return dtExcelTable;
                    }
                    var i = 1;
                    var currentRow = sh.GetRow(i);
                    while (currentRow != null)
                    {
                        var dr = dtExcelTable.NewRow();
                        for (var j = 0; j < headerRow.Cells.Count; j++)
                        {

                            var cell = currentRow.GetCell(j);

                            // Invalid Email Check
                            if (j == 1)
                            {

                                if (!new EmailAddressAttribute().IsValid(cell.RichStringCellValue.String))
                                {
                                    model.EmployeeName = currentRow.Cells[0].ToString();
                                    model.EmailId = currentRow.Cells[1].ToString();
                                    list.Add(model);
                                }
                                else
                                {
                                    dr[j] = cell;
                                }
                            }
                            else
                            {
                                dr[j] = cell;
                            }
                        }
                        dtExcelTable.Rows.Add(dr);
                        i++;
                        currentRow = sh.GetRow(i);
                    }
                    if (list != null && list.Count > 0) // Returning List if Emails are Invalid in Sheet
                    {
                        sh = GetFileStream(excelFile);
                        dtExcelTable.Rows.Clear();
                        dtExcelTable.Columns.Clear();
                        headerRow = sh.GetRow(0);
                        colCount = headerRow.LastCellNum;
                        for (var c = 0; c < colCount; c++)
                            dtExcelTable.Columns.Add(headerRow.GetCell(c).ToString().Trim());
                        var dr = dtExcelTable.NewRow();
                        for (var k = 0; k < list.Count(); k++)
                        {
                            dr[0] = list[k].EmployeeName;
                            dr[1] = list[k].EmailId;
                        }
                        dtExcelTable.Rows.Add(dr);
                        dtExcelTable.TableName = ErrorMessages.InvalidEmailAddress;
                        return dtExcelTable;
                    }
                }

                var res = dtExcelTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull || (field as string) == null || String.CompareOrdinal((field as string).Trim(), string.Empty) == 0));

                if (res.Count() > 0)
                {
                    dtExcelTable = res.CopyToDataTable();
                }
                else
                {
                    dtExcelTable = new DataTable();
                }
                return dtExcelTable;
            }
            finally
            {
                dtExcelTable?.Dispose();
            }
        }

        private bool IsMissingRequiredColumns(DataTable table)
        {
            return _requiredColumns.Any(requiredColumn => !table.Columns.Contains(requiredColumn));
        }

        private bool IsColumnSequenceMissing(DataTable table)
        {
            for (int i = 0; i < _requiredColumns.Count; i++)
            {
                if (_requiredColumns[i] != table.Columns[i].ToString())
                    return true;
            }
            return false;
        }

        private static ISheet GetFileStream(IFormFile excelFile)
        {
            var fileExtension = Path.GetExtension(excelFile.FileName);
            string sheetName;
            ISheet sheet = null;
            Stream stream = excelFile.OpenReadStream();
            switch (fileExtension.ToLower())
            {
                case ".xlsx":
                    var xwb = new XSSFWorkbook(stream);
                    sheetName = xwb.GetSheetAt(0).SheetName;
                    sheet = (XSSFSheet)xwb.GetSheet(sheetName);
                    break;
                case ".xls":
                    var wb = new HSSFWorkbook(stream);
                    sheetName = wb.GetSheetAt(0).SheetName;
                    sheet = (HSSFSheet)wb.GetSheet(sheetName);
                    break;
                default:
                    throw new Exception("File type not accepted");
            }
            return sheet;
        }

    }
}
