using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PPE.Data;
using PPE.DomainObjects.PPE;
using PPE.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Repository.Implement
{
    public class RegisterService : IRegisterService
    {
        private readonly DataContext _dataContext;
        public RegisterService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> AddUpdateRegisterAsync(ppe_register model)
        {
            try
            {

                if (model.RegisterId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_register.FindAsync(model.RegisterId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.ppe_register.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteRegisterAsync(int id)
        {
            var itemToDelete = await _dataContext.ppe_register.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<ppe_register> GetRegisterByIdAsync(int id)
        {
            return await _dataContext.ppe_register.FindAsync(id);
        }

        public async Task<IEnumerable<ppe_register>> GetAllRegisterAsync()
        {
            return await _dataContext.ppe_register.Where(d => d.Deleted == false).ToListAsync();
        }

        public async Task<bool> UploadRegisterAsync(byte[] record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                List<ppe_register> uploadedRecord = new List<ppe_register>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new ppe_register
                        {
                            AssetNumber = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                            LpoNumber = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null,
                            AssetClassificationId = workSheet.Cells[i, 3].Value != null ? int.Parse(workSheet.Cells[i, 3].Value.ToString()) : 0,
                            Description = workSheet.Cells[i, 4].Value != null ? workSheet.Cells[i, 4].Value.ToString() : null,
                            Cost = workSheet.Cells[i, 5].Value != null ? decimal.Parse(workSheet.Cells[i, 5].Value.ToString()) : 0,
                            DateOfPurchaase = workSheet.Cells[i, 6].Value != null ? DateTime.Parse(workSheet.Cells[i, 6].Value.ToString()) : DateTime.Now,
                            Quantity = workSheet.Cells[i, 7].Value != null ? int.Parse(workSheet.Cells[i, 7].Value.ToString()) : 0,
                            DepreciationStartDate = workSheet.Cells[i, 8].Value != null ? DateTime.Parse(workSheet.Cells[i, 8].Value.ToString()) : DateTime.Now,
                            UsefulLife = workSheet.Cells[i, 9].Value != null ? int.Parse(workSheet.Cells[i, 9].Value.ToString()) : 0,
                            ResidualValue = workSheet.Cells[i, 10].Value != null ? decimal.Parse(workSheet.Cells[i, 10].Value.ToString()) : 0,
                            Location = workSheet.Cells[i, 11].Value != null ? workSheet.Cells[i, 11].Value.ToString() : null,
                            DepreciationForThePeriod = workSheet.Cells[i, 12].Value != null ? decimal.Parse(workSheet.Cells[i, 12].Value.ToString()) : 0,
                            AccumulatedDepreciation = workSheet.Cells[i, 13].Value != null ? decimal.Parse(workSheet.Cells[i, 13].Value.ToString()) : 0,
                            NetBookValue = workSheet.Cells[i, 14].Value != null ? decimal.Parse(workSheet.Cells[i, 14].Value.ToString()) : 0,
                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var category = _dataContext.ppe_register.Where(x => x.LpoNumber == item.LpoNumber && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.AssetNumber = item.AssetNumber;
                            category.LpoNumber = item.LpoNumber;
                            category.AssetClassificationId = item.AssetClassificationId;
                            category.Description = item.Description;
                            category.Cost = item.Cost;
                            category.DateOfPurchaase = item.DateOfPurchaase;
                            category.Quantity = item.Quantity;
                            category.DepreciationStartDate = item.DepreciationStartDate;
                            category.UsefulLife = item.UsefulLife;
                            category.ResidualValue = item.ResidualValue;
                            category.Location = item.Location;
                            category.DepreciationForThePeriod = item.DepreciationForThePeriod;
                            category.AccumulatedDepreciation = item.AccumulatedDepreciation;
                            category.NetBookValue = item.NetBookValue;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var structure = new ppe_register
                            {
                                AssetNumber = item.AssetNumber,
                                LpoNumber = item.LpoNumber,
                                AssetClassificationId = item.AssetClassificationId,
                                Description = item.Description,
                                Cost = item.Cost,
                                DateOfPurchaase = item.DateOfPurchaase,
                                Quantity = item.Quantity,
                                DepreciationStartDate = item.DepreciationStartDate,
                                UsefulLife = item.UsefulLife,
                                ResidualValue = item.ResidualValue,
                                Location = item.Location,
                                DepreciationForThePeriod = item.DepreciationForThePeriod,
                                AccumulatedDepreciation = item.AccumulatedDepreciation,
                                NetBookValue = item.NetBookValue,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.ppe_register.AddAsync(structure);
                        }
                    }
                }

                var response = _dataContext.SaveChanges() > 0;
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public byte[] GenerateExportRegister()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Asset Number");
            dt.Columns.Add("LPO Number");
            dt.Columns.Add("Classification");
            dt.Columns.Add("Description");
            dt.Columns.Add("Cost");
            dt.Columns.Add("Date Of Purchase");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Depreciation Start Date");
            dt.Columns.Add("Useful Life");
            dt.Columns.Add("Residual Value");
            dt.Columns.Add("Location");
            dt.Columns.Add("Depreciation For The Period");
            dt.Columns.Add("Accumulated Depreciation");
            dt.Columns.Add("Net Book Value");
            var category = (from a in _dataContext.ppe_register
                            where a.Deleted == false
                            select new ppe_register
                            {
                                RegisterId = a.RegisterId,
                                AssetNumber = a.AssetNumber,
                                LpoNumber = a.LpoNumber,
                                AssetClassificationId = a.AssetClassificationId,
                                Description = a.Description,
                                Cost = a.Cost,
                                DateOfPurchaase = a.DateOfPurchaase,
                                Quantity = a.Quantity,
                                DepreciationStartDate = a.DepreciationStartDate,
                                UsefulLife = a.UsefulLife,
                                ResidualValue = a.ResidualValue,
                                Location = a.Location,
                                DepreciationForThePeriod = a.DepreciationForThePeriod,
                                AccumulatedDepreciation = a.AccumulatedDepreciation,
                                NetBookValue = a.NetBookValue,
                            }).ToList();
            foreach (var kk in category)
            {
                var row = dt.NewRow();
                row["Asset Number"] = kk.AssetNumber;
                row["LPO Number"] = kk.LpoNumber;
                row["Classification"] = kk.AssetClassificationId;
                row["Description"] = kk.Description;
                row["Cost"] = kk.Cost;
                row["Date Of Purchase"] = kk.DateOfPurchaase;
                row["Quantity"] = kk.Quantity;
                row["Depreciation Start Date"] = kk.DepreciationStartDate;
                row["Useful Life"] = kk.UsefulLife;
                row["Residual Value"] = kk.ResidualValue;
                row["Location"] = kk.Location;
                row["Depreciation For The Period"] = kk.DepreciationForThePeriod;
                row["Accumulated Depreciation"] = kk.AccumulatedDepreciation;
                row["Net Book Value"] = kk.NetBookValue;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (category != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Register");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
    }
}