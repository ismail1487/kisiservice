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
    /// Kişi telefon bilgilerinin yöneten interface
    /// </summary>
    public interface IKisiTelefonBilgileriService : IService<KisiTelefonBilgileri>
    {
        /// <summary>
        /// Id ile kişi telefon bilgilerinin getirildiği metod
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        Result<List<KisiTelefonBilgileri>> KisiIdileGetir(int kisiID);

        ///// <summary>
        ///// Kişi Id'ye göre kişi telefon bilgilerini silindi yapan metot
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>
        //Result<bool> KisiBilgileriSilindiYap(int kisiID);

        ///// <summary>
        ///// Kişi Id'ye göre kişi telefon bilgilerini aktif yapan metot
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>
        //Result<bool> KisiBilgileriAktifYap(int kisiID);
    }

    /// <summary>
    ///  Kişi telefon bilgilerinin yöneten servis sınıfı
    /// </summary>
    public class KisiTelefonBilgileriService : Service<KisiTelefonBilgileri>, IKisiTelefonBilgileriService
    {
        /// <summary>
        /// Kişi telefon bilgilerinin yöneten servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public KisiTelefonBilgileriService(IRepository<KisiTelefonBilgileri> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<KisiTelefonBilgileriService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
        }

        /// <summary>
        /// Id ile kişi telefon bilgilerinin getirildiği metod
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        public Result<List<KisiTelefonBilgileri>> KisiIdileGetir(int kisiID)
        {
            var result = List(x => x.KisiTemelBilgiId == kisiID);
            return result;
        }

        ///// <summary>
        ///// Kişi Id'ye göre kişi telefon bilgilerini silindi yapan metot
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
        ///// Kişi Id'ye göre kişi telefon bilgilerini aktif yapan metot
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
        //        this.Update(item);
        //    }
        //    return true.ToResult();
        //}
    }
}