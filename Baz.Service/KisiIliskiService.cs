using Baz.AOP.Logger.ExceptionLog;
using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.Model.Pattern;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.RequestManager.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Baz.Service.Base;
using Decor;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Baz.Service
{
    /// <summary>
    /// Kişiler arası ilişkiye ait metotların yer aldığı interface
    /// </summary>
    public interface IKisiIliskiService : IService<Iliskiler>
    {
        /// <summary>
        /// Kişiler arası ilişkiyi kurumId'ye göre listeleyen metot
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        Result<List<Iliskiler>> KisiIliskiList(int kurumID);

        /// <summary>
        /// Bu kurumun müşteri ilişklerini getiren metod
        /// </summary>
        /// <param name="buKurumID"></param>
        /// <returns></returns>
        Result<List<Iliskiler>> MusteriList(int buKurumID);

        /// <summary>
        /// Kişiler arası ilişkiyi silindi yapan metot
        /// </summary>
        /// <param name="tabloID"></param>
        /// <returns></returns>
        Result<bool> KisiIliskiSil(int tabloID);

        /// <summary>
        /// Kişiler arası ilişkiyi kaydeden metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Result<Iliskiler> KisiIliskiKaydet(KisiIliskiKayitModel model);

        /// <summary>
        /// Kişiler arası ilişkiyi güncelleyen metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Result<Iliskiler> KisiIliskiGuncelle(KisiIliskiKayitModel model);

        /// <summary>
        /// müşteri temsilcisine baplı kurumları listeleme
        /// </summary>
        /// <param name="musteriTemsilciId"></param>
        /// <returns></returns>
        public Result<List<int>> MusteriTemsilcisiBagliKurumGetir(int musteriTemsilciId);
    }

    /// <summary>
    /// Kişiler arası ilişkiye ait metotların yer aldığı servis sınıfıdır
    /// </summary>
    public class KisiIliskiService : Service<Iliskiler>, IKisiIliskiService
    {
        private readonly ILoginUser _loginUser;
        private readonly IRequestHelper _requestHelper;

        /// <summary>
        /// işiler arası ilişkiye ait metotların yer aldığı servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="loginUser"></param>
        /// <param name="requestHelper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public KisiIliskiService(IRepository<Iliskiler> repository, IDataMapper dataMapper, ILoginUser loginUser, IRequestHelper requestHelper, IServiceProvider serviceProvider, ILogger<Iliskiler> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
            _loginUser = loginUser;
            _requestHelper = requestHelper;
        }

        /// <summary>
        /// Kişiler arası ilişkiyi kurumId'ye göre listeleyen metot
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        public Result<List<Iliskiler>> KisiIliskiList(int kurumID)
        {
            var paramIliski = new ParametreRequest()
            {
                ModelName = "ParamIliskiTurleri",
                UstId = 0,
                KurumId = 0,
                TabloID = 0,
                Tanim = "..",
                ParamKod = "",
                DilID = 1,
                EsDilID = 0
            };
            var url1 = LocalPortlar.IYSService + "/api/KureselParametreler/ListParam";
            var tanim = _requestHelper.Post<Result<List<ParametreResult>>>(url1, paramIliski).Result.Value.Where(a => a.ParamKod == "K");
            var Ids = tanim.Select(a => a.TabloID).ToList();
            var _kisiService = _serviceProvider.GetService<IKisiService>();
            // Kurum ilişki kayıtlarını getiren sorgu
            var join = (from iliski in _repository.List()
                        join kisi1 in _kisiService.ListForQuery() on iliski.BuKisiId equals kisi1.TabloID
                        join kisi2 in _kisiService.ListForQuery() on iliski.BuKisiId equals kisi2.TabloID
                        where iliski.KurumID == kurumID && iliski.BuKisiId != null && iliski.BununKisiId != null && iliski.AktifMi == 1 && kisi1.AktifMi == 1 && kisi2.AktifMi == 1 && Ids.Contains(iliski.IliskiTuruId.Value)
                        select iliski).Distinct().ToList();

            return join.ToResult();
        }

        /// <summary>
        /// Kişiler arası ilişkiyi kaydeden metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public Result<Iliskiler> KisiIliskiKaydet(KisiIliskiKayitModel model)
        {
            var iliskiler = new Iliskiler()
            {
                KayitEdenID = model.KayıtEdenID,
                GuncelleyenKisiID = model.GuncelleyenKisiID,
                KurumID = model.KurumID,
                BuKisiId = model.BuKisiID,
                IliskiTuruId = model.IliskiTuruID,
                BununKisiId = model.BununKisiID,
                AktifMi = 1,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                IliskiBaslamaZamani = DateTime.Now,
                IliskiBitisZamani = DateTime.Now
            };
            if (model.IliskiTuruID == (int)IliskiTipi.MusteriTemsilcisi)//11
            {
                iliskiler.BuKurumId = model.BununKisiID;
                iliskiler.BununKisiId = null;
            }

            var iliskikayit = this.Add(iliskiler);

            return iliskikayit;
        }

        /// <summary>
        /// Kişiler arası ilişkiyi güncelleyen metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public Result<Iliskiler> KisiIliskiGuncelle(KisiIliskiKayitModel model)
        {
            if (model.BuKisiID == 0 && model.BununKisiID == 0)
            {
                return Results.Fail("Kişiler Geçersiz.", ResultStatusCode.CreateError);
            }
            var iliskiler = new Iliskiler()
            {
                TabloID = model.TabloID,
                KayitEdenID = model.KayıtEdenID,
                GuncelleyenKisiID = model.GuncelleyenKisiID,
                KurumID = model.KurumID,
                BuKisiId = model.BuKisiID,
                IliskiTuruId = model.IliskiTuruID,
                BununKisiId = model.BununKisiID,
                AktifMi = 1,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                IliskiBaslamaZamani = DateTime.Now,
                IliskiBitisZamani = DateTime.Now
            };
            if (model.IliskiTuruID == (int)IliskiTipi.MusteriTemsilcisi)//11
            {
                iliskiler.BuKurumId = model.BununKisiID;
                iliskiler.BununKisiId = null;
            }

            var iliskikayit = this.Update(iliskiler);

            return iliskikayit;
        }

        /// <summary>
        /// Kişiler arası ilişkiyi silindi yapan metot
        /// </summary>
        /// <param name="tabloID"></param>
        /// <returns></returns>
        public Result<bool> KisiIliskiSil(int tabloID)
        {
            var iliskiler = this.SingleOrDefault(tabloID).Value;
            if (iliskiler == null)
            {
                return Results.Fail("İşlem başarısız.", ResultStatusCode.DeleteError);
            }
            iliskiler.AktifMi = 0;
            iliskiler.SilindiMi = 1;
            var result = this.Update(iliskiler);
            return true.ToResult();
        }

        /// <summary>
        /// Bu kurumun müşteri ilişklerini getiren metod
        /// </summary>
        /// <param name="buKurumID"></param>
        /// <returns></returns>
        public Result<List<Iliskiler>> MusteriList(int buKurumID)
        {
            var result = List(a => a.BuKurumId == buKurumID && a.AktifMi == 1 && a.BuKisiId != null).Value;
            return result.ToResult();
        }

        /// <summary>
        /// müşteri temsilcisine baplı kurumları listeleme
        /// </summary>
        /// <param name="musteriTemsilciId"></param>
        /// <returns></returns>
        public Result<List<int>> MusteriTemsilcisiBagliKurumGetir(int musteriTemsilciId)
        {
            var list = _repository.List(a => a.BuKisiId == musteriTemsilciId && a.IliskiTuruId == (int)IliskiTipi.MusteriTemsilcisi && a.AktifMi == 1).Select(a => Convert.ToInt32(a.BuKurumId)).ToList(); // 11, param iliski turleri tablosunda kayıtlı müsteri temsilcisi ID degeri. kayıt işlemi tamamlansın dinamikleştirilecek.
            return list.ToResult();
        }
    }
}