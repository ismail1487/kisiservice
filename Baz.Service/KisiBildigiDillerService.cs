using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Baz.Service
{
    /// <summary>
    /// Kişi dil bilgileri servis interface'i.
    /// </summary>
    public interface IKisiBildigiDillerService : IService<KisiBildigiDiller>
    {
        Result<List<KisiBildigiDiller>> KisiBildigiDilList(int kisiID);

    }
    public class KisiBildigiDillerService : Service<KisiBildigiDiller>, IKisiBildigiDillerService
    {
        /// <summary>
        ///Şifre yenileme taleplerinin kaydedildiği SistemLoginSifreYenilemeAktivasyonHareketleri tablosu ile ilgili işlemleri içeren servis sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public KisiBildigiDillerService(IRepository<KisiBildigiDiller> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<SistemLoginSifreYenilemeAktivasyonHareketleri> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

        public Result<List<KisiBildigiDiller>> KisiBildigiDilList(int kisiID)
        {
            var bildigiDiller = this.List(x=>x.AktifMi==1&&x.SilindiMi==0&&x.KisiTemelBilgiID==kisiID).Value;
            return bildigiDiller.ToResult();
        }
    }
}