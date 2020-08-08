using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PPE.Contracts.Response;
using PPE.Data;
using PPE.DomainObjects.PPE;
using PPE.Repository.Interface;
using PPE.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Repository.Implement
{
    public class AssetClassificationService : IAssetClassificationService
    {
        private readonly DataContext _dataContext;
        private readonly IFinanceServerRequest _financeRequest;
        public AssetClassificationService(DataContext dataContext, IFinanceServerRequest financeRequest)
        {
            _dataContext = dataContext;
            _financeRequest = financeRequest;
        }
        public async Task<bool> AddUpdateAssetClassificationAsync(ppe_assetclassification model)
        {
            try
            {

                if (model.AsetClassificationId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_assetclassification.FindAsync(model.AsetClassificationId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.ppe_assetclassification.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAssetClassificationAsync(int id)
        {
            var itemToDelete = await _dataContext.ppe_assetclassification.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<ppe_assetclassification> GetAssetClassificationByIdAsync(int id)
        {
            return await _dataContext.ppe_assetclassification.FindAsync(id);
        }

        public async Task<IEnumerable<ppe_assetclassification>> GetAllAssetClassificationAsync()
        {
            return await _dataContext.ppe_assetclassification.Where(d => d.Deleted == false).ToListAsync();
        }

        public async Task<bool> UploadAssetClassificationAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                var subGlResponse = await _financeRequest.GetAllSubGlAsync();
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<AssetClassificationObj> uploadedRecord = new List<AssetClassificationObj>();
                if (record.Count() > 0)
                {
                    foreach (var byteItem in record)
                    {
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;

                            for (int i = 2; i <= totalRows; i++)
                            {
                                var item = new AssetClassificationObj
                                {
                                    ClassificationName = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                                    UsefulLifeMin = workSheet.Cells[i, 2].Value != null ? int.Parse(workSheet.Cells[i, 2].Value.ToString()) : 0,
                                    UsefulLifeMax = workSheet.Cells[i, 3].Value != null ? int.Parse(workSheet.Cells[i, 3].Value.ToString()) : 0,
                                    ResidualValue = workSheet.Cells[i, 4].Value != null ? decimal.Parse(workSheet.Cells[i, 4].Value.ToString()) : 0,
                                    Depreciable = workSheet.Cells[i, 5].Value != null ? bool.Parse(workSheet.Cells[i, 5].Value.ToString()) : false,
                                    SubGlAdditionCode = workSheet.Cells[i, 6].Value != null ? workSheet.Cells[i, 6].Value.ToString() : null,
                                    SubGlDepreciationCode = workSheet.Cells[i, 7].Value != null ? workSheet.Cells[i, 7].Value.ToString() : null,
                                    SubGlAccumulatedDepreciationCode = workSheet.Cells[i, 8].Value != null ? workSheet.Cells[i, 8].Value.ToString() : null,
                                    SubGlDisposalCode = workSheet.Cells[i, 9].Value != null ? workSheet.Cells[i, 9].Value.ToString() : null,
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }

                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var SubGlAdditionCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLCode == item.SubGlAdditionCode)?.SubGLId ?? 0;
                        var SubGlDepreciationCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLCode == item.SubGlDepreciationCode)?.SubGLId ?? 0;
                        var SubGlAccumulatedDepreciationCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLCode == item.SubGlAccumulatedDepreciationCode)?.SubGLId ?? 0;
                        var SubGlDisposalCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLCode == item.SubGlDisposalCode)?.SubGLId ?? 0;
                        var category = _dataContext.ppe_assetclassification.Where(x => x.ClassificationName == item.ClassificationName && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.ClassificationName = item.ClassificationName;
                            category.UsefulLifeMin = item.UsefulLifeMin;
                            category.UsefulLifeMax = item.UsefulLifeMax;
                            category.ResidualValue = item.ResidualValue;
                            category.Depreciable = item.Depreciable;
                            category.SubGlAddition = SubGlAdditionCode;
                            category.SubGlDepreciation = SubGlDepreciationCode;
                            category.SubGlAccumulatedDepreciation = SubGlAccumulatedDepreciationCode;
                            category.SubGlDisposal = SubGlDisposalCode;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var assetclassification = new ppe_assetclassification();

                            assetclassification.ClassificationName = item.ClassificationName;
                            assetclassification.UsefulLifeMin = item.UsefulLifeMin;
                            assetclassification.UsefulLifeMax = item.UsefulLifeMax;
                            assetclassification.ResidualValue = item.ResidualValue;
                            assetclassification.Depreciable = item.Depreciable;
                            assetclassification.SubGlAddition = SubGlAdditionCode;
                            assetclassification.SubGlDepreciation = SubGlDepreciationCode;
                            assetclassification.SubGlAccumulatedDepreciation = SubGlAccumulatedDepreciationCode;
                            assetclassification.SubGlDisposal = SubGlAccumulatedDepreciationCode;
                            assetclassification.AsetClassificationId = item.AsetClassificationId;
                            assetclassification.Active = true;
                            assetclassification.Deleted = false;
                            assetclassification.CreatedBy = createdBy;
                            assetclassification.CreatedOn = DateTime.Now;
                            
                            await _dataContext.ppe_assetclassification.AddAsync(assetclassification);
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

        public byte[] GenerateExportAssetClassification()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Classification Name");                                                                            
            dt.Columns.Add("Useful Life (Min)");
            dt.Columns.Add("Useful Life (Max)");
            dt.Columns.Add("Residual Value");
            dt.Columns.Add("Depreciable");
            dt.Columns.Add("SubGL Addition");
            dt.Columns.Add("SubGL Depreciation");
            dt.Columns.Add("SubGL Accumulated Depreciation");
            dt.Columns.Add("SubGL Disposal");
            var subGlResponse = _financeRequest.GetAllSubGlAsync().Result;
            var category = (from a in _dataContext.ppe_assetclassification
                            where a.Deleted == false
                            select new AssetClassificationObj
                            {
                                ClassificationName = a.ClassificationName,
                                UsefulLifeMin = a.UsefulLifeMin,
                                UsefulLifeMax = a.UsefulLifeMax,
                                ResidualValue = a.ResidualValue,
                                Depreciable = a.Depreciable,
                                SubGlAddition = a.SubGlAddition,
                                SubGlDepreciation = a.SubGlDepreciation,
                                SubGlAccumulatedDepreciation = a.SubGlAccumulatedDepreciation,
                                SubGlDisposal = a.SubGlDisposal,
                            }).ToList();

            if (category.Count() > 0)
            {
                foreach (var res in category)
                {
                    res.SubGlAdditionCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlAddition)?.SubGLCode;
                    res.SubGlDepreciationCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlDepreciation)?.SubGLCode;
                    res.SubGlAccumulatedDepreciationCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlAccumulatedDepreciation)?.SubGLCode;
                    res.SubGlDisposalCode = subGlResponse.subGls.FirstOrDefault(d => d.SubGLId == res.SubGlDisposal)?.SubGLCode;

                }
            }
            foreach (var kk in category)
            {
                var row = dt.NewRow();
                row["Classification Name"] = kk.ClassificationName;
                row["Useful Life (Min)"] = kk.UsefulLifeMin;
                row["Useful Life (Max)"] = kk.UsefulLifeMax;
                row["Residual Value"] = kk.ResidualValue;
                row["Depreciable"] = kk.Depreciable;
                row["SubGL Addition"] = kk.SubGlAdditionCode;
                row["SubGL Depreciation"] = kk.SubGlDepreciationCode;
                row["SubGL Accumulated Depreciation"] = kk.SubGlAccumulatedDepreciationCode;
                row["SubGL Disposal"] = kk.SubGlDisposalCode;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (category != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Asset Classification");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
    }
}