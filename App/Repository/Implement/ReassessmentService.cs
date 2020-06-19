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
    public class ReassessmentService : IReassessmentService
    {
        private readonly DataContext _dataContext;
        public ReassessmentService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> AddUpdateReassessmentAsync(ppe_reassessment model)
        {
            try
            {

                if (model.ReassessmentId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_reassessment.FindAsync(model.ReassessmentId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.ppe_reassessment.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteReassessmentAsync(int id)
        {
            var itemToDelete = await _dataContext.ppe_reassessment.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<ppe_reassessment> GetReassessmentByIdAsync(int id)
        {
            return await _dataContext.ppe_reassessment.FindAsync(id);
        }

        public async Task<IEnumerable<ppe_reassessment>> GetAllReassessmentAsync()
        {
            return await _dataContext.ppe_reassessment.Where(d => d.Deleted == false).ToListAsync();
        }

        public async Task<bool> UploadReassessmentAsync(byte[] record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                List<ppe_reassessment> uploadedRecord = new List<ppe_reassessment>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new ppe_reassessment
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
                            RemainingUsefulLife = workSheet.Cells[i, 15].Value != null ? int.Parse(workSheet.Cells[i, 15].Value.ToString()) : 0,
                            ProposedUsefulLife = workSheet.Cells[i, 16].Value != null ? int.Parse(workSheet.Cells[i, 16].Value.ToString()) : 0,
                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var category = _dataContext.ppe_reassessment.Where(x => x.LpoNumber == item.LpoNumber && x.Deleted == false).FirstOrDefault();
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
                            category.RemainingUsefulLife = item.RemainingUsefulLife;
                            category.ProposedUsefulLife = item.ProposedUsefulLife;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var structure = new ppe_reassessment
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
                                RemainingUsefulLife = item.RemainingUsefulLife,
                                ProposedUsefulLife = item.ProposedUsefulLife,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.ppe_reassessment.AddAsync(structure);
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

        public byte[] GenerateExportReassessment()
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
            dt.Columns.Add("Remaining Useful Life");
            dt.Columns.Add("Proposed Useful Life");
            var category = (from a in _dataContext.ppe_reassessment
                            where a.Deleted == false
                            select new ppe_reassessment
                            {
                                ReassessmentId = a.ReassessmentId,
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
                                RemainingUsefulLife = a.RemainingUsefulLife,
                                ProposedUsefulLife = a.ProposedUsefulLife,
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
                row["Remaining Useful Life"] = kk.RemainingUsefulLife;
                row["Proposed Useful Life"] = kk.ProposedUsefulLife;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (category != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Assessment");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
    }
}