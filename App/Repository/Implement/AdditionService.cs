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
    public class AdditionService : IAdditionService
    {
        private readonly DataContext _dataContext;
        public AdditionService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> AddUpdateAdditionAsync(ppe_additionform model)
        {
            try
            {

                if (model.AdditionFormId > 0)
                {
                    var itemToUpdate = await _dataContext.ppe_additionform.FindAsync(model.AdditionFormId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.ppe_additionform.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAdditionAsync(int id)
        {
            var itemToDelete = await _dataContext.ppe_additionform.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<ppe_additionform> GetAdditionByIdAsync(int id)
        {
            return await _dataContext.ppe_additionform.FindAsync(id);
        }

        public async Task<IEnumerable<ppe_additionform>> GetAllAdditionAsync()
        {
            return await _dataContext.ppe_additionform.Where(d => d.Deleted == false).ToListAsync();
        }

        public async Task<bool> UploadAdditionAsync(byte[] record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                List<ppe_additionform> uploadedRecord = new List<ppe_additionform>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new ppe_additionform
                        {
                            LpoNumber = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null,
                            //DateOfPurchase = workSheet.Cells[i, 2].Value != null ? DateTime.Parse(workSheet.Cells[i, 2].Value.ToString()) : ,
                            Description = workSheet.Cells[i, 3].Value != null ? workSheet.Cells[i, 3].Value.ToString() : null,
                            Quantity = workSheet.Cells[i, 4].Value != null ? int.Parse(workSheet.Cells[i, 4].Value.ToString()) : 0,
                            Cost = workSheet.Cells[i, 5].Value != null ? decimal.Parse(workSheet.Cells[i, 5].Value.ToString()) : 0,
                            SubGlAddition = workSheet.Cells[i, 6].Value != null ? int.Parse(workSheet.Cells[i, 6].Value.ToString()) : 0,
                            Location = workSheet.Cells[i, 7].Value != null ? workSheet.Cells[i, 7].Value.ToString() : null,
                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var category = _dataContext.ppe_additionform.Where(x => x.LpoNumber == item.LpoNumber && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.LpoNumber = item.LpoNumber;
                            category.DateOfPurchase = item.DateOfPurchase;
                            category.Description = item.Description;
                            category.Quantity = item.Quantity;
                            category.Cost = item.Cost;
                            category.SubGlAddition = item.SubGlAddition;
                            category.Location = item.Location;
                            category.Active = true;
                            category.Deleted = false;
                            category.UpdatedBy = createdBy;
                            category.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var structure = new ppe_additionform
                            {
                                LpoNumber = item.LpoNumber,
                                DateOfPurchase = item.DateOfPurchase,
                                Description = item.Description,
                                Quantity = item.Quantity,
                                Cost = item.Cost,
                                SubGlAddition = item.SubGlAddition,
                                Location = item.Location,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.ppe_additionform.AddAsync(structure);
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

        public byte[] GenerateExportAddition()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Lpo Number");
            dt.Columns.Add("Date Of Purchase");
            dt.Columns.Add("Description");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Cost");
            dt.Columns.Add("SubGl Addition");
            dt.Columns.Add("Location");
            var category = (from a in _dataContext.ppe_additionform
                            where a.Deleted == false
                            select new ppe_additionform
                            {
                                LpoNumber = a.LpoNumber,
                                DateOfPurchase = a.DateOfPurchase,
                                Description = a.Description,
                                Quantity = a.Quantity,
                                Cost = a.Cost,
                                SubGlAddition = a.SubGlAddition,
                                Location = a.Location,
                            }).ToList();
            foreach (var kk in category)
            {
                var row = dt.NewRow();
                row["Lpo Number"] = kk.LpoNumber;
                row["Date Of Purchase"] = kk.DateOfPurchase;
                row["Description"] = kk.Description;
                row["Quantity"] = kk.Quantity;
                row["Cost"] = kk.Cost;
                row["SubGl Addition"] = kk.SubGlAddition;
                row["Location"] = kk.Location;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (category != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Addition Form");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
    }
}
