using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Pattern;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.RequestManager.Abstracts;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Baz.Service
{
    /// <summary>
    /// Kurumlar işlemlerini yöneten interface
    /// </summary>
    public interface IKurumlarService : IService<KurumTemelBilgiler>
    {
        /// <summary>
        /// kurumun altındaki kurumları getiren metod
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns></returns>
        Result<List<int>> ListForAltKurum(int kurumId);

        /// <summary>
        /// kurum idye göre hedef kitle kişileri getiren metod
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        Result<List<HedefKitleTanimlamalar>> GetKisiListByKurum(int kurumID);

        /// <summary>
        /// amirIdye göre kurumları getiren metod
        /// </summary>
        /// <param name="amirId"></param>
        /// <returns></returns>
        public Result<List<KurumTemelBilgiler>> AmirlereAstMusteriTemsilcisiKurumlariniGetir(int amirId);

        /// <summary>
        /// Musteri temsilcisi idye göre kurumları getiren metod
        /// </summary>
        /// <param name="musteriTemsilcisiId"></param>
        /// <returns></returns>
        public Result<List<KurumTemelBilgiler>> MusteriTemsilcisiBagliKurumlarList(int musteriTemsilcisiId);

        /// <summary>
        /// Kuruma bağlı kurumların tabloID'lerini getiren metot
        /// </summary>
        /// <param name="kurumId">kurum ID</param>
        /// <returns>int Id listesi</returns>
        public Result<List<int>> KurumaBagliKurumIdleriList(int kurumId);
    }

    /// <summary>
    /// Kurumlar servis sınıfıdır
    /// </summary>
    public class KurumlarService : Service<KurumTemelBilgiler>, IKurumlarService
    {
        private readonly IRequestHelper _helper;

        /// <summary>
        /// Kurumlar servis sınıfının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="helper"></param>
        /// <param name="logger"></param>
        public KurumlarService(IRepository<KurumTemelBilgiler> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, IRequestHelper helper, ILogger<KurumlarService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
            _helper = helper;
        }

        /// <summary>
        /// kurumun altındaki kurumları getiren metod
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns></returns>
        public Result<List<int>> ListForAltKurum(int kurumId)
        {
            var kurumlar = _repository.List(k => k.KurumID == kurumId && k.AktifMi == 1).Select(x => x.TabloID).ToList().ToResult();
            return kurumlar;
        }

        /// <summary>
        /// kurum idye göre hedef kitle kişileri getiren metod
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        public Result<List<HedefKitleTanimlamalar>> GetKisiListByKurum(int kurumID)
        {
            var x = _helper.Get<Result<List<HedefKitleTanimlamalar>>>(LocalPortlar.KurumService + "/api/HedefKitle/GetKisiListByKurum/" + kurumID);
            if (x.StatusCode == HttpStatusCode.OK)
            {
                return x.Result;
            }
            else
                return Results.Fail(x.Error.Message);
        }

        /// <summary>
        /// Musteri temsilcisi idye göre kurumları getiren metod
        /// </summary>
        /// <param name="musteriTemsilcisiId"></param>
        /// <returns></returns>
        public Result<List<KurumTemelBilgiler>> MusteriTemsilcisiBagliKurumlarList(int musteriTemsilcisiId)
        {
            var url = LocalPortlar.KurumService + "/api/KurumService/MusteriTemsilcisiBagliKurumlarList/" + musteriTemsilcisiId;
            var x = _helper.Get<Result<List<KurumTemelBilgiler>>>(url);

            if (x.StatusCode == HttpStatusCode.OK)
                return x.Result;
            return Results.Fail(x.Error.Message);
        }

        /// <summary>
        /// amirIdye göre kurumları getiren metod
        /// </summary>
        /// <param name="amirId"></param>
        /// <returns></returns>
        public Result<List<KurumTemelBilgiler>> AmirlereAstMusteriTemsilcisiKurumlariniGetir(int amirId)
        {
            var url = LocalPortlar.KurumService + "/api/KurumService/AmirlereAstMusteriTemsilcisiKurumlariniGetir/" + amirId;
            var x = _helper.Get<Result<List<KurumTemelBilgiler>>>(url);

            if (x.StatusCode == HttpStatusCode.OK)
                return x.Result;
            return Results.Fail(x.Error.Message);
        }

        /// <summary>
        /// Kuruma bağlı kurumların tabloID'lerini getiren metot
        /// </summary>
        /// <param name="kurumId">kurum ID</param>
        /// <returns>int Id listesi</returns>
        public Result<List<int>> KurumaBagliKurumIdleriList(int kurumId)
        {
            var result = _repository.List(a => a.AktifMi == 1 && a.KurumID == kurumId).Select(x => x.TabloID).ToList();
            return result.ToResult();
        }
    }
}