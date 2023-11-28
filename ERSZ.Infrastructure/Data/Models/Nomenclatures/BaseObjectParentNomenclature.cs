using ERSZ.Infrastructure.Contracts;

namespace ERSZ.Infrastructure.Data.Models.Nomenclatures
{
    public class BaseObjectParentNomenclature : BaseCommonNomenclature, IObjectParentNomenclature
    {
        public int ObjectId { get; set; }

        public int? ParentId { get; set; }

        public int? ParentObjectId { get; set; }
    }
}
