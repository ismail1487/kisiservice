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
    /// Kişi adres bilgilerinin servis sınıfıdır.
    /// </summary>
    public interface IKisiAdresBilgileriService : IService<KisiAdresBilgileri>
    {
        /// <summary>
        /// Id ile kişilerin getiren metot
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        Result<List<KisiAdresBilgileri>> KisiIdileGetir(int kisiID);

        ///// <summary>
        ///// Kişi Id'ye göre kişi adres bilgilerini silindi yapan metot
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>
        //Result<bool> KisiBilgileriSilindiYap(int kisiID);

        ///// <summary>
        ///// Kişi Id'ye göre kişi adres bilgilerini aktif yapan metot
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>
        //Result<bool> KisiBilgileriAktifYap(int kisiID);
    }

    /// <summary>
    /// Kişi adres bilgilerinin servis sınıfıdır.
    /// </summary>
    public class KisiAdresBilgileriService : Service<KisiAdresBilgileri>, IKisiAdresBilgileriService
    {
        /// <summary>
        /// Kişi adres bilgilerinin servis sınıfının yapıcı metodu.
        /// </summary>
        public KisiAdresBilgileriService(IRepository<KisiAdresBilgileri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<KisiAdresBilgileriService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

        /// <summary>
        /// Id ile kişilerin getiren metot
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        public Result<List<KisiAdresBilgileri>> KisiIdileGetir(int kisiID)
        {
            var result = List(x => x.KisiTemelBilgiId == kisiID);
            return result;
        }

        ///// <summary>
        ///// Kişi Id'ye göre kişi adres bilgilerini silindi yapan metot
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>
        //public Result<bool> KisiBilgileriSilindiYap(int kisiID)
        //{
        //    var list = this.List(x => x.KisiTemelBilgiId == kisiID).Value;

        //    foreach (var item in list)
        //    {
        //        item.SilindiMi = 1;
        //        item.AktifMi = 0;
        //        item.SilinmeTarihi = DateTime.Now;
        //        this.Update(item);
        //    }
        //    return true.ToResult();
        //}

        ///// <summary>
        ///// Kişi Id'ye göre kişi adres bilgilerini aktif yapan metot
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>
        //public Result<bool> KisiBilgileriAktifYap(int kisiID)
        //{
        //    var list = this.List(x => x.KisiTemelBilgiId == kisiID).Value;

        //    foreach (var item in list)
        //    {
        //        item.SilindiMi = 0;
        //        item.AktifMi = 1;
        //        item.AktiflikTarihi = DateTime.Now;
        //       this.Update(item);
        //    }
        //    return true.ToResult();
        //}
    }
}