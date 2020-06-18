using PPE.Data;
using PPE.DomainObjects.PPE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPE.Repository.Interface
{
    public interface IAssetClassificationService
    {
        Task<bool> AddUpdateAssetClassificationAsync(ppe_assetclassification model);

        Task<bool> DeleteAssetClassificationAsync(int id);

        Task<ppe_assetclassification> GetAssetClassificationByIdAsync(int id);

        Task<IEnumerable<ppe_assetclassification>> GetAllAssetClassificationAsync();

        Task<bool> UploadAssetClassificationAsync(byte[] record, string createdBy);

        byte[] GenerateExportAssetClassification();
    }
}
