using BuildingCompany.Connection;
using BuildingCompany.Model.Entities;
using BuildingCompany.Model.ModelBases;
using System.Data.Entity;

namespace BuildingCompany.Model.Collections
{
    public class HouseCollection : CollectionModelBase<HouseModel>
    {
        static HouseCollection() => DatabaseContext.Entities.House.Load();
    }
}
