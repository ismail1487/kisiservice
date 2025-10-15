using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Baz.Service
{
    /// <summary>
    /// Kişi eğitim bilgilerinin servis sınıfıdır
    /// </summary>
    public interface IKisiEgitimBilgileriService : IService<KisiEgitimBilgileri>
    {
        /// <summary>
        /// Id ile kişinin eğitim bilgilerini getiren method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        Result<List<KisiEgitimBilgileri>> KisiIdileGetir(int kisiID);
    }

    /// <summary>
    /// Kişi eğitim bilgilerinin servis sınıfıdır
    /// </summary>
    public class KisiEgitimBilgileriService : Service<KisiEgitimBilgileri>, IKisiEgitimBilgileriService
    {
        /// <summary>
        /// Kişi eğitim bilgilerinin servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public KisiEgitimBilgileriService(IRepository<KisiEgitimBilgileri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<KisiEgitimBilgileriService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

        /// <summary>
        /// Id ile kişinin eğitim bilgilerini getiren method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        public Result<List<KisiEgitimBilgileri>> KisiIdileGetir(int kisiID)
        {
            var result = List(x => x.KisiTemelBilgiId == kisiID);
            return result;
        }
    }
}