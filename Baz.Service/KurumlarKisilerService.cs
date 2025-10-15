using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.RequestManager.Abstracts;
using Baz.Service.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baz.Service
{
    /// <summary>
    /// Kurumlar kişilerin servis sınıfıdır.
    /// </summary>
    public interface IKurumlarKisilerService : IService<KurumlarKisiler>
    {
        /// <summary>
        /// Id ile kurum, kişileri getiren method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        Result<List<KurumlarKisiler>> KisiIdileGetir(int kisiID);

        ///// <summary>
        ///// Kişi bilgilerini silindi yapan method
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>

        //Result<bool> KisiBilgileriSilindiYap(int kisiID);

        /// <summary>
        /// Kuruma ait pozisyonları kurumId'ye göre listeleyen method
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        Result<List<KurumOrganizasyonBirimView>> PozisyonlarList(int kurumID);

        ///// <summary>
        ///// Kişi bilgilerini aktif yapan method
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>
        //Result<bool> KisiBilgileriAktifYap(int kisiID);

        /// <summary>
        /// Kişinin müşteri temsilcisi mi olup olmadığını kontrol eden metod
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        Result<bool> KisiMusteriTemsilcisiMi(int kisiId);
    }

    /// <summary>
    /// Kurumlar kişilerin servis sınıfıdır.
    /// </summary>
    public class KurumlarKisilerService : Service<KurumlarKisiler>, IKurumlarKisilerService
    {
        private readonly IRequestHelper _requestHelper;
        private readonly IKurumOrganizasyonBirimTanimlariService _kurumOrganizasyonBirimTanimlariService;

        /// <summary>
        /// Kurumlar kişilerin servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="requestHelper"></param>
        public KurumlarKisilerService(IRepository<KurumlarKisiler> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<KurumlarKisilerService> logger, IRequestHelper requestHelper, IKurumOrganizasyonBirimTanimlariService kurumOrganizasyonBirimTanimlariService) : base(repository, dataMapper, serviceProvider, logger)
        {
            _requestHelper = requestHelper;
            _kurumOrganizasyonBirimTanimlariService = kurumOrganizasyonBirimTanimlariService;
        }

        /// <summary>
        /// Id ile kurum, kişileri getiren method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        public Result<List<KurumlarKisiler>> KisiIdileGetir(int kisiID)
        {
            var result = List(x => x.IlgiliKisiId == kisiID && x.SilindiMi == 0);
            return result;
        }

        ///// <summary>
        ///// Kişi bilgilerini silindi yapan method
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>
        //public Result<bool> KisiBilgileriSilindiYap(int kisiID)
        //{
        //    var list = this.List(x => x.IlgiliKisiId == kisiID).Value;

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
        ///// Kişi bilgilerini aktif yapan method
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>
        //public Result<bool> KisiBilgileriAktifYap(int kisiID)
        //{
        //    var list = this.List(x => x.IlgiliKisiId == kisiID).Value;

        //    foreach (var item in list)
        //    {
        //        item.SilindiMi = 0;
        //        item.AktifMi = 1;
        //        item.AktiflikTarihi = DateTime.Now;
        //       this.Update(item);
        //    }
        //    return true.ToResult();
        //}

        /// <summary>
        /// Kuruma ait pozisyonları kurumId'ye göre listeleyen method
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        public Result<List<KurumOrganizasyonBirimView>> PozisyonlarList(int kurumID)
        {
            var pozisyonRequest = new KurumOrganizasyonBirimRequest()
            {
                KurumId = kurumID,
                Name = "pozisyon"
            };
            var pozisyonList = _kurumOrganizasyonBirimTanimlariService.ListTip(pozisyonRequest);
            return pozisyonList;
        }

        /// <summary>
        /// Kişinin müşteri temsilcisi mi olup olmadığını kontrol eden metod
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        public Result<bool> KisiMusteriTemsilcisiMi(int kisiId)
        {
            var _organizasyon = _serviceProvider.GetService<IKurumOrganizasyonBirimTanimlariService>();
            var temsilciOrgTanim = _organizasyon.List(a => a.BirimTanim.ToLower().Contains("müşteri temsilcisi") && a.AktifMi == 1).Value.Select(a => a.TabloID);
            var kontrol = _repository.List(a => a.IlgiliKisiId == kisiId && a.AktifMi == 1 && temsilciOrgTanim.Contains(a.KurumOrganizasyonBirimTanimId)).Any();
            if (kontrol)
                return true.ToResult();
            return false.ToResult();
        }
    }
}