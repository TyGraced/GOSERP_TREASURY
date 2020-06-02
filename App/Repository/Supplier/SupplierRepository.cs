using GODP.APIsContinuation.DomainObjects.Supplier;
using GODP.APIsContinuation.Repository.Interface;
using GOSLibraries.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Puchase_and_payables.AuthHandler;
using Puchase_and_payables.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Repository.Inplimentation
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        public SupplierRepository(DataContext dataContext, IIdentityService identityService)
        {
            _identityService = identityService;
            _dataContext = dataContext;
        }

        public async Task<bool> AddNewSupplierAsync(cor_supplier model)
        {
            await _dataContext.cor_supplier.AddAsync(model);
            var added = await _dataContext.SaveChangesAsync();
            
            return added > 0;
            
        }

        public async Task<bool> DeleteSupplierAsync(cor_supplier model)
        {   
            var item = await _dataContext.cor_supplier.FindAsync(model.SupplierId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSupplierAuthorizationAsync(cor_supplierauthorization model)
        {
            var item = await _dataContext.cor_supplier.FindAsync(model.SupplierId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSupplierBusinessOwnerAsync(cor_supplierbusinessowner model)
        {
            var item = await _dataContext.cor_supplier.FindAsync(model.SupplierId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSupplierDocumentAsync(cor_supplierdocument model)
        {
            var item = await _dataContext.cor_supplier.FindAsync(model.SupplierId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSupplierTopClientAsync(cor_topclient model)
        {
            var item = await _dataContext.cor_supplier.FindAsync(model.SupplierId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSupplierTopSupplierAsync(cor_topsupplier model)
        {
            var item = await _dataContext.cor_supplier.FindAsync(model.SupplierId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }


        public async Task<IEnumerable<cor_supplier>> GetAllSupplierAsync()
        {
            var queryAble =  _dataContext.cor_supplier.AsQueryable();
            var suppliers =  await queryAble.Where(m => m.Deleted == false)
                .Include(x => x.cor_supplierauthorization)
                .Include(x => x.cor_supplierbusinessowner)
                .Include(x => x.cor_supplierdocument)
                .Include(x => x.cor_suppliertype)
                .Include(x => x.cor_topsupplier).ToListAsync();

            return suppliers;
        }

        public async Task<IEnumerable<cor_supplierauthorization>> GetAllSupplierAuthorizationAsync()
        {
            return await _dataContext.cor_supplierauthorization.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_supplierbusinessowner>> GetAllSupplierBusinessOwnerAsync()
        {
            return await _dataContext.cor_supplierbusinessowner.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_supplierdocument>> GetAllSupplierDocumentAsync()
        {
            return await _dataContext.cor_supplierdocument.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_topclient>> GetAllSupplierTopClientAsync()
        {
            return await _dataContext.cor_topclient.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_topsupplier>> GetAllSupplierTopSupplierAsync()
        {
            return await _dataContext.cor_topsupplier.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<cor_supplier> GetSupplierAsync(int supplierId)
        {
            return await _dataContext.cor_supplier.SingleOrDefaultAsync(x => x.SupplierId == supplierId && x.Deleted == false);
        }

        public async Task<cor_supplierauthorization> GetSupplierAuthorizationAsync(int supplierAuthorizationId)
        {
            return await _dataContext.cor_supplierauthorization.SingleOrDefaultAsync(x => x.SupplierAuthorizationId == supplierAuthorizationId
             && x.Deleted == false);
        }

        public async Task<cor_supplierbusinessowner> GetSupplierBusinessOwnerAsync(int supplierBusinessOwnerId)
        {
            return await _dataContext.cor_supplierbusinessowner.SingleOrDefaultAsync(x => x.SupplierBusinessOwnerId == supplierBusinessOwnerId
             && x.Deleted == false);
        }

        public async Task<cor_supplierdocument> GetSupplierDocumentAsync(int supplierDocumentId)
        {
            return await _dataContext.cor_supplierdocument.SingleOrDefaultAsync(x => x.SupplierDocumentId == supplierDocumentId
             && x.Deleted == false);
        }

        public async Task<IEnumerable<cor_supplier>> GetSuppliersByCountryAsync(int countryId)
        {
            return await _dataContext.cor_supplier.Where(x => x.CountryId == countryId && x.Deleted == false).ToListAsync();
        }

        public async Task<cor_topclient> GetSupplierTopClientAsync(int supplierTopClientId)
        {
            return await _dataContext.cor_topclient.SingleOrDefaultAsync(x => x.TopClientId == supplierTopClientId && x.Deleted == false);
        }

        public async Task<cor_topsupplier> GetSupplierTopSupplierAsync(int supplierTopSupplierId)
        {
            return await _dataContext.cor_topsupplier.SingleOrDefaultAsync(x => x.TopSupplierId == supplierTopSupplierId && x.Deleted == false);
        }

        
        public async Task<IEnumerable<cor_supplier>> SupplierInformationAwaitingApprovalAsync(string userName)
        {
            var userDetail = await _identityService.UserDataAsync(); 
            var thisUserSupplierData = await _dataContext.cor_supplier.Where(x => x.Email == userDetail.Email).ToListAsync();

            return thisUserSupplierData.Where(x => x.ApprovalStatusId == (int)ApprovalStatus.Pending
            && x.ApprovalStatusId == (int)ApprovalStatus.Processing);

        }

        public async  Task<IEnumerable<cor_supplier>> SupplierSearchAsync()
        {
            return await _dataContext.cor_supplier.ToListAsync();
        }

        public async Task<bool> UpdateSupplierAsync(cor_supplier model)
        {
            var supplier = await _dataContext.cor_supplier.FindAsync(model.SupplierId);
            _dataContext.Entry(supplier).CurrentValues.SetValues(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSupplierAuthorizationAsync(cor_supplierauthorization model)
        {
            if(model.SupplierAuthorizationId > 0)
            {
                var itemTUpdate = await GetSupplierAuthorizationAsync(model.SupplierAuthorizationId);
                _dataContext.Entry(itemTUpdate).CurrentValues.SetValues(model);
            }
            await _dataContext.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSupplierBusinessOwnerAsync(cor_supplierbusinessowner model)
        {
            if (model.SupplierBusinessOwnerId > 0)
            {
                var itemTUpdate = await GetSupplierBusinessOwnerAsync(model.SupplierBusinessOwnerId);
                _dataContext.Entry(itemTUpdate).CurrentValues.SetValues(model);
            }
            await _dataContext.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSupplierDocumentAsync(cor_supplierdocument model)
        {
            if (model.SupplierDocumentId > 0)
            {
                var itemTUpdate = await GetSupplierDocumentAsync(model.SupplierDocumentId);
                _dataContext.Entry(itemTUpdate).CurrentValues.SetValues(model);
            }
            await _dataContext.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSupplierTopClientAsync(cor_topclient model)
        {
            if (model.TopClientId > 0)
            {
                var itemTUpdate = await GetSupplierTopClientAsync(model.TopClientId);
                _dataContext.Entry(itemTUpdate).CurrentValues.SetValues(model);
            }
            await _dataContext.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSupplierTopSupplierAsync(cor_topsupplier model)
        {
            if (model.TopSupplierId > 0)
            {
                var itemTUpdate = await GetSupplierTopSupplierAsync(model.TopSupplierId);
                _dataContext.Entry(itemTUpdate).CurrentValues.SetValues(model);
            }
            await _dataContext.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        //public int CountryIdByCountryName(string countryName)
        //{
        //    return _dataContext.cor_country.FirstOrDefault(x => x.CountryName.Trim().ToLower() == countryName.Trim().ToLower()).CountryId;
        //}
        //public int SupplierTypeBySupplierTypeName(string countryName)
        //{
        //    return _dataContext.cor_country.FirstOrDefault(x => x.CountryName.Trim().ToLower() == countryName.Trim().ToLower()).CountryId;
        //}
        public async Task<bool> UploadSupplierListAsync(byte[] record, string createdBy)
        {
            try
            {
                var updated = 0;
                var uploadedRecord = new List<cor_supplier>();
                using (MemoryStream stream = new MemoryStream(record))

                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new cor_supplier
                        {
                            Name = workSheet.Cells[i, 1].Value.ToString(),
                            //SupplierTypeId = SupplierTypeBySupplierTypeName(workSheet.Cells[i, 2].Value.ToString()),
                            Address = workSheet.Cells[i, 3].Value.ToString(),
                            Email = workSheet.Cells[i, 2].Value.ToString(),
                            PostalAddress = workSheet.Cells[i, 3].Value.ToString(),
                            //CountryId = CountryIdByCountryName(workSheet.Cells[i, 2].Value.ToString()),
                            PhoneNo = workSheet.Cells[i, 1].Value.ToString(),
                            RegistrationNo = workSheet.Cells[i, 1].Value.ToString(),
                            Active = true,
                            Deleted = false,
                            CreatedBy = createdBy,
                            CreatedOn = DateTime.Now,
                            UpdatedBy = createdBy,
                            UpdatedOn = DateTime.Now,
                        });
                    }

                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var supplierFrmDB = await _dataContext.cor_supplier.FirstOrDefaultAsync(x => (
                        x.Name.Trim().ToLower() == item.Name.Trim().ToLower() ||
                        x.SupplierNumber.Trim().ToLower() == item.Name.Trim().ToLower() ||
                        x.Email.ToLower().Trim() == item.Email.ToLower().Trim()) && x.Deleted == false);

                        if (supplierFrmDB != null)
                        {
                            supplierFrmDB.SupplierNumber = item.SupplierNumber;
                            supplierFrmDB.Name = item.Name;
                            supplierFrmDB.TaxIDorVATID = item.TaxIDorVATID;
                            supplierFrmDB.RegistrationNo = item.RegistrationNo;
                            supplierFrmDB.PostalAddress = item.PostalAddress;
                            supplierFrmDB.Address = item.Address;
                            supplierFrmDB.PhoneNo = item.PhoneNo;
                            supplierFrmDB.Email = item.Email;
                            supplierFrmDB.CountryId = item.CountryId;
                            supplierFrmDB.Active = true;
                            supplierFrmDB.Deleted = false;
                            supplierFrmDB.CreatedBy = createdBy;
                            supplierFrmDB.CreatedOn = DateTime.Now;
                            supplierFrmDB.UpdatedBy = createdBy;
                            supplierFrmDB.UpdatedOn = DateTime.Now;
                            _dataContext.Entry(supplierFrmDB).State = EntityState.Modified;
                        }
                        else
                        {
                            var newSupplier = new cor_supplier
                            {
                                SupplierNumber = item.SupplierNumber,
                                Name = item.Name,
                                TaxIDorVATID = item.TaxIDorVATID,
                                RegistrationNo = item.RegistrationNo,
                                PostalAddress = item.PostalAddress,
                                Address = item.Address,
                                PhoneNo = item.PhoneNo,
                                Email = item.Email,
                                CountryId = item.CountryId,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                                UpdatedBy = createdBy,
                                UpdatedOn = DateTime.Now,
                            };
                            _dataContext.cor_supplier.Add(newSupplier);
                        }
                    }

                    updated = await _dataContext.SaveChangesAsync();

                }
                return updated > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
