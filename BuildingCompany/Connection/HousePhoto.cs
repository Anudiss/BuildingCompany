//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BuildingCompany.Connection
{
    using System;
    using System.Collections.Generic;
    
    public partial class HousePhoto
    {
        public int ID { get; set; }
        public int House_id { get; set; }
        public byte[] Photo { get; set; }
    
        public virtual House House { get; set; }
    }
}
