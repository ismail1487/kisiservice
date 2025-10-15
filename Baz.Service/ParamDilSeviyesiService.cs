using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Repository.Pattern;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;

namespace Baz.Service
{
    /// <summary>
    /// Login - Şifre Yenileme - Hesap Aktivasyon  işlemlerini yöneten interface
    /// </summary>
    public interface IParamDilSeviyesiService : IService<ParamDilSeviyesi>
    {
    }

    /// <summary>
    /// Şifre yenileme taleplerinin kaydedildiği SistemLoginSifreYenilemeAktivasyonHareketleri tablosu ile ilgili işlemleri içeren servis classı.
    /// </summary>
    public class ParamDilSeviyesiService : Service<ParamDilSeviyesi>, IParamDilSeviyesiService
    {
        /// <summary>
        ///Şifre yenileme taleplerinin kaydedildiği SistemLoginSifreYenilemeAktivasyonHareketleri tablosu ile ilgili işlemleri içeren servis sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public ParamDilSeviyesiService(IRepository<ParamDilSeviyesi> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<SistemLoginSifreYenilemeAktivasyonHareketleri> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }
    }
}