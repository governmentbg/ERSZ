using ERSZ.Infrastructure.Data.Models.Nomenclatures;

namespace ERSZ.Infrastructure.Contracts
{
    public interface ICommonLocationEntity : IObjectParentNomenclature
    {
        string Longitude { get; set; }

        string Latitude { get; set; }

        string CityCode { get; set; }

        ICommonNomenclature EntityType { get; }
    }
}
