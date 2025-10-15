using Baz.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baz.Model.Entity;
using Baz.Repository.Pattern;
using Baz.Mapper.Pattern;
using Microsoft.Extensions.Logging;

namespace Baz.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMesajAkisIcerikService : IService<MesajAkisIcerik>
    {

    }
    
    
    /// <summary>
    /// 
    /// </summary>
    public class MesajAkisIcerikService : Service<MesajAkisIcerik>, IMesajAkisIcerikService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public MesajAkisIcerikService(IRepository<MesajAkisIcerik> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<MesajAkisIcerikService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {

        }
    }


}