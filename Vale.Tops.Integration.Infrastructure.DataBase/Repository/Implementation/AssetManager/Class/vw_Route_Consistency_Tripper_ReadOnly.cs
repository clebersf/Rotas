using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager;

namespace Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class
{
    public class vw_Route_Consistency_Tripper_ReadOnly : ReadOnly<vw_Route_Consistency_Tripper>, Ivw_Route_Consistency_Tripper_ReadOnly
    {
    }
}
