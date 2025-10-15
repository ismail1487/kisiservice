using Baz.AletKutusu;
using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.Model.Pattern;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static Baz.Service.Helper.ExpressionUtils;

namespace Baz.Service.Base
{
    /// <summary>
    ///     Kişi hassas bilgiler yöneten interface
    /// </summary>
    public interface IKisiHassasBilgilerService : IService<KisiHassasBilgiler>
    {
        /// <summary>
        /// Id ile kişi hassas bilgileri getiren method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        Result<KisiHassasBilgiler> KisiIdileGetir(int kisiID);

        /// <summary>
        ///  Kurum idleri ve filter ile key value listesi getirme
        /// </summary>
        /// <param name="comparisonType"></param>
        /// <param name="buildPredicateModels"></param>
        /// <param name="kurumIdleri"></param>
        /// <returns></returns>
        public Result<List<KeyValueModel>> List(ComparisonType comparisonType, List<BuildPredicateModel> buildPredicateModels, List<int> kurumIdleri);

        /// <summary>
        /// Kisiye sifre atama islemi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result<bool> SifreAtama(SifreAtamaModel model);
    }

    /// <summary>
    /// Kişilerin parola, TC Kimlik No, Pasaport numarası gibi hassas bilgilerini içeren KisiHassasBilgiler tablosu ile ilgili işlemleri barındıran servis classı.
    /// </summary>
    public class KisiHassasBilgilerService : Service<KisiHassasBilgiler>, IKisiHassasBilgilerService
    {
        private new readonly IServiceProvider _serviceProvider;
        private readonly ILoginUser _loginUser;

        /// <summary>
        /// Kişilerin parola, TC Kimlik No, Pasaport numarası gibi hassas bilgilerini içeren KisiHassasBilgiler tablosu ile ilgili işlemleri barındıran servis classnın apıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="loginUser"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public KisiHassasBilgilerService(IRepository<KisiHassasBilgiler> repository, IDataMapper dataMapper, ILoginUser loginUser, IServiceProvider serviceProvider, ILogger<KisiHassasBilgiler> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
            _loginUser = loginUser;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Id ile kişi hassas bilgileri getiren method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        public Result<KisiHassasBilgiler> KisiIdileGetir(int kisiID)
        {
            var result = List(x => x.KisiTemelBilgiId == kisiID).Value.FirstOrDefault();
            if (result == null)
            {
                return Results.Fail("Kayıt bulunmadı.", ResultStatusCode.ReadError);
            }
            return result.ToResult();
        }

        /// <summary>
        ///  Kurum idleri ve filter ile key value listesi getirme
        /// </summary>
        /// <param name="comparisonType"></param>
        /// <param name="buildPredicateModels"></param>
        /// <param name="kurumIdleri"></param>
        /// <returns></returns>
        public Result<List<KeyValueModel>> List(ComparisonType comparisonType, List<BuildPredicateModel> buildPredicateModels, List<int> kurumIdleri)
        {
            var _kisiService = _serviceProvider.GetService<IKisiService>();

            var result = _repository.List(x => x.AktifMi == 1).AsQueryable().WhereBuilder(comparisonType, buildPredicateModels).ToList();
            if (result == null || result.Count == 0)
                return new List<KeyValueModel>().ToResult();
            var kisiIDs = result.Select(x => x.KisiTemelBilgiId);
            var kisiList = _kisiService.List(x => kisiIDs.Contains(x.TabloID) && x.AktifMi == 1 && kurumIdleri.Contains(x.KisiBagliOlduguKurumId.Value)).Value;
            return result.Where(x => kisiList.Select(a => a.TabloID).Contains(x.KisiTemelBilgiId)).Select(p => new KeyValueModel() { Key = kisiList.FirstOrDefault(x => x.TabloID == p.KisiTemelBilgiId).KisiAdi + " " + kisiList.FirstOrDefault(x => x.TabloID == p.KisiTemelBilgiId).KisiSoyadi, Value = p.KisiTemelBilgiId.ToString() }).ToList().ToResult();
        }

        /// <summary>
        /// Kisiye sifre atama islemi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result<bool> SifreAtama(SifreAtamaModel model)
        {
            var hashSalt = HashSalt.GenerateSaltedHash(64, model.KisiSifre);
            var kisiHassasBilgi = this.List(x => x.KisiTemelBilgiId == model.KisiId).Value.FirstOrDefault();
            kisiHassasBilgi.HashValue = hashSalt.Hash;
            kisiHassasBilgi.SaltValue = hashSalt.Salt;
            var result = false;
            var resultModel = Update(kisiHassasBilgi);
            if (resultModel.Value != null)
            {
                result = true;
            }
            return result.ToResult();
        }
    }
}