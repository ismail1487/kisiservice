using Baz.AletKutusu;
using Baz.AOP.Logger.ExceptionLog;
using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.Model.Pattern;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.RequestManager.Abstracts;
using Baz.Service.Base;
using Decor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Http;
using static Baz.Service.Helper.ExpressionUtils;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Baz.Service
{
    /// <summary>
    /// KisiService ile kişilere dair işlemlerin yönetileceği interface.
    /// </summary>
    public interface IKisiService : IService<KisiTemelBilgiler>
    {
        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve SilindiMi değerini aktif eden method.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Silme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>
        Result<KisiTemelBilgiler> KisiBilgileriSilme(int id);

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve Aktif Mi değerini 1 yapan method.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Aktifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>
        Result<KisiTemelBilgiler> KisiBilgileriAktiflestirme(int id);

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini aktif eden method.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Aktifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>
        Result<KisiTemelBilgiler> KisiAktiveEtme(int id);

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini pasif eden method.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Pasifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>
        Result<KisiTemelBilgiler> KisiPasifEtme(int id);

        /// <summary>
        /// Id değerine göre kişi verilerini getiren method.
        /// </summary>
        /// <param name="id"> kişi id değeri.</param>
        /// <returns>BasicKisiModel türünde ilgili kişi verilerini içeren modeli döndürür</returns>
        Result<BasicKisiModel> BasicSingleOrDefault(int id);

        /// <summary>
        /// Mail adresine göre kişi verilerini getiren method.
        /// </summary>
        /// <param name="mail"> kişi mail adresi.</param>
        /// <returns>BasicKisiModel türünde ilgili kişi verilerini içeren modeli döndürür</returns>
        Result<BasicKisiModel> SingleOrDefault(string mail);

        /// <summary>
        /// Kullanıcı adına göre kişi verilerini getiren method.
        /// </summary>
        /// <param name="userName"> kişi kullanıcı adı.</param>
        /// <returns>BasicKisiModel türünde ilgili kişi verilerini içeren modeli döndürür</returns>
        Result<BasicKisiModel> SingleOrDefaultForUserName(string userName);

        /// <summary>
        /// KişiID değeriyle bir şifre yenileme talebi oluşturan method.
        /// </summary>
        /// <param name="KisiID"> ilgili kişinin Id değeri</param>
        /// <returns>sonucu döndürür.</returns>
        Result<SistemLoginSifreYenilemeAktivasyonHareketleri> SifreYenilemeIstegi(int KisiID);

        /// <summary>
        ///  kişi mail adresi değeriyle bir şifre yenileme talebi oluşturan method.
        /// </summary>
        /// <param name="mail"> ilgili kişinin Id değeri</param>
        /// <returns>sonucu döndürür.</returns>
        Result<SistemLoginSifreYenilemeAktivasyonHareketleri> SifreYenilemeIstegi(string mail);

        /// <summary>
        /// İlgili şifre yenileme maili geçerli mi kontrolü sağlayan method.
        /// </summary>
        /// <param name="guid">şifre yenileme için gereke GUID değerini içeren string parametre.</param>
        /// <returns>Geçerliyse true, değilse false döndürür.</returns>
        Result<bool> SifreYenilemeGecerliMi(string guid);

        /// <summary>
        /// Kişinin ado soyadını direk seç
        /// </summary>
        /// <param name="KisiID"></param>
        /// <returns></returns>
        string GetAdSoyad(int KisiID);

        /// <summary>
        /// Kişi şifre değişim methodu. Kullanıcı şifre yenileme talebi ile şifre yeilediğinde bu method kullanılır.
        /// </summary>
        /// <param name="model">Şifre model türünde model parametresi</param>
        /// <returns>şifre değişim işlemi sonrası, yeni verileri döndürür.</returns>
        Result<SifreModel> SifreDegistir(SifreModel model);

        Result<SifreModel> SifreVer(string sifre);
        /// <summary>
        /// Kişi şifre değişim methodu. Kullanıcı login durumdayken profilinden şifresini değiştirmek istediğinde bu method kullanılır.
        /// </summary>
        /// <param name="model">Şifre model türünde model parametresi</param>
        /// <returns>şifre değişim işlemi sonrası, yeni verileri döndürür.</returns>
        Result<SifreModel> SifreYenile(SifreModel model);

        /// <summary>
        /// Kişi bilgilerinin kaydının yapıldığı method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Result<KisiTemelKayitModel> KisiTemelKaydet(KisiTemelKayitModel model);

        /// <summary>
        /// Id ile kisi bilgilerinin getirildiği method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        Result<KisiTemelKayitModel> KisiTemelKayitVerileriGetir(int kisiID);

        /// <summary>
        /// Kişi bilgilerinin güncellendiği method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Result<KisiTemelKayitModel> KisiTemelKayitVerileriGuncelle(KisiTemelKayitModel model);

        /// <summary>
        /// Kuruma bağlı kişilerin listelendiği method
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns>listelenen kişileri döndürür.</returns>
        Result<List<KisiListeModel>> KisiListesiGetir(int kurumId);

        /// <summary>
        /// Temel kişi kayıt ile eklenmiş kişilerin silindiği method
        /// </summary>
        /// <param name="kisiID"> silinecek kişiID</param>
        /// <returns></returns>
        Result<bool> TemelKisiSilindiYap(int kisiID);

        /// <summary>
        /// Kişilerin aktifleştirildiği method
        /// </summary>
        /// <param name="kisiID"> aktifleşecek kişiID</param>
        /// <returns></returns>
        Result<bool> TemelKisiAktifYap(int kisiID);

        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin astlarını getiren method
        /// </summary>
        /// <param name="kisiID"> astları getirilecek kişiId</param>
        /// <returns>kişinin astları listesi.</returns>
        Result<List<KisiOrganizasyonBirimView>> KisiAstlarListGetir(int kisiID);

        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin amirlerini getiren method
        /// </summary>
        /// <param name="kisiID"> amirleri getirilecek kişiId</param>
        /// <returns>kişinin amirlerinin listesi</returns>
        Result<List<KisiOrganizasyonBirimView>> KisiAmirlerListGetir(int kisiID);

        Result<List<KurumOrganizasyonBirimTanimlari>> KisiOrganizasyonListGetir(int kisiID);


        /// <summary>
        /// KisiId değeri ile ilgili kişinin sifre son yenileme tarihi değerini getiren method
        /// </summary>
        /// <param name="kisiID"> ilgili kişiId</param>
        /// <returns>şifre son yenileme tarihi</returns>
        Result<DateTime> KisiSifreSonYenilemeTarihiGetir(int kisiID);

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini pasif eden method.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Pasifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>
        Result<KisiTemelBilgiler> KisiHesabiPasifEtme(int id);

        /// <summary>
        ///  Kurum idleri ve filter ile key value listesi getirme
        /// </summary>
        /// <param name="comparisonType"></param>
        /// <param name="buildPredicateModels"></param>
        /// <param name="_kurumIdleri"></param>
        /// <returns></returns>
        public Result<List<KeyValueModel>> List(ComparisonType comparisonType, List<BuildPredicateModel> buildPredicateModels, List<int> _kurumIdleri);

        /// <summary>
        /// Id Lere göre kişileri KisiListeModel türünde getirir.
        /// </summary>
        /// <param name="model"> ilgili kisi Idleri</param>
        /// <returns>listelenen kişileri döndürür.</returns>
        Result<List<KisiTemelBilgiler>> IdlereGoreKisileriGetir(List<int> model);

        /// <summary>
        /// kurumId ile kuruma bağlı kişileri getirme
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns></returns>
        Result<List<KisiTemelBilgiler>> KurumaBagliKisilerList(int kurumId);

        /// <summary>
        /// Excel dosyasından import edilen kişi listesinin kaydedilmesi.
        /// </summary>
        /// <param name="list"> Kişi listesi</param>
        /// <returns>kaydedilen kişileri döndürür</returns>
        Result<List<KisiTemelKayitModel>> ListeIleTemelKisikaydet(List<KisiTemelKayitModel> list);



        /// <summary>
        /// Idye göre kişi getirme
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        Result<KisiTemelBilgiler> IdyeGoreKisiGetir(int kisiID);

        /// <summary>
        /// Kurum Id'ye göre Pasif kişileri getirem metod
        /// </summary>
        /// <returns></returns>
        Result<List<KisiTemelBilgiler>> PasifKisiListesiGetir();

        /// <summary>
        /// Müşteri temsilcisine bağlı kişileri getirme
        /// </summary>
        /// <param name="musteriTemsilcisiId"></param>
        /// <returns></returns>
        Result<List<KisiListeModel>> MusteriTemsilcisiBagliKisilerList(int musteriTemsilcisiId);

        /// <summary>
        /// İdlere göre kişileri getirme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Result<List<KisiListeModel>> IdlereGoreKisiListeModelGetir(List<int> model);

        /// <summary>
        /// Amir Id ile müsteri temsilcilerini getirme
        /// </summary>
        /// <param name="amirId"></param>
        /// <returns></returns>
        Result<List<KisiListeModel>> AmirlereAstMusteriTemsilcisiKisileriniGetir(int amirId);

        /// <summary>
        /// kisi Id ile müsteri temsilcileri getirme
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        Result<List<int>> AmireBagliMusteriTemsilcileriList(int kisiId);

        /// <summary>
        /// Kisi Id ile liste model şeklinde kişi getirme
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        Result<KisiListeModel> SingleOrDefaultListeModel(int kisiId);

        /// <summary>
        /// kisi Id mile amir veya müsteri temsilcisi listeleme
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        public Result<List<int>> AmirveyaMusteriTemsilcisiKurumlariIDGetir(int kisiId);

        /// <summary>
        /// Pozisyona bağlı hiyerarşik ağaçta ast-üst ilişkisi bulunmayan ancak ilgili kişilere irişmesi gereken kullanıcılar için kullanılacak kişi listesi metodu.
        /// </summary>
        /// <returns>KisiListeModel listesi döndürür. <see cref="KisiListeModel"></see></returns>
        public Result<List<KisiListeModel>> HiyerarsiDisiKisilerKisiListesi();

        /// <summary>
        /// İlgili kurum Id ve aktif kişi Id değerine göre o kurumda tanımlı sanal kişi verilerini getiren metot.
        /// </summary>
        /// <param name="kurumId"></param>
        /// <param name="aktifKisiId"></param>
        /// <returns></returns>
        public Result<KisiTemelBilgiler> KurumaBagliSanalKisiGetir(int kurumId, int aktifKisiId);

        /// <summary>
        /// Kişinin firebase token nını günceller
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Result<bool> UpdateFirebaseToken(string token);

        /// <summary>
        /// Kişinin firebase token nını temizler
        /// </summary>
        /// <returns></returns>
        public Result<bool> DeleteFirebaseToken();
    }

    /// <summary>
    /// KisiService ile kişilere dair işlemlerin yönetileceği servis classı.
    /// IKisiService interface'ini ve Service class'ını baz alır.
    /// </summary>
    public class KisiService : Service<KisiTemelBilgiler>, IKisiService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestHelper _requestHelper;
        private readonly ISistemLoginSifreYenilemeAktivasyonHareketleriService _sistemLoginSifreYenilemeAktivasyonHareketleriService;
        private readonly IKisiHassasBilgilerService _kisiHassasBilgilerService;
        private readonly IKisiAdresBilgileriService _kisiAdresBilgileriService;
        private readonly IKisiEgitimBilgileriService _kisiEgitimBilgileriService;
        private readonly IKisiTelefonBilgileriService _kisiTelefonBilgileriService;
        private readonly IKurumlarKisilerService _kurumlarKisilerService;
        private readonly IKurumlarService _kurumlarService;
        private readonly IKureselParametrelerService _kureselParametrelerService;
        private readonly IMedyaKutuphanesiService _medyaKutuphanesiService;
        private readonly ILoginUser _loginUser;
        private readonly IYetkiMerkeziService _yetkiMerkeziService;
        private readonly IKurumOrganizasyonBirimTanimlariService _kurumOrganizasyonBirimTanimlariService;
        private readonly IPostaciBekleyenIslemlerGenelService _postaciBekleyenIslemlerGenelService;
        private readonly IPostaciBekleyenIslemlerAyrintilarService _postaciBekleyenIslemlerAyrintilarService;
        private readonly IParamDilSeviyesiService _paramDilSeviyesiService;
        private readonly IKisiBildigiDillerService _kisiBildigiDillerService ;

        /// <summary>
        /// KisiService ile kişilere dair işlemlerin yönetileceği servis classının yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="kisiHassasBilgilerService"></param>
        /// <param name="sistemLoginSifreYenilemeAktivasyonHareketleriService"></param>
        /// <param name="kisiAdresBilgileriService"></param>
        /// <param name="kisiEgitimBilgileriService"></param>
        /// <param name="kisiTelefonBilgileriService"></param>
        /// <param name="kurumlarKisilerService"></param>
        /// <param name="kurumlarService"></param>
        /// <param name="kureselParametrelerService"></param>
        /// <param name="medyaKutuphanesiService"></param>
        /// <param name="yetkiMerkeziService"></param>
        /// <param name="kurumOrganizasyonBirimTanimlariService"></param>
        /// <param name="postaciBekleyenIslemlerGenelService"></param>
        /// <param name="postaciBekleyenIslemlerAyrintilarService"></param>
        /// <param name="requestHelper"></param>
        /// <param name="loginUser"></param>
        public KisiService(IRepository<KisiTemelBilgiler> repository, IKisiBildigiDillerService kisiBildigiDillerService, IParamDilSeviyesiService paramDilSeviyesiService, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<KisiService> logger, IKisiHassasBilgilerService kisiHassasBilgilerService, ISistemLoginSifreYenilemeAktivasyonHareketleriService sistemLoginSifreYenilemeAktivasyonHareketleriService, IKisiAdresBilgileriService kisiAdresBilgileriService, IKisiEgitimBilgileriService kisiEgitimBilgileriService, IKisiTelefonBilgileriService kisiTelefonBilgileriService, IKurumlarKisilerService kurumlarKisilerService, IKurumlarService kurumlarService, IKureselParametrelerService kureselParametrelerService, IMedyaKutuphanesiService medyaKutuphanesiService, IYetkiMerkeziService yetkiMerkeziService, IKurumOrganizasyonBirimTanimlariService kurumOrganizasyonBirimTanimlariService, IPostaciBekleyenIslemlerGenelService postaciBekleyenIslemlerGenelService, IPostaciBekleyenIslemlerAyrintilarService postaciBekleyenIslemlerAyrintilarService, IRequestHelper requestHelper, ILoginUser loginUser) : base(repository, dataMapper, serviceProvider, logger)
        {
            _sistemLoginSifreYenilemeAktivasyonHareketleriService = sistemLoginSifreYenilemeAktivasyonHareketleriService;
            _kisiHassasBilgilerService = kisiHassasBilgilerService;
            _requestHelper = requestHelper;
            _kisiAdresBilgileriService = kisiAdresBilgileriService;
            _kisiEgitimBilgileriService = kisiEgitimBilgileriService;
            _kisiTelefonBilgileriService = kisiTelefonBilgileriService;
            _kurumlarKisilerService = kurumlarKisilerService;
            _kurumlarService = kurumlarService;
            _kureselParametrelerService = kureselParametrelerService;
            _loginUser = loginUser;
            _httpContextAccessor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
            _medyaKutuphanesiService = medyaKutuphanesiService;
            _yetkiMerkeziService = yetkiMerkeziService;
            _kurumOrganizasyonBirimTanimlariService = kurumOrganizasyonBirimTanimlariService;
            _postaciBekleyenIslemlerGenelService = postaciBekleyenIslemlerGenelService;
            _postaciBekleyenIslemlerAyrintilarService = postaciBekleyenIslemlerAyrintilarService;
            _paramDilSeviyesiService = paramDilSeviyesiService;
            _kisiBildigiDillerService= kisiBildigiDillerService;
        }

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve SilindiMi değerini aktif eden method.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Silme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>

        public Result<KisiTemelBilgiler> KisiBilgileriSilme(int id)
        {
            var temelBilgilerData = List(a => a.TabloID == id).Value.SingleOrDefault();
            var hassasBilgilerData = _kisiHassasBilgilerService.List(a => a.KisiTemelBilgiId == id).Value.SingleOrDefault();
            var kisiKurumIliskileri = _kurumlarKisilerService.List(x => x.IlgiliKisiId == temelBilgilerData.TabloID && x.AktifMi == 1 && x.SilindiMi == 0).Value;
            DataContextConfiguration().BeginNewTransactionIsNotFound();
            temelBilgilerData.AktifMi = 0;
            temelBilgilerData.SilindiMi = 1;
            temelBilgilerData.SilinmeTarihi = DateTime.Now;
            temelBilgilerData.HesapIptalTarihi = DateTime.Now;
            temelBilgilerData.GuncellenmeTarihi = DateTime.Now;
            //İliski tablosundan kaldırılması kişinin verilerinin kaldırılması
            foreach (var item in kisiKurumIliskileri)
            {
                item.AktifMi = 0;
                item.SilindiMi = 1;
                item.SilinmeTarihi = DateTime.Now;
                item.GuncellenmeTarihi = DateTime.Now;
                _kurumlarKisilerService.Update(item);
            }
            hassasBilgilerData.AktifMi = 0;
            hassasBilgilerData.SilindiMi = 1;
            hassasBilgilerData.HesabiAktifMi = false;
            hassasBilgilerData.SilinmeTarihi = DateTime.Now;
            hassasBilgilerData.GuncellenmeTarihi = DateTime.Now;

            Update(temelBilgilerData);
            _kisiHassasBilgilerService.Update(hassasBilgilerData);
            DataContextConfiguration().Commit();
            return temelBilgilerData.ToResult();
        }

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve Aktif Mi değerini 1 yapan method.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Aktifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>

        public Result<KisiTemelBilgiler> KisiBilgileriAktiflestirme(int id)
        {
            var temelBilgilerData = List(a => a.TabloID == id).Value.SingleOrDefault();
            var hassasBilgilerData = _kisiHassasBilgilerService.List(a => a.KisiTemelBilgiId == id).Value.SingleOrDefault();
            DataContextConfiguration().BeginNewTransactionIsNotFound();
            temelBilgilerData.AktifMi = 1;
            temelBilgilerData.SilindiMi = 0;
            temelBilgilerData.GuncellenmeTarihi = DateTime.Now;
            temelBilgilerData.AktiflikTarihi = DateTime.Now;
            hassasBilgilerData.AktifMi = 1;
            hassasBilgilerData.SilindiMi = 0;
            hassasBilgilerData.HesabiAktifMi = true;
            hassasBilgilerData.AktiflikTarihi = DateTime.Now;
            hassasBilgilerData.GuncellenmeTarihi = DateTime.Now;
            Update(temelBilgilerData);
            _kisiHassasBilgilerService.Update(hassasBilgilerData);
            DataContextConfiguration().Commit();
            return temelBilgilerData.ToResult();
        }

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini aktif eden method.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Aktifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>

        public Result<KisiTemelBilgiler> KisiAktiveEtme(int id)
        {
            var temelBilgilerData = List(a => a.TabloID == id).Value.SingleOrDefault();
            var hassasBilgilerData = _kisiHassasBilgilerService.List(a => a.KisiTemelBilgiId == id).Value.SingleOrDefault();
            DataContextConfiguration().BeginNewTransactionIsNotFound();
            temelBilgilerData.AktifMi = 1;
            temelBilgilerData.AktiflikTarihi = DateTime.Now;
            temelBilgilerData.GuncellenmeTarihi = DateTime.Now;
            hassasBilgilerData.AktifMi = 1;
            hassasBilgilerData.AktiflikTarihi = DateTime.Now;
            hassasBilgilerData.GuncellenmeTarihi = DateTime.Now;
            hassasBilgilerData.HesabiAktifMi = true;
            Update(temelBilgilerData);
            _kisiHassasBilgilerService.Update(hassasBilgilerData);
            DataContextConfiguration().Commit();
            return temelBilgilerData.ToResult();
        }

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini pasif eden method.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Pasifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>

        public Result<KisiTemelBilgiler> KisiPasifEtme(int id)
        {
            var temelBilgilerData = List(a => a.TabloID == id).Value.SingleOrDefault();
            var hassasBilgilerData = _kisiHassasBilgilerService.List(a => a.KisiTemelBilgiId == id).Value.SingleOrDefault();
            DataContextConfiguration().BeginNewTransactionIsNotFound();
            temelBilgilerData.AktifMi = 0;
            temelBilgilerData.PasiflikTarihi = DateTime.Now;
            temelBilgilerData.GuncellenmeTarihi = DateTime.Now;
            hassasBilgilerData.AktifMi = 0;
            hassasBilgilerData.PasiflikTarihi = DateTime.Now;
            hassasBilgilerData.GuncellenmeTarihi = DateTime.Now;
            hassasBilgilerData.HesabiAktifMi = false;
            hassasBilgilerData.HesapDeAktifTarihi = DateTime.Now;
            Update(temelBilgilerData);
            _kisiHassasBilgilerService.Update(hassasBilgilerData);
            DataContextConfiguration().Commit();
            return temelBilgilerData.ToResult();
        }


        /// <summary>
        /// Id değerine göre kişi verilerini getiren method.
        /// </summary>
        /// <param name="id"> kişi id değeri.</param>
        /// <returns>BasicKisiModel türünde ilgili kişi verilerini içeren modeli döndürür</returns>

        public Result<BasicKisiModel> BasicSingleOrDefault(int id)
        {
            var model = new BasicKisiModel();

            var temelBilgilerData = this.List(a => a.TabloID == id && a.AktifMi == 1).Value.FirstOrDefault();
            if (temelBilgilerData == null)
            {
                return Results.Fail("mail adresi kayıtlı değil.");
            }
            var hassasBilgilerData = _kisiHassasBilgilerService.List(a => a.KisiTemelBilgiId == temelBilgilerData.TabloID).Value.SingleOrDefault();

            if (hassasBilgilerData == null)
            {
                return Results.Fail("hassa bilgiler tablosunda kişi kaydı bulunmuyor.");
            }

            model.TabloID = temelBilgilerData.TabloID;
            model.KisiAdi = temelBilgilerData.KisiAdi;
            model.KisiSoyadi = temelBilgilerData.KisiSoyadi;
            model.KisiMail = temelBilgilerData.KisiEposta;
            model.KisiKullaniciAdi = temelBilgilerData.KisiKullaniciAdi;
            model.KisiBagliOlduguKurumId = temelBilgilerData.KisiBagliOlduguKurumId;
            model.KisiEposta = temelBilgilerData.KisiEposta;
            model.KisiCinsiyetId = temelBilgilerData.KisiCinsiyetId;

            return model.ToResult();
        }

        /// <summary>
        /// Kullanıcı adına göre kişi verilerini getiren method.
        /// </summary>
        /// <param name="userName"> kişi kullanıcı adı.</param>
        /// <returns>BasicKisiModel türünde ilgili kişi verilerini içeren modeli döndürür</returns>

        public Result<BasicKisiModel> SingleOrDefaultForUserName(string userName)
        {
            var model = new BasicKisiModel();

            var temelBilgilerData = List(a => a.KisiKullaniciAdi == userName).Value.SingleOrDefault();

            model.TabloID = temelBilgilerData.TabloID;
            model.KisiAdi = temelBilgilerData.KisiAdi;
            model.KisiSoyadi = temelBilgilerData.KisiSoyadi;
            model.KisiMail = temelBilgilerData.KisiEposta;
            model.KisiKullaniciAdi = temelBilgilerData.KisiKullaniciAdi;
            model.KisiBagliOlduguKurumId = temelBilgilerData.KisiBagliOlduguKurumId;
            model.KisiEposta = temelBilgilerData.KisiEposta;
            model.KisiCinsiyetId = temelBilgilerData.KisiCinsiyetId;
            return model.ToResult();
        }

        /// <summary>
        /// Id'ye göre sonucun döndürüldüğü methodtur.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public override Result<KisiTemelBilgiler> SingleOrDefault(int id)
        {
            var result = base.SingleOrDefault(id);
            if (result.Value == null)
            {
                return (new KisiTemelBilgiler()).ToResult();
            }
            if (result.Value.KisiResimId > 0)
            {
                var ppResult = _medyaKutuphanesiService.SingleOrDefault(result.Value.KisiResimId);
                if (ppResult.Value != null)
                    result.Value.KisiResimUrl = ppResult.Value.MedyaUrl;
            }
            return result;
        }

        /// <summary>
        /// Mail adresine göre kişi verilerini getiren method.
        /// </summary>
        /// <param name="mail"> kişi mail adresi.</param>
        /// <returns>BasicKisiModel türünde ilgili kişi verilerini içeren modeli döndürür</returns>

        public Result<BasicKisiModel> SingleOrDefault(string mail)
        {
            var model = new BasicKisiModel();

            KisiTemelBilgiler temelBilgilerData = null;
            if (_loginUser.KurumID == 0)
            {
                temelBilgilerData = List(a => a.KisiEposta == mail && a.AktifMi == 1).Value.FirstOrDefault();//Gerçek kişi
            }
            else
            {
                temelBilgilerData = List(a => a.KisiEposta == mail && a.KurumID == _loginUser.KurumID && a.AktifMi == 1).Value.FirstOrDefault();//Kuruma ait gerçek veya sanal kişi.
            }

            if (temelBilgilerData == null)
            {
                var kisiPasifModel = List(t => t.KisiEposta == mail && t.AktifMi == 0).Value.FirstOrDefault();
                if (kisiPasifModel != null)
                {
                    return Results.Fail("Hesabınız pasif durumdadır. Lütfen yöneticinizle iletişime geçin.", ResultStatusCode.ReadError);
                }
                return Results.Fail("Mail adresi kayıtlı değil.", ResultStatusCode.ReadError);
            }
            var hassasBilgilerData = _kisiHassasBilgilerService.List(a => a.KisiTemelBilgiId == temelBilgilerData.TabloID).Value.SingleOrDefault();

            if (hassasBilgilerData == null)
            {
                return Results.Fail("Hassas bilgiler tablosunda kişi kaydı bulunmuyor.");
            }

            model.TabloID = temelBilgilerData.TabloID;
            model.KisiAdi = temelBilgilerData.KisiAdi;
            model.KisiSoyadi = temelBilgilerData.KisiSoyadi;
            model.KisiMail = temelBilgilerData.KisiEposta;
            model.KisiKullaniciAdi = temelBilgilerData.KisiKullaniciAdi;
            model.KisiBagliOlduguKurumId = temelBilgilerData.KisiBagliOlduguKurumId;
            model.KisiEposta = temelBilgilerData.KisiEposta;
            model.KisiCinsiyetId = temelBilgilerData.KisiCinsiyetId;

            return model.ToResult();
        }

        /// <summary>
        /// KişiID değeriyle bir şifre yenileme talebi oluşturan method.
        /// </summary>
        /// <param name="KisiID"> ilgili kişinin Id değeri</param>
        /// <returns>sonucu döndürür.</returns>

        public Result<SistemLoginSifreYenilemeAktivasyonHareketleri> SifreYenilemeIstegi(int KisiID)
        {
            var guid = Guid.NewGuid().ToString();
            var url = "/LoginRegister/RecoverPassword?g=" + guid;

            var kisiModel = SingleOrDefault(KisiID).Value;

            var sifreyenilemodel = new SistemLoginSifreYenilemeAktivasyonHareketleri()
            {
                SifreYenilemeAktivasyonLinkiYollananKisiId = kisiModel.TabloID,
                SifreYenilemeAktivasyonPostaciBekleyenIslemlerAyrintiId = 1,
                SifreYenilemeSayfasiGeciciUrl = url,
                GeciciUrlsonGecerlilikZamani = DateTime.Now.AddHours(5),
                GuncellenmeTarihi = DateTime.Now,
                KayitTarihi = DateTime.Now
            };

            // İlgili URL'e şifre yenileme mailinin gönderilmesi methodu burada olacak
            var result = _sistemLoginSifreYenilemeAktivasyonHareketleriService.Add(sifreyenilemodel);

            // TODO : Hakan - Hatırlatma servisi kaldırıldı.
            //Sişfre Yenile bildirimi gönderilmesi
            //var bildirim = new HatirlatmaKayitlar()
            //{
            //    AktifEdenKisiID = KisiID,
            //    KurumID = kisiModel.KurumID,
            //    KayitEdenID = KisiID,
            //    HatirlatmaKayıtEdenKisiId = KisiID,
            //    KisiID = KisiID,
            //    HatirlatmaMetni = "https://app.octapull.com" + url,
            //    HatirlatmaZamani = DateTime.Now,
            //    KayitTarihi = DateTime.Now,
            //    GuncellenmeTarihi = DateTime.Now,
            //    HatirlatmaTipi = BildirimSablonTipleri.SifreHatirlatma.ToString(),
            //    HatirlatmaSmsyollayacakMi = false,
            //    HatirlatmaEpostaYollayacakMi = true,
            //};
            //_requestHelper.Post<Result<HatirlatmaKayitlar>>(LocalPortlar.PostaciService + "/hatirlatma/Add", bildirim);

            return result;
        }

        /// <summary>
        ///  kişi mail adresi değeriyle bir şifre yenileme talebi oluşturan method.
        /// </summary>
        /// <param name="mail"> ilgili kişinin Id değeri</param>
        /// <returns>sonucu döndürür.</returns>

        public Result<SistemLoginSifreYenilemeAktivasyonHareketleri> SifreYenilemeIstegi(string mail)
        {
            var kisiModel = List(t => t.KisiEposta == mail && t.AktifMi == 1).Value.FirstOrDefault();
            var kisiHassas = _kisiHassasBilgilerService
                .List(a => a.HesabiAktifMi == false && a.KisiTemelBilgiId == kisiModel.TabloID).Value
                .FirstOrDefault();
            if (kisiHassas != null)
            {
                return Results.Fail("Hesabınız pasif durumdadır. Lütfen yöneticinizle iletişime geçin.", ResultStatusCode.ReadError);
            }
            //Kişinin pasif kontrolü
            if (kisiModel == null)
            {
                var kisiPasifModel = List(t => t.KisiEposta == mail).Value.FirstOrDefault();
                if (kisiPasifModel != null)
                {
                    var rs = _kisiHassasBilgilerService
                        .List(a => a.HesabiAktifMi == false && a.KisiTemelBilgiId == kisiPasifModel.TabloID).Value
                        .FirstOrDefault();
                    if (rs != null)
                    {
                        return Results.Fail("Hesabınız pasif durumdadır. Lütfen yöneticinizle iletişime geçin.", ResultStatusCode.ReadError);
                    }
                    return Results.Fail("Böyle bir kayıt mevcut değil.", ResultStatusCode.ReadError);
                }
                return Results.Fail("Kişi bulunamadı.", ResultStatusCode.ReadError);
            }
            //var result = SifreYenilemeIstegi(Convert.ToInt32(kisiModel.TabloID));

            var guid = Guid.NewGuid().ToString();
            var url = "/LoginRegister/RecoverPassword?g=" + guid;




            // İlgili URL'e şifre yenileme mailinin gönderilmesi methodu burada olacak
            PostaciBekleyenIslemlerGenel postacigenel = new PostaciBekleyenIslemlerGenel()
            {
                KurumID = kisiModel.KurumID,
                KisiID = kisiModel.TabloID,
                KayitEdenID = kisiModel.TabloID,
                AktifMi = 0,// Ayrıntı kaydolunca aktif edeceğiz yoksa Postacı devreye girdiğinde henüz ayrıntı kaydolmamış olabilir.
                DilID = 0,
                GuncellenmeTarihi = DateTime.Now,
                GuncelleyenKisiID = kisiModel.TabloID,
                TetiklemeEpostaMi = true,
                TetiklemeSmsmi = false,
                TetiklemeIlgiliKisiId = kisiModel.TabloID,
                SilindiMi = 0,
                PostaciIslemDurumTipiId = 1,
                TetiklemeIlgiliKurumId = kisiModel.KisiBagliOlduguKurumId,
                PostaciIslemReferansNo = Guid.NewGuid().ToString(),
                IcerikSablonTanimID = 3  // BildirimTipiEntity eklendikten sonra bunlar sabitlenecekler. Bildirim tiplerinden gelecek. BildirimSablonTipleri.SifreHatirlatma
            };
            _postaciBekleyenIslemlerGenelService.Add(postacigenel);
            PostaciBekleyenIslemlerAyrintilar ayrinti = new PostaciBekleyenIslemlerAyrintilar()
            {

                AktifMi = 1,
                DilID = 0,
                KisiID = kisiModel.TabloID,
                KayitEdenID = kisiModel.TabloID,
                GonderimHedefKisiId = kisiModel.TabloID,
                GuncellenmeTarihi = (DateTime)DateTime.Now,
                GuncelleyenKisiID = kisiModel.TabloID,
                PlanlananEpostaGonderimZamani = (DateTime)DateTime.Now,
                PostaciBekleyenIslemlerGenelId = postacigenel.TabloID,
                SilindiMi = 0,
                KurumID = kisiModel.KurumID,

            };

            _postaciBekleyenIslemlerAyrintilarService.Add(ayrinti);

            var sifreyenilemodel = new SistemLoginSifreYenilemeAktivasyonHareketleri()
            {
                SifreYenilemeAktivasyonLinkiYollananKisiId = kisiModel.TabloID,
                SifreYenilemeAktivasyonPostaciBekleyenIslemlerAyrintiId = ayrinti.TabloID,
                SifreYenilemeSayfasiGeciciUrl = url,
                GeciciUrlsonGecerlilikZamani = DateTime.Now.AddHours(5),
                GuncellenmeTarihi = DateTime.Now,
                KayitTarihi = DateTime.Now,
                AktifMi = 1,
                SilindiMi = 0,
                KisiID = kisiModel.TabloID,
                KurumID = kisiModel.KurumID,
            };

            var result = _sistemLoginSifreYenilemeAktivasyonHareketleriService.Add(sifreyenilemodel);




            postacigenel.AktifMi = 1; // Bu aşamadan sonra postacı servis otomatik bu işlemi görür.
            _postaciBekleyenIslemlerGenelService.Update(postacigenel);

            // Burada postaci bekleyenislemlerGenel tablosuna ve PostaciBekleyenIslemlerAyrinti tablosuna kayıt düşüreceğiz.



            // TODO : Hakan - Hatırlatma servisi kaldırıldı.
            //Kişiye bildirim gönderilmesi
            //var bildirim = new HatirlatmaKayitlar()
            //{
            //    AktifEdenKisiID = kisiModel.TabloID,
            //    KurumID = kisiModel.KurumID,
            //    KayitEdenID = kisiModel.TabloID,
            //    HatirlatmaKayıtEdenKisiId = kisiModel.TabloID,
            //    KisiID = kisiModel.TabloID,
            //    HatirlatmaMetni = "https://app.octapull.com" + url,
            //    HatirlatmaZamani = DateTime.Now,
            //    KayitTarihi = DateTime.Now,
            //    GuncellenmeTarihi = DateTime.Now,
            //    HatirlatmaTipi = BildirimSablonTipleri.SifreHatirlatma.ToString(),
            //    HatirlatmaSmsyollayacakMi = false,
            //    HatirlatmaEpostaYollayacakMi = true,
            //};
            //_requestHelper.Post<Result<HatirlatmaKayitlar>>(LocalPortlar.PostaciService + "/hatirlatma/Add", bildirim);
            return result;
        }

        /// <summary>
        /// İlgili şifre yenileme maili geçerli mi kontrolü sağlayan method.
        /// </summary>
        /// <param name="guid">şifre yenileme için gereke GUID değerini içeren string parametre.</param>
        /// <returns>Geçerliyse true, değilse false döndürür.</returns>

        public Result<bool> SifreYenilemeGecerliMi(string guid)
        {
            var sifreyenilemeModel = _sistemLoginSifreYenilemeAktivasyonHareketleriService.List(x => x.SifreYenilemeSayfasiGeciciUrl.Contains(guid)).Value.FirstOrDefault();
            if (sifreyenilemeModel.GeciciUrlsonGecerlilikZamani > DateTime.Now)
            {
                return true.ToResult();
            }
            else
            {
                return false.ToResult().WithError("Gecici url kullanım zamanı geçmiş.");
            }
        }
        /// <summary>
        /// Kişinin ado soyad lazım olan yerde kullanmak için
        /// </summary>
        /// <param name="KisiID"></param>
        /// <returns></returns>
        public string GetAdSoyad(int KisiID)
        {
            var model = this.SingleOrDefault(KisiID).Value;
            return string.Concat(model.KisiAdi, " ", model.KisiSoyadi);
        }

        /// <summary>
        /// Kişi şifre değişim methodu. Kullanıcı şifre yenileme talebi ile şifre yeilediğinde bu method kullanılır.
        /// </summary>
        /// <param name="model">Şifre model türünde model parametresi</param>
        /// <returns>şifre değişim işlemi sonrası, yeni verileri döndürür.</returns>

        public Result<SifreModel> SifreYenile(SifreModel model)
        {
            if (SifreYenilemeGecerliMi(model.kontrolGUID).Value)
            {
                var kisiID = _sistemLoginSifreYenilemeAktivasyonHareketleriService.List(x => x.SifreYenilemeSayfasiGeciciUrl.Contains(model.kontrolGUID)).Value.FirstOrDefault().SifreYenilemeAktivasyonLinkiYollananKisiId;
                var kisiModel = this.SingleOrDefault(kisiID).Value;
                var BasicKisiModel = SingleOrDefault(kisiModel.KisiEposta).Value;
                BasicKisiModel.KisiSifre = Hash.Encrypt(model.NewPassword);
                var hashSalt = HashSalt.GenerateSaltedHash(64, model.NewPassword);
                var temelBilgilerData = List(a => a.TabloID == BasicKisiModel.TabloID).Value.SingleOrDefault();
                var hassasBilgilerData = _kisiHassasBilgilerService.List(a => a.KisiTemelBilgiId == temelBilgilerData.TabloID).Value.SingleOrDefault();
                if (hassasBilgilerData.HashValue != null || hassasBilgilerData.SaltValue != null)
                {
                    var t = HashSalt.VerifyPassword(model.NewPassword, hassasBilgilerData.HashValue, hassasBilgilerData.SaltValue);
                    if (t)
                    {
                        return Results.Fail("Lütfen eski şifrenizden farklı bir şifre girin.", ResultStatusCode.UpdateError);
                    }
                }

                hassasBilgilerData.KisiSifre = Hash.Encrypt(BasicKisiModel.KisiSifre);
                hassasBilgilerData.SaltValue = hashSalt.Salt;
                hassasBilgilerData.HashValue = hashSalt.Hash;

                hassasBilgilerData.SifreSonYenilemeTarihi = DateTime.Now;
                var update = _kisiHassasBilgilerService.Update(hassasBilgilerData);

                model.KisiID = Convert.ToInt32(BasicKisiModel.TabloID);
                model.NewPassword = BasicKisiModel.KisiSifre;

                return model.ToResult();
            }
            else
            {
                return model.ToResult().WithError("Şifre yenileme isteği zamanı dolmuş.");
            }
        }

        public Result<SifreModel> SifreVer(string sifre)
        {
            var hassasModel = _kisiHassasBilgilerService.KisiIdileGetir(129).Value;
            var t = HashSalt.VerifyPassword(sifre, hassasModel.HashValue, hassasModel.SaltValue);
            if (t)
            {
                return Results.Fail("Lütfen eski şifrenizden farklı bir şifre girin.", ResultStatusCode.UpdateError);
            }

            var r = HashSalt.VerifyPassword(sifre, hassasModel.HashValue, hassasModel.SaltValue);
            if (r)
            {
                var hashNewPword = HashSalt.GenerateSaltedHash(64, sifre);
                hassasModel.HashValue = hashNewPword.Hash;
                hassasModel.SaltValue = hashNewPword.Salt;
                hassasModel.SifreSonYenilemeTarihi = DateTime.Now;
                _kisiHassasBilgilerService.Update(hassasModel);

                return Results.Fail("Şifre Güncellendi.", ResultStatusCode.Success);

            }
            else
            {
                return Results.Fail("Eski şifrenizi kontrol ediniz.", ResultStatusCode.UpdateError);
            }
        }


        /// <summary>
        /// Kişi şifre değişim methodu. Kullanıcı login durumdayken profilinden şifresini değiştirmek istediğinde bu method kullanılır.
        /// </summary>
        /// <param name="model">Şifre model türünde model parametresi</param>
        /// <returns>şifre değişim işlemi sonrası, yeni verileri döndürür.</returns>

        public Result<SifreModel> SifreDegistir(SifreModel model)
        {
            var hassasModel = _kisiHassasBilgilerService.KisiIdileGetir(model.KisiID).Value;
            var t = HashSalt.VerifyPassword(model.NewPassword, hassasModel.HashValue, hassasModel.SaltValue);
            if (t)
            {
                return Results.Fail("Lütfen eski şifrenizden farklı bir şifre girin.", ResultStatusCode.UpdateError);
            }

            var r = HashSalt.VerifyPassword(model.OldPassword, hassasModel.HashValue, hassasModel.SaltValue);
            if (r)
            {
                var hashNewPword = HashSalt.GenerateSaltedHash(64, model.NewPassword);
                hassasModel.HashValue = hashNewPword.Hash;
                hassasModel.SaltValue = hashNewPword.Salt;
                hassasModel.SifreSonYenilemeTarihi = DateTime.Now;
                _kisiHassasBilgilerService.Update(hassasModel);

                return model.ToResult();
            }
            else
            {
                return Results.Fail("Eski şifrenizi kontrol ediniz.", ResultStatusCode.UpdateError);
            }
        }

        /// <summary>
        /// Kişi bilgilerinin kaydının yapıldığı method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public Result<KisiTemelKayitModel> KisiTemelKaydet(KisiTemelKayitModel model)
        {
            if (model.Kurum == _loginUser.KurumID)
            {
                var user = this.List(x => x.KisiEposta == model.EpostaAdresi).Value.FirstOrDefault();//Sisteme kayıtlı gerçek kişi kontrolü
                if (user != null)
                {
                    return Results.Fail("Kişi sisteme kayıtlıdır. Kendi kurumunuza kayıt edemezsiniz.");
                }
            }
            DataContextConfiguration().BeginNewTransactionIsNotFound();
            var kurum = _kurumlarService.SingleOrDefault(model.Kurum);
            var KisiTemelBilgilerModel = new KisiTemelBilgiler()
            {
                KisiID = _loginUser.KisiID,
                KurumID = _loginUser.KurumID,
                KisiBagliOlduguKurumId = model.Kurum,
                KisiAdi = model.Adi,
                KisiSoyadi = model.Soyadi,
                KisiEposta = model.EpostaAdresi,
                KisiCinsiyetId = model.Cinsiyeti,
                KisiEkranAdi = model.EpostaAdresi,
                KisiKullaniciAdi = model.EpostaAdresi,
                KurumsalMi = true,
                IseGirisTarihi = Convert.ToDateTime(model.IseGirisTarihi).Year == 1 ? null : model.IseGirisTarihi,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                AktifMi = 1,
                SilindiMi = 0,
                DilID = kurum.Value.DilID
            };

            var TemelBilgilerReturnModel = this.Add(KisiTemelBilgilerModel).Value;

            if (model.DilListesi != null && model.DilListesi.Any())
            {
                foreach (var dil in model.DilListesi)
                {
                    var kisiDilBilgisi = new KisiBildigiDiller()
                    {
                        KurumID = model.Kurum,
                        KisiID = TemelBilgilerReturnModel.TabloID,
                        KisiTemelBilgiID = TemelBilgilerReturnModel.TabloID,
                        ParamYabanciDilID = dil.YabanciDilTipi,
                        ParamDilSeviyesiID = Convert.ToInt16(dil.DilSeviye),
                        KayitTarihi = DateTime.Now,
                        GuncellenmeTarihi = DateTime.Now,
                        AktifMi = 1,
                        SilindiMi = 0
                    };
                    _kisiBildigiDillerService.Add(kisiDilBilgisi);
                }
            }
            //Sanal kişinin gerçek kişiye atanması
            if (TemelBilgilerReturnModel.KurumID == model.Kurum)
            {

                TemelBilgilerReturnModel.GuncellenmeTarihi = DateTime.Now;
                this.Update(TemelBilgilerReturnModel);

                var aktifOlmayanAltKisiler = this.List(x => x.KisiEposta == model.EpostaAdresi).Value;
                foreach (var aktifOlmayanAltKisi in aktifOlmayanAltKisiler)
                {
                    aktifOlmayanAltKisi.GuncellenmeTarihi = DateTime.Now;
                    this.Update(aktifOlmayanAltKisi);
                }
            }
            else
            {
                var aktifAltKisiler = this.List(x => x.KisiEposta == model.EpostaAdresi).Value;
                if (aktifAltKisiler.Count > 0)
                {
                    TemelBilgilerReturnModel.GuncellenmeTarihi = DateTime.Now;
                    this.Update(TemelBilgilerReturnModel);
                }
            }

            var KisiHassasBilgilerModel = new KisiHassasBilgiler()
            {
                KurumID = model.Kurum,
                KisiID = TemelBilgilerReturnModel.TabloID,
                KisiTemelBilgiId = TemelBilgilerReturnModel.TabloID,
                KisiKimlikNo = model.TCKimlikNo,
                SicilNo = model.SicilNo,
                BabaAdi = model.BabaAdi,
                AnneAdi = model.AnneAdi,
                MedeniHali = model.MedeniHali,
                Dini = model.Dini,
                DogduguUlkeId = model.DogduguUlke,
                DogduguSehirId = model.DogduguSehir == null ? 0 : model.DogduguSehir.Value,
                DogumTarihi = Convert.ToDateTime(model.DogumTarihi).Year == 1 ? null : model.DogumTarihi,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                KisiPasaportNo = "",
                AktifMi = 1,
                SilindiMi = 0
            };
            //Kişinin ek bilgilerinin kaydı
            _kisiHassasBilgilerService.Add(KisiHassasBilgilerModel);
            foreach (var item in model.OkulListesi)
            {
                var KisiEgitimGecmisi = new KisiEgitimBilgileri()
                {
                    KurumID = model.Kurum,
                    KisiID = TemelBilgilerReturnModel.TabloID,
                    KisiTemelBilgiId = TemelBilgilerReturnModel.TabloID,
                    OkulTipiId = item.OkulTipi,
                    OkulAdi = item.OkulAdi,
                    Fakulte = item.Fakulte,
                    MezuniyetTarihi = Convert.ToDateTime(item.MezuniyetTarihi).Year == 1 ? null : model.DogumTarihi,//Convert.ToDateTime(item.MezuniyetTarihi),
                    KayitTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now
                };
                _kisiEgitimBilgileriService.Add(KisiEgitimGecmisi);
            }

            foreach (var item in model.AdresListesi)
            {
                var KisiAdresModel = new KisiAdresBilgileri()
                {
                    KurumID = model.Kurum,
                    KisiID = TemelBilgilerReturnModel.TabloID,
                    KisiTemelBilgiId = TemelBilgilerReturnModel.TabloID,
                    AdresTipiId = item.AdresTipi,
                    Adres = item.Adres,
                    UlkeId = item.Ulke == null ? 0 : item.Ulke.Value,
                    SehirId = item.Sehir == null ? 0 : item.Sehir.Value,
                    KayitTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now
                };
                _kisiAdresBilgileriService.Add(KisiAdresModel);
            }



            foreach (var item in model.TelefonListesi)
            {
                var kisiTelefonModel = new KisiTelefonBilgileri()
                {
                    KurumID = model.Kurum,
                    KisiID = TemelBilgilerReturnModel.TabloID,
                    KisiTemelBilgiId = TemelBilgilerReturnModel.TabloID,
                    TelefonTipiId = item.TelefonTipi,
                    TelefonNo = item.TelefonNo,
                    KayitTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now
                };
                _kisiTelefonBilgileriService.Add(kisiTelefonModel);
            }

            var KurumlarKisiler = new List<KurumlarKisiler>();

            var DepartmanModel = new KurumlarKisiler()
            {
                KurumID = model.Kurum,
                KisiID = TemelBilgilerReturnModel.TabloID,
                IlgiliKisiId = TemelBilgilerReturnModel.TabloID,
                IlgiliKurumId = model.Kurum,
                KurumOrganizasyonBirimTanimId = model.Departman,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                AtanmaZamani = DateTime.Now,
                AtanmaninSonlanmaZamani = DateTime.Now.AddYears(1),
                AktifMi = 1,
                SilindiMi = 0,
            };
            var PozisyonModel = new KurumlarKisiler()
            {
                KurumID = model.Kurum,
                KisiID = TemelBilgilerReturnModel.TabloID,
                IlgiliKisiId = TemelBilgilerReturnModel.TabloID,
                IlgiliKurumId = model.Kurum,
                KurumOrganizasyonBirimTanimId = model.Pozisyon,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                AtanmaZamani = DateTime.Now,
                AtanmaninSonlanmaZamani = DateTime.Now.AddYears(1),
                AktifMi = 1,
                SilindiMi = 0,
            };
            var RolModel = new KurumlarKisiler()
            {
                KurumID = model.Kurum,
                KisiID = TemelBilgilerReturnModel.TabloID,
                IlgiliKisiId = TemelBilgilerReturnModel.TabloID,
                IlgiliKurumId = model.Kurum,
                KurumOrganizasyonBirimTanimId = model.Rol,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                AtanmaZamani = DateTime.Now,
                AtanmaninSonlanmaZamani = DateTime.Now.AddYears(1),
                AktifMi = 1,
                SilindiMi = 0,
            };
            var LokasyonModel = new KurumlarKisiler()
            {
                KurumID = model.Kurum,
                KisiID = TemelBilgilerReturnModel.TabloID,
                IlgiliKisiId = TemelBilgilerReturnModel.TabloID,
                IlgiliKurumId = model.Kurum,
                KurumOrganizasyonBirimTanimId = model.Lokasyon,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                AtanmaZamani = DateTime.Now,
                AtanmaninSonlanmaZamani = DateTime.Now.AddYears(1),
                AktifMi = 1,
                SilindiMi = 0,
            };
            //Kurum kişi ilişki kaydı
            if (model.Departman > 0)
                KurumlarKisiler.Add(DepartmanModel);
            if (model.Pozisyon > 0)
                KurumlarKisiler.Add(PozisyonModel);
            if (model.Rol > 0)
                KurumlarKisiler.Add(RolModel);
            if (model.Lokasyon > 0)
                KurumlarKisiler.Add(LokasyonModel);
            var KurumOrganizasyonBirimTanimID = 0;
            foreach (var item in KurumlarKisiler)
            {
                _kurumlarKisilerService.Add(item);
                KurumOrganizasyonBirimTanimID = item.KurumOrganizasyonBirimTanimId;
            }

            /*
             * KurumlarKisiler tablosunda birder fazla aynı kişiye ait bilgi olabilir. 
             * Bu bilgileri Cookie ile session da tutacağız.
             */


            // KurumOrganizasyonBirimTanımID yabi TabloID si gelmeli.
            /*
            var BirimTanimID = 0;
            if (model.Departman > 0)
                BirimTanimID = (int)OrganizasyonBirimTipi.Departman;
            if (model.Pozisyon>0)
                BirimTanimID = (int)OrganizasyonBirimTipi.Pozisyon;
            if (model.Rol> 0)
                BirimTanimID = (int)OrganizasyonBirimTipi.Rol;
            if (model.Lokasyon> 0)
                BirimTanimID = (int)OrganizasyonBirimTipi.Lokasyon;

            Düzeltinmiş kod artık yukarıda çalışıyor

            */

            // Kaydedilen kişiye varsayılan bazı sayfalara erişim vermeliyiz. DashBoard bunun başında gelir. Bunu yetki merkezinden yapıyoruz.

            var ErisilecekSayfalar = new List<int>() { 10, 11, 18, 19, 254, 255 };
            var list = new List<ErisimYetkilendirmeTanimlari>();

            foreach (var sayfa in ErisilecekSayfalar)
            {
                var yetkiTanimi = new ErisimYetkilendirmeTanimlari()
                {
                    IlgiliKurumOrganizasyonBirimTanimiId = KurumOrganizasyonBirimTanimID,//  item.TabloID,
                    ErisimYetkisiVerilenSayfaId = sayfa,
                    KayitTarihi = DateTime.Now,
                    ErisimYetkisiVerilmeTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now,
                    AktifMi = 1,
                    SilindiMi = 0,
                    KayitEdenID = _loginUser.KisiID,
                    KurumID = model.Kurum,
                    KisiID = TemelBilgilerReturnModel.TabloID,
                    ErisimYetkisiVerenKisiId = _loginUser.KisiID,
                    IlgiliKurumId = model.Kurum,
                };
                list.Add(yetkiTanimi);
            }

            _yetkiMerkeziService.ErisimYetkilendirmeTanimlariKaydet(list);



            model.TemelBilgiTabloID = TemelBilgilerReturnModel.TabloID;
            DataContextConfiguration().Commit();
            //Session güncellenmesi
            var sessionId = _httpContextAccessor.HttpContext.Request.Headers["sessionid"][0];
            _requestHelper.Get<Result<bool>>(LocalPortlar.UserLoginregisterService + "/api/LoginRegister/KimlikGuncelle/" + sessionId);

            return model.ToResult();
        }



        /// <summary>
        /// Id ile kisi bilgilerinin getirildiği method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>

        public Result<KisiTemelKayitModel> KisiTemelKayitVerileriGetir(int kisiID)
        {
            var model = new KisiTemelKayitModel();
            var TemelBilgilerReturnModel = this.SingleOrDefault(kisiID).Value;
            if (TemelBilgilerReturnModel == null)
            {
                return Results.Fail("Kayıt bulunamadı.", ResultStatusCode.ReadError);
            }

            model.TemelBilgiTabloID = TemelBilgilerReturnModel.TabloID;
            model.Kurum = Convert.ToInt32(TemelBilgilerReturnModel.KisiBagliOlduguKurumId);
            model.Adi = TemelBilgilerReturnModel.KisiAdi;
            model.Soyadi = TemelBilgilerReturnModel.KisiSoyadi;
            model.EpostaAdresi = TemelBilgilerReturnModel.KisiEposta;
            model.Cinsiyeti = Convert.ToInt32(TemelBilgilerReturnModel.KisiCinsiyetId);
            model.EpostaAdresi = TemelBilgilerReturnModel.KisiEkranAdi;
            model.EpostaAdresi = TemelBilgilerReturnModel.KisiKullaniciAdi;
            model.IseGirisTarihi = Convert.ToDateTime(TemelBilgilerReturnModel.IseGirisTarihi);

            var KisiHassasBilgilerModel = _kisiHassasBilgilerService.KisiIdileGetir(kisiID).Value;
            model.TCKimlikNo = KisiHassasBilgilerModel.KisiKimlikNo;
            model.SicilNo = KisiHassasBilgilerModel.SicilNo;
            model.BabaAdi = KisiHassasBilgilerModel.BabaAdi;
            model.AnneAdi = KisiHassasBilgilerModel.AnneAdi;
            model.MedeniHali = Convert.ToInt32(KisiHassasBilgilerModel.MedeniHali);
            model.DogduguUlke = Convert.ToInt32(KisiHassasBilgilerModel.DogduguUlkeId);
            model.DogduguSehir = Convert.ToInt32(KisiHassasBilgilerModel.DogduguSehirId);
            model.DogumTarihi = Convert.ToDateTime(KisiHassasBilgilerModel.DogumTarihi);
            model.Dini = Convert.ToInt32(KisiHassasBilgilerModel.Dini);

            //Ek bilgilerin getirlmesi
            var OkulModelList = _kisiEgitimBilgileriService.KisiIdileGetir(kisiID)?.Value;
            model.OkulListesi = OkulModelList.Select(a => new OkulModel()
            {
                Fakulte = a.Fakulte,
                MezuniyetTarihi = Convert.ToDateTime(a.MezuniyetTarihi),
                OkulAdi = a.OkulAdi,
                OkulTipi = a.OkulTipiId
            }).ToList();

            var AdresModelList = _kisiAdresBilgileriService.KisiIdileGetir(kisiID)?.Value;
            model.AdresListesi = AdresModelList.Select(a => new AdresModel()
            {
                Adres = a.Adres,
                AdresTipi = a.AdresTipiId,
                Sehir = a.SehirId,
                Ulke = a.UlkeId
            }).ToList();



            var telefonModelList = _kisiTelefonBilgileriService.KisiIdileGetir(kisiID)?.Value;
            model.TelefonListesi = telefonModelList.Select(a => new TelefonModel()
            {
                TelefonNo = a.TelefonNo,
                TelefonTipi = a.TelefonTipiId
            }).ToList();

            var url = LocalPortlar.KurumService + "/api/OrganizasyonBirim/ListTip";
            var departmanReq = _requestHelper.Post<Result<List<KurumOrganizasyonBirimView>>>(url, new KurumOrganizasyonBirimRequest()
            {
                KurumId = model.Kurum,
                Name = "departman"
            }).Result.Value;
            var departmanList = departmanReq == null ? new List<int>() : departmanReq.Select(p => p.TabloId).ToList();

            var lokasyonReq = _requestHelper.Post<Result<List<KurumOrganizasyonBirimView>>>(url, new KurumOrganizasyonBirimRequest()
            {
                KurumId = model.Kurum,
                Name = "lokasyon"
            }).Result.Value;
            var lokasyonList = lokasyonReq == null ? new List<int>() : lokasyonReq.Select(p => p.TabloId).ToList();

            var pozisyonReq = _requestHelper.Post<Result<List<KurumOrganizasyonBirimView>>>(url, new KurumOrganizasyonBirimRequest()
            {
                KurumId = model.Kurum,
                Name = "pozisyon"
            }).Result.Value;
            var pozisyonList = pozisyonReq == null ? new List<int>() : pozisyonReq.Select(p => p.TabloId).ToList();

            var rolReq = _requestHelper.Post<Result<List<KurumOrganizasyonBirimView>>>(url, new KurumOrganizasyonBirimRequest()
            {
                KurumId = model.Kurum,
                Name = "rol"
            }).Result.Value;
            var rolList = rolReq == null ? new List<int>() : rolReq.Select(p => p.TabloId).ToList();

            //Kişinin ilişkilerine göre organizasyon birimlerinin getirilmesi
            var kurumlarKisiler = _kurumlarKisilerService.KisiIdileGetir(model.TemelBilgiTabloID).Value;
            foreach (var item in kurumlarKisiler)
            {
                if (item.KurumOrganizasyonBirimTanimId > 0)
                {
                    if (model.Departman == 0)
                        model.Departman = departmanList.FirstOrDefault(a => a == item.KurumOrganizasyonBirimTanimId);
                    if (model.Pozisyon == 0)
                        model.Pozisyon = pozisyonList.FirstOrDefault(a => a == item.KurumOrganizasyonBirimTanimId);
                    if (model.Lokasyon == 0)
                        model.Lokasyon = lokasyonList.FirstOrDefault(a => a == item.KurumOrganizasyonBirimTanimId);
                    if (model.Rol == 0)
                        model.Rol = rolList.FirstOrDefault(a => a == item.KurumOrganizasyonBirimTanimId);
                }
            }

            // Yabancı dil ve dil seviyesi bilgilerini DilListesi'ne ekle
            model.DilListesi = new List<DilModel>();
            var kisiDiller = _kisiBildigiDillerService.List(x => x.KisiID == kisiID && x.AktifMi == 1 && x.SilindiMi == 0).Value;
            model.DilListesi = kisiDiller
                .Select(d => new DilModel
                {
                    YabanciDilTipi = d.ParamYabanciDilID,
                    DilSeviye = d.ParamDilSeviyesiID.ToString()
                })
                .ToList();

            return model.ToResult();

        }

        /// <summary>
        /// Kişi bilgilerinin güncellendiği method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public Result<KisiTemelKayitModel> KisiTemelKayitVerileriGuncelle(KisiTemelKayitModel model)
        {
            DataContextConfiguration().BeginNewTransactionIsNotFound();
            var kurum = _kurumlarService.SingleOrDefault(model.Kurum);
            var KisiTemelBilgilerModel = this.SingleOrDefault(model.TemelBilgiTabloID).Value;
          
            KisiTemelBilgilerModel.TabloID = model.TemelBilgiTabloID;
            KisiTemelBilgilerModel.KisiBagliOlduguKurumId = model.Kurum;
            KisiTemelBilgilerModel.KisiAdi = model.Adi;
            KisiTemelBilgilerModel.KisiSoyadi = model.Soyadi;
            KisiTemelBilgilerModel.KisiEposta = model.EpostaAdresi;
            KisiTemelBilgilerModel.KisiCinsiyetId = model.Cinsiyeti;
            KisiTemelBilgilerModel.KisiEkranAdi = model.EpostaAdresi;
            KisiTemelBilgilerModel.KisiKullaniciAdi = model.EpostaAdresi;
            KisiTemelBilgilerModel.DilID = kurum.Value.DilID;
            KisiTemelBilgilerModel.GuncellenmeTarihi = DateTime.Now;
            KisiTemelBilgilerModel.IseGirisTarihi = Convert.ToDateTime(model.IseGirisTarihi).Year == 1 ? null : model.IseGirisTarihi;
            KisiTemelBilgilerModel.KayitTarihi = DateTime.Now;
            KisiTemelBilgilerModel.GuncellenmeTarihi = DateTime.Now;
            var TemelBilgilerReturnModel = this.Update(KisiTemelBilgilerModel).Value;
            
            var KisiHassasBilgilerModel = _kisiHassasBilgilerService.KisiIdileGetir(model.TemelBilgiTabloID).Value;
            KisiHassasBilgilerModel.KurumID = model.Kurum;
            KisiHassasBilgilerModel.KisiID = TemelBilgilerReturnModel.TabloID;
            KisiHassasBilgilerModel.KisiTemelBilgiId = TemelBilgilerReturnModel.TabloID;
            KisiHassasBilgilerModel.KisiKimlikNo = model.TCKimlikNo;
            KisiHassasBilgilerModel.SicilNo = model.SicilNo;
            KisiHassasBilgilerModel.BabaAdi = model.BabaAdi;
            KisiHassasBilgilerModel.AnneAdi = model.AnneAdi;
            KisiHassasBilgilerModel.MedeniHali = model.MedeniHali;
            KisiHassasBilgilerModel.DogduguUlkeId = model.DogduguUlke;
            KisiHassasBilgilerModel.DogduguSehirId = model.DogduguSehir == null ? 0 : model.DogduguSehir.Value;
            KisiHassasBilgilerModel.DogumTarihi = Convert.ToDateTime(model.DogumTarihi).Year == 1 ? null : model.DogumTarihi;
            KisiHassasBilgilerModel.KayitTarihi = DateTime.Now;
            KisiHassasBilgilerModel.GuncellenmeTarihi = DateTime.Now;
            KisiHassasBilgilerModel.KisiPasaportNo = "";
            KisiHassasBilgilerModel.Dini = model.Dini;
            _kisiHassasBilgilerService.Update(KisiHassasBilgilerModel);
            //okul listesi için değişkenleri getirip güncelleme methodu
            var okulReturnList = _kisiEgitimBilgileriService.KisiIdileGetir(model.TemelBilgiTabloID).Value;

            foreach (var item in okulReturnList)
            {
                _kisiEgitimBilgileriService.Delete(item.TabloID);
            }

            foreach (var item in model.OkulListesi)
            {
                var KisiEgitimGecmisi = new KisiEgitimBilgileri()
                {
                    KurumID = model.Kurum,
                    KisiID = TemelBilgilerReturnModel.TabloID,
                    KisiTemelBilgiId = TemelBilgilerReturnModel.TabloID,
                    OkulTipiId = item.OkulTipi,
                    OkulAdi = item.OkulAdi,
                    Fakulte = item.Fakulte,
                    MezuniyetTarihi = Convert.ToDateTime(item.MezuniyetTarihi).Year == 1 ? null : item.MezuniyetTarihi,//Convert.ToDateTime(item.MezuniyetTarihi),
                    KayitTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now
                };
                _kisiEgitimBilgileriService.Add(KisiEgitimGecmisi);
            }

            //adres listesi için değişkenleri getirip güncelleme methodu
            var adresReturnList = _kisiAdresBilgileriService.KisiIdileGetir(model.TemelBilgiTabloID).Value;

            foreach (var item in adresReturnList)
            {
                _kisiAdresBilgileriService.Delete(item.TabloID);
            }
            foreach (var item in model.AdresListesi)
            {
                var KisiAdresModel = new KisiAdresBilgileri()
                {
                    KurumID = model.Kurum,
                    KisiID = TemelBilgilerReturnModel.TabloID,
                    KisiTemelBilgiId = TemelBilgilerReturnModel.TabloID,
                    AdresTipiId = item.AdresTipi,
                    Adres = item.Adres,
                    UlkeId = item.Ulke == null ? 0 : item.Ulke.Value,
                    SehirId = item.Sehir == null ? 0 : item.Sehir.Value,
                    KayitTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now
                };
                _kisiAdresBilgileriService.Add(KisiAdresModel);
            }

            //telefon listesi için değişkenleri getirip güncelleme methodu
            var telefonReturnList = _kisiTelefonBilgileriService.KisiIdileGetir(model.TemelBilgiTabloID).Value;

            foreach (var item in telefonReturnList)
            {
                _kisiTelefonBilgileriService.Delete(item.TabloID);
            }
            foreach (var item in model.TelefonListesi)
            {
                var kisiTelefonModel = new KisiTelefonBilgileri()
                {
                    KurumID = model.Kurum,
                    KisiID = TemelBilgilerReturnModel.TabloID,
                    KisiTemelBilgiId = TemelBilgilerReturnModel.TabloID,
                    TelefonTipiId = item.TelefonTipi,
                    TelefonNo = item.TelefonNo,
                    KayitTarihi = DateTime.Now,
                    GuncellenmeTarihi = DateTime.Now
                };
                _kisiTelefonBilgileriService.Add(kisiTelefonModel);
            }
            //Var olan ilişkilerin pasife alınması
            var kisiOrgList = _kurumlarKisilerService.KisiIdileGetir(model.TemelBilgiTabloID).Value;

            foreach (var item in kisiOrgList)
            {
                _kurumlarKisilerService.Delete(item.TabloID);
            }
            var KurumlarKisiler = new List<KurumlarKisiler>();

            var DepartmanModel = new KurumlarKisiler()
            {
                KurumID = model.Kurum,
                KisiID = TemelBilgilerReturnModel.TabloID,
                IlgiliKisiId = TemelBilgilerReturnModel.TabloID,
                IlgiliKurumId = model.Kurum,
                KurumOrganizasyonBirimTanimId = model.Departman,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                AtanmaZamani = DateTime.Now,
                AtanmaninSonlanmaZamani = DateTime.Now.AddYears(1),
                AktifMi = 1,
                SilindiMi = 0,
            };
            var PozisyonModel = new KurumlarKisiler()
            {
                KurumID = model.Kurum,
                KisiID = TemelBilgilerReturnModel.TabloID,
                IlgiliKisiId = TemelBilgilerReturnModel.TabloID,
                IlgiliKurumId = model.Kurum,
                KurumOrganizasyonBirimTanimId = model.Pozisyon,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                AtanmaZamani = DateTime.Now,
                AtanmaninSonlanmaZamani = DateTime.Now.AddYears(1),
                AktifMi = 1,
                SilindiMi = 0,
            };
            var RolModel = new KurumlarKisiler()
            {
                KurumID = model.Kurum,
                KisiID = TemelBilgilerReturnModel.TabloID,
                IlgiliKisiId = TemelBilgilerReturnModel.TabloID,
                IlgiliKurumId = model.Kurum,
                KurumOrganizasyonBirimTanimId = model.Rol,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                AtanmaZamani = DateTime.Now,
                AtanmaninSonlanmaZamani = DateTime.Now.AddYears(1),
                AktifMi = 1,
                SilindiMi = 0,
            };
            var LokasyonModel = new KurumlarKisiler()
            {
                KurumID = model.Kurum,
                KisiID = TemelBilgilerReturnModel.TabloID,
                IlgiliKisiId = TemelBilgilerReturnModel.TabloID,
                IlgiliKurumId = model.Kurum,
                KurumOrganizasyonBirimTanimId = model.Lokasyon,
                KayitTarihi = DateTime.Now,
                GuncellenmeTarihi = DateTime.Now,
                AtanmaZamani = DateTime.Now,
                AtanmaninSonlanmaZamani = DateTime.Now.AddYears(1),
                AktifMi = 1,
                SilindiMi = 0,
            };

            if (model.Departman > 0)
                KurumlarKisiler.Add(DepartmanModel);
            if (model.Pozisyon > 0)
                KurumlarKisiler.Add(PozisyonModel);
            if (model.Rol > 0)
                KurumlarKisiler.Add(RolModel);
            if (model.Lokasyon > 0)
                KurumlarKisiler.Add(LokasyonModel);

            //İlişkilerin güncellenmesi
            foreach (var item in KurumlarKisiler)
            {
                _kurumlarKisilerService.Add(item);
            }
            //KisiBildigiDillerTablosu
            var eskiDiller = _kisiBildigiDillerService.List(x => x.KisiID == model.TemelBilgiTabloID).Value;
            foreach (var eskiDil in eskiDiller)
            {
                eskiDil.AktifMi = 0;
                eskiDil.SilindiMi = 1;
                eskiDil.GuncellenmeTarihi = DateTime.Now;
                eskiDil.GuncelleyenKisiID = _loginUser.KisiID;
                _kisiBildigiDillerService.Update(eskiDil);
            }

            // Yeni kayıtları ekle
            if (model.DilListesi != null && model.DilListesi.Any())
            {
                foreach (var dil in model.DilListesi)
                {
                    var kisiDilBilgisi = new KisiBildigiDiller()
                    {
                        KurumID = model.Kurum,
                        KisiID = model.TemelBilgiTabloID,
                        KisiTemelBilgiID = model.TemelBilgiTabloID,
                        ParamYabanciDilID = dil.YabanciDilTipi,
                        ParamDilSeviyesiID = Convert.ToInt16(dil.DilSeviye),
                        KayitTarihi = DateTime.Now,
                        GuncellenmeTarihi = DateTime.Now,
                        AktifMi = 1,
                        SilindiMi = 0
                    };
                    _kisiBildigiDillerService.Add(kisiDilBilgisi);
                }
            }
            var returnResult = this.KisiTemelKayitVerileriGetir(model.TemelBilgiTabloID);
            DataContextConfiguration().Commit();
            return returnResult;
        }

        /// <summary>
        /// Id Lere göre kişileri getirir
        /// </summary>
        /// <param name="model"> ilgili kurumId</param>
        /// <returns>listelenen kişileri döndürür.</returns>

        public Result<List<KisiTemelBilgiler>> IdlereGoreKisileriGetir(List<int> model)
        {
            return this.List(x => model.Contains(x.TabloID) && x.AktifMi == 1);
        }

        /// <summary>
        /// Kuruma bağlı kişilerin listelendiği method
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns>listelenen kişileri döndürür.</returns>

        public Result<List<KisiListeModel>> KisiListesiGetir(int kurumId)
        {
            var temelbilgiList = this.List(p => p.AktifMi == 1 && p.SilindiMi == 0 && p.KurumID == kurumId).Value;
            var kisiListModel = new List<KisiListeModel>();
            temelbilgiList = KisiResimUrlGetir(temelbilgiList);
            foreach (var kisi in temelbilgiList)
            {
                var kurumModel = _kurumlarService.SingleOrDefault(kisi.KisiBagliOlduguKurumId.Value).Value;
                if (kurumModel != null)
                {
                    var kisiListItem = new KisiListeModel()
                    {
                        TabloID = kisi.TabloID,
                        KisiAdi = kisi.KisiAdi,
                        KisiSoyadi = kisi.KisiSoyadi,
                        KisiKullaniciAdi = kisi.KisiKullaniciAdi,
                        KisiEkranAdi = kisi.KisiEkranAdi,
                        KisiEposta = kisi.KisiEposta,
                        KurumAdi = kurumModel.KurumTicariUnvani,
                        KurumID = Convert.ToInt32(kisi.KisiBagliOlduguKurumId),
                        KisiResimUrl = kisi.KisiResimUrl,
                        sifreVarMi = _loginUser.KurumID != kisi.KurumID,
                        KisiBagliOlduguKurumId = kisi.KisiBagliOlduguKurumId
                    };
                    kisiListModel.Add(kisiListItem);
                }
            }
            return kisiListModel.ToResult();
        }

        /// <summary>
        /// Kurum Id'ye göre Pasif kişileri getirem metod
        /// </summary>
        /// <returns></returns>

        public Result<List<KisiTemelBilgiler>> PasifKisiListesiGetir()
        {
            List<KisiOrganizasyonBirimView> amir = new();
            Amirler(_loginUser.KisiID, amir);
            if (amir.Count > 0)
            {
                return new List<KisiTemelBilgiler>().ToResult();
            }
            var toplamKisi = new List<KisiTemelBilgiler>();

            //Hesabı silinmiş kişilerin getirildği sorgu
            var kisi = (from temelBilgiler in _repository.List()
                        join hassasBilgiler in _kisiHassasBilgilerService.ListForQuery() on temelBilgiler.TabloID equals hassasBilgiler.KisiTemelBilgiId
                        where temelBilgiler.SilindiMi == 1 && hassasBilgiler.HesabiAktifMi == false && temelBilgiler.KurumID == _loginUser.KurumID
                        select temelBilgiler
                ).ToList();

            if (kisi.Count > 0)
            {
                toplamKisi.AddRange(kisi);
            }
            //Hesabı pasife alınmış kişilerin getirildği sorgu
            var kisi2 = (from temelBilgiler in _repository.List()
                         join hassasBilgiler in _kisiHassasBilgilerService.ListForQuery() on temelBilgiler.TabloID equals hassasBilgiler.KisiTemelBilgiId
                         where temelBilgiler.AktifMi == 1 && hassasBilgiler.HesabiAktifMi == false && temelBilgiler.KurumID == _loginUser.KurumID
                         select temelBilgiler
                ).ToList();

            if (kisi2.Count > 0)
            {
                toplamKisi.AddRange(kisi2);
            }

            return toplamKisi.ToResult();
        }

        /// <summary>
        /// Temel kişi kayıt ile eklenmiş kişilerin silindiği method
        /// </summary>
        /// <param name="kisiID"> silinecek kişiID</param>
        /// <returns></returns>

        public Result<bool> TemelKisiSilindiYap(int kisiID)
        {
            this.KisiBilgileriSilme(kisiID);
            return true.ToResult();
        }

        /// <summary>
        /// Kişilerin aktifleştirildiği method
        /// </summary>
        /// <param name="kisiID"> aktifleşecek kişiID</param>
        /// <returns></returns>

        public Result<bool> TemelKisiAktifYap(int kisiID)
        {
            this.KisiBilgileriAktiflestirme(kisiID);

            _requestHelper.Get<Result<SistemLoginSonDurum>>(LocalPortlar.UserLoginregisterService + "/api/LoginRegister/BasarisizLoginSifirla/" + kisiID);
            return true.ToResult();
        }

        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin astlarını getiren method
        /// </summary>
        /// <param name="kisiID"> astları getirilecek kişiId</param>
        /// <returns>kişinin astları listesi.</returns>

        public Result<List<KisiOrganizasyonBirimView>> KisiAstlarListGetir(int kisiID)
        {
            List<KisiOrganizasyonBirimView> altbirimler = new();
            Aslar(kisiID, altbirimler);
            return altbirimler.ToResult();
        }

        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin astlarının astlarını getiren method
        /// </summary>
        /// <param name="kisiID"> astları getirilecek kişiId</param>
        /// <param name="altbirimler"></param>
        /// <returns>kişinin astları listesi.</returns>
        private void Aslar(int kisiID, List<KisiOrganizasyonBirimView> altbirimler)
        {
            var kisiBilgileri = this.SingleOrDefault(kisiID).Value;
            if (kisiBilgileri == null)
            {
                return;
            }
            var kurumPozisyonlari = _kurumlarKisilerService.PozisyonlarList(kisiBilgileri.KurumID).Value;
            if (kurumPozisyonlari == null)
            {
                return;
            }
            var kisiPozisyonTanimId = _kurumlarKisilerService.List(s => s.IlgiliKisiId == kisiID && s.AktifMi == 1 && s.KurumOrganizasyonBirimTanimId != 0).Value.Select(a => a.KurumOrganizasyonBirimTanimId).ToList();
            if (kisiPozisyonTanimId == null)
            {
                return;
            }
            //Kişinin organizasyon ağacındaki pozisyona göre altındaki roldeki kişileri getirir.
            foreach (var item in kurumPozisyonlari.Where(a => kisiPozisyonTanimId.Contains(a.UstId)))
            {
                var altKisiPozisyonlar = _kurumlarKisilerService.List(w => w.KurumOrganizasyonBirimTanimId == item.TabloId && w.AktifMi == 1 && !altbirimler.Select(a => a.KisiId).Contains(w.IlgiliKisiId)).Value;
                foreach (var altKisiPozisyon in altKisiPozisyonlar)
                {
                    if (altKisiPozisyon != null)
                    {
                        if (altKisiPozisyon.IlgiliKisiId == kisiID)
                            break;
                        var kisiTemelBilgi = this.SingleOrDefault(altKisiPozisyon.IlgiliKisiId).Value;
                        if (kisiTemelBilgi != null && kisiTemelBilgi.TabloID > 0)
                        {
                            var altbirim = new KisiOrganizasyonBirimView()
                            {
                                KisiAdi = kisiTemelBilgi.KisiAdi,
                                KisiSoyadi = kisiTemelBilgi.KisiSoyadi,
                                KurumId = kisiTemelBilgi.KurumID,
                                TipId = item.TabloId,
                                Tanim = item.Tanim,
                                UstId = item.UstId,
                                KisiId = kisiTemelBilgi.TabloID,
                                KisiUstler = new List<KisiOrganizasyonBirimView>(),
                                KisiAstlar = new()
                            };
                            altbirimler.Add(altbirim);
                            Aslar(kisiTemelBilgi.TabloID, altbirimler);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin amirlerini getiren method
        /// </summary>
        /// <param name="kisiID"> amirleri getirilecek kişiId</param>
        /// <returns>kişinin amirlerinin listesi</returns>

        public Result<List<KisiOrganizasyonBirimView>> KisiAmirlerListGetir(int kisiID)
        {
            List<KisiOrganizasyonBirimView> ustBirimler = new();
            Amirler(kisiID, ustBirimler);
            return ustBirimler.ToResult();
        }
        public Result<List<KurumOrganizasyonBirimTanimlari>> KisiOrganizasyonListGetir(int kisiID)
        {
            List<int> tanimList = new List<int>();
            var tanimlar = _kurumlarKisilerService.List(x => x.IlgiliKisiId == kisiID).Value;
            if (tanimlar != null)
            {
                foreach (var tanim in tanimlar)
                {
                    tanimList.Add(tanim.KurumOrganizasyonBirimTanimId);
                }
            }
            var birimler = _kurumOrganizasyonBirimTanimlariService.List(x => tanimList.Contains(x.TabloID)).Value;
            return birimler.ToResult();
        }


        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin amirlerinin amirlerini getiren method
        /// </summary>
        /// <param name="kisiID"> amirleri getirilecek kişiId</param>
        /// <param name="ustBirimler"></param>
        /// <returns>kişinin amirlerinin listesi</returns>
        private void Amirler(int kisiID, List<KisiOrganizasyonBirimView> ustBirimler)
        {
            var _kurumOrganizasyon = _serviceProvider.GetService<IKurumOrganizasyonBirimTanimlariService>();
            var kisiBilgileri = this.SingleOrDefault(kisiID).Value;
            if (kisiBilgileri == null)
            {
                return;
            }
            //Kişinin kurumdaki organizasyonIdleri sorgusu
            var kisiOrgBirimleri = _kurumlarKisilerService.List(s => s.IlgiliKisiId == kisiID && s.KurumID == kisiBilgileri.KurumID && s.AktifMi == 1).Value;
            if (kisiOrgBirimleri == null)
            {
                return;
            }
            var kurumPozisyonlari = _kurumlarKisilerService.PozisyonlarList(kisiBilgileri.KurumID).Value;
            if (kurumPozisyonlari == null)
            {
                return;
            }
            //Kurumdaki pozisyonlar içinde kişinin organziasyon ıdlerini filtreleme

            var x = kurumPozisyonlari.Where(a => kisiOrgBirimleri.Select(a => a.KurumOrganizasyonBirimTanimId).Contains(a.TabloId)).ToList();
            //Kişinin amir pozisyonlarına göre amirleri döndüren döngü
            foreach (var item in x)
            {
                var AmirPozisyonlar = _kurumlarKisilerService.List(w => w.KurumOrganizasyonBirimTanimId == item.UstId && item.UstId != 0 && w.AktifMi == 1 && !ustBirimler.Select(a => a.KisiId).Contains(w.IlgiliKisiId)).Value;
                foreach (var AmirPozisyon in AmirPozisyonlar)
                {
                    if (AmirPozisyon != null)
                    {
                        if (AmirPozisyon.IlgiliKisiId == kisiID)
                            break;
                        var kisiTemelBilgi = this.SingleOrDefault(AmirPozisyon.IlgiliKisiId).Value;
                        var amirPozisyonData = _kurumOrganizasyon.List(a => a.TabloID == AmirPozisyon.KurumOrganizasyonBirimTanimId && a.AktifMi == 1).Value;

                        if (kisiTemelBilgi != null && amirPozisyonData != null)
                        {
                            var ustBirim = new KisiOrganizasyonBirimView()
                            {
                                KisiAdi = kisiTemelBilgi.KisiAdi,
                                KisiSoyadi = kisiTemelBilgi.KisiSoyadi,
                                KurumId = kisiTemelBilgi.KurumID,
                                TipId = item.TabloId,
                                Tanim = amirPozisyonData.FirstOrDefault().BirimTanim,
                                UstId = item.UstId,
                                KisiId = kisiTemelBilgi.TabloID,
                                KisiAstlar = new List<KisiOrganizasyonBirimView>(),
                                KisiUstler = new List<KisiOrganizasyonBirimView>()
                            };
                            ustBirimler.Add(ustBirim);
                            Amirler(kisiTemelBilgi.TabloID, ustBirimler);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// KisiId değeri ile ilgili kişinin sifre son yenileme tarihi değerini getiren method
        /// </summary>
        /// <param name="kisiID"> ilgili kişiId</param>
        /// <returns>şifre son yenileme tarihi</returns>

        public Result<DateTime> KisiSifreSonYenilemeTarihiGetir(int kisiID)
        {
            var hassasData = _kisiHassasBilgilerService.KisiIdileGetir(kisiID).Value;
            if (hassasData.SifreSonYenilemeTarihi == null)
            {
                var sifreYenilemeZamaniParam = _kureselParametrelerService.ZorunluSifreYenilemeAraligiGetir("ZorunluŞifreYenilemeAralığı").Value;
                var paramDeger = Convert.ToDouble(sifreYenilemeZamaniParam.ParametreBaslangicDegeri);
                hassasData.SifreSonYenilemeTarihi = DateTime.Now.AddDays(-(paramDeger));
                _kisiHassasBilgilerService.Update(hassasData);
                return Convert.ToDateTime(hassasData.SifreSonYenilemeTarihi).ToResult();
            }
            return Convert.ToDateTime(hassasData.SifreSonYenilemeTarihi).ToResult();
        }

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini pasif eden method.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Pasifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>

        public Result<KisiTemelBilgiler> KisiHesabiPasifEtme(int id)
        {
            var temelBilgilerData = List(a => a.TabloID == id).Value.SingleOrDefault();
            if (temelBilgilerData == null)
            {
                return Results.Fail("Kişi bulunamadı.", ResultStatusCode.ReadError);
            }
            var hassasBilgilerData = _kisiHassasBilgilerService.List(a => a.KisiTemelBilgiId == id).Value.SingleOrDefault();

            if (hassasBilgilerData != null)
            {
                hassasBilgilerData.HesabiAktifMi = false;
                hassasBilgilerData.HesapDeAktifTarihi = DateTime.Now;
                _kisiHassasBilgilerService.Update(hassasBilgilerData);
                return temelBilgilerData.ToResult();
            }
            else
            {
                return Results.Fail("Hassas bilgiler tablosunda kayıt bulunamadı.", ResultStatusCode.ReadError);
            }
        }

        /// <summary>
        ///  Kurum idleri ve filter ile key value listesi getirme
        /// </summary>
        /// <param name="comparisonType"></param>
        /// <param name="buildPredicateModels"></param>
        /// <param name="_kurumIdleri"></param>
        /// <returns></returns>
        public Result<List<KeyValueModel>> List(ComparisonType comparisonType, List<BuildPredicateModel> buildPredicateModels, List<int> _kurumIdleri)
        {
            var result = _repository.List(x => x.AktifMi == 1 && _kurumIdleri.Contains(x.KisiBagliOlduguKurumId.Value)).AsQueryable().WhereBuilder(comparisonType, buildPredicateModels).ToList();
            if (result == null || result.Count == 0)
                return new List<KeyValueModel>().ToResult();
            return result.Select(p => new KeyValueModel() { Key = p.KisiAdi + " " + p.KisiSoyadi, Value = p.TabloID.ToString() }).ToList().ToResult();
        }

        /// <summary>
        /// Kurum bağlı kişileri kurumIdye göre getiren metod
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns></returns>

        public Result<List<KisiTemelBilgiler>> KurumaBagliKisilerList(int kurumId)
        {
            var kurumIds = _kurumlarService.ListForAltKurum(kurumId).Value;

            if (!kurumIds.Contains(kurumId))
                kurumIds.Add(kurumId);
            //Kuruma bağlı kişileri getiren sorgu
            var users = (from user in _repository.List(t => t.AktifMi == 1)
                         where kurumIds.Contains(user.KisiBagliOlduguKurumId.Value)
                         select user
                ).ToList();
            users = KisiResimUrlGetir(users);
            return users.ToResult();
        }

        /// <summary>
        /// Kişilerin resim urlleri ile birlikte döndüren met
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private List<KisiTemelBilgiler> KisiResimUrlGetir(List<KisiTemelBilgiler> users)
        {
            var medyaKutuphanesiService = _serviceProvider.GetService<IMedyaKutuphanesiService>();
            foreach (var user in users)
            {
                if (user.KisiResimId > 0)
                {
                    var ppResult = medyaKutuphanesiService.SingleOrDefault(user.KisiResimId);
                    if (ppResult.IsSuccess && ppResult.Value != null)
                        user.KisiResimUrl = ppResult.Value.MedyaUrl;
                }
            }
            return users;
        }

        /// <summary>
        /// Excel dosyasından import edilen kişi listesinin kaydedilmesi.
        /// </summary>
        /// <param name="list"> Kişi listesi</param>
        /// <returns>kaydedilen kişileri döndürür</returns>

        public Result<List<KisiTemelKayitModel>> ListeIleTemelKisikaydet(List<KisiTemelKayitModel> list)
        {
            var result = new List<KisiTemelKayitModel>();
            foreach (var item in list)
            {
                var add = this.KisiTemelKaydet(item);
                if (add.Value != null)
                    result.Add(add.Value);
            }
            return result.ToResult();
        }

        /// <summary>
        /// Idye göre kişi getirme
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        public Result<KisiTemelBilgiler> IdyeGoreKisiGetir(int kisiID)
        {
            var result = SingleOrDefault(kisiID).Value;
            if (result == null)
            {
                return Results.Fail("Getirme işlemi başarısızdır.", ResultStatusCode.ReadError);
            }

            return result.ToResult();
        }

        /// <summary>
        /// Müşteri temsilcisine bağlı kişileri getirme
        /// </summary>
        /// <param name="musteriTemsilcisiId"></param>
        /// <returns></returns>
        public Result<List<KisiListeModel>> MusteriTemsilcisiBagliKisilerList(int musteriTemsilcisiId)
        {
            var _iliskiler = _serviceProvider.GetService<IKisiIliskiService>();
            var kurumlarList = _iliskiler.MusteriTemsilcisiBagliKurumGetir(musteriTemsilcisiId);
            List<KisiListeModel> kisiler = new();
            foreach (var kurumId in kurumlarList.Value)
            {
                var tempKisiler = KisiListesiGetir(kurumId);
                kisiler.AddRange(tempKisiler.Value);
            }
            if (kisiler.Count == 0)
            {
                var kisi = SingleOrDefaultListeModel(musteriTemsilcisiId).Value;
                if (kisi != null)
                    kisiler.Add(kisi);
            }
            return kisiler.Distinct().ToList().ToResult();
        }

        /// <summary>
        /// Id Lere göre kişileri KisiListeModel türünde getirir.
        /// </summary>
        /// <param name="model"> ilgili kisi Idleri</param>
        /// <returns>listelenen kişileri döndürür.</returns>

        public Result<List<KisiListeModel>> IdlereGoreKisiListeModelGetir(List<int> model)
        {
            var list = _repository.List(x => model.Contains(x.TabloID) && x.AktifMi == 1).ToList();
            var kisiListModel = new List<KisiListeModel>();
            list = KisiResimUrlGetir(list);
            foreach (var kisi in list)
            {
                var kurumModel = _kurumlarService.SingleOrDefault(kisi.KisiBagliOlduguKurumId.Value).Value;
                if (kurumModel != null)
                {
                    var kisiListItem = new KisiListeModel()
                    {
                        TabloID = kisi.TabloID,
                        KisiAdi = kisi.KisiAdi,
                        KisiSoyadi = kisi.KisiSoyadi,
                        KisiKullaniciAdi = kisi.KisiKullaniciAdi,
                        KisiEkranAdi = kisi.KisiEkranAdi,
                        KisiEposta = kisi.KisiEposta,
                        KurumID = kisi.KurumID,
                        KurumAdi = kurumModel.KurumTicariUnvani,
                        KisiResimUrl = kisi.KisiResimUrl,
                        KisiBagliOlduguKurumId = kisi.KisiBagliOlduguKurumId
                    };
                    kisiListModel.Add(kisiListItem);
                }
            }
            return kisiListModel.ToResult();
        }

        /// <summary>
        /// Kisi Id ile liste model şeklinde kişi getirme
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        public Result<KisiListeModel> SingleOrDefaultListeModel(int kisiId)
        {
            var kisiListItem = new KisiListeModel();
            var kisi = base.SingleOrDefault(kisiId).Value;
            var kurumModel = _kurumlarService.SingleOrDefault(kisi.KurumID).Value;
            if (kurumModel != null)
            {
                kisiListItem.TabloID = kisi.TabloID;
                kisiListItem.KisiAdi = kisi.KisiAdi;
                kisiListItem.KisiSoyadi = kisi.KisiSoyadi;
                kisiListItem.KisiKullaniciAdi = kisi.KisiKullaniciAdi;
                kisiListItem.KisiEkranAdi = kisi.KisiEkranAdi;
                kisiListItem.KisiEposta = kisi.KisiEposta;
                kisiListItem.KurumAdi = kurumModel.KurumTicariUnvani;
            }
            return kisiListItem.ToResult();
        }

        /// <summary>
        /// Amir Id ile müsteri temsilcilerini getirme
        /// </summary>
        /// <param name="amirId"></param>
        /// <returns></returns>

        public Result<List<KisiListeModel>> AmirlereAstMusteriTemsilcisiKisileriniGetir(int amirId)
        {
            Result<List<KisiOrganizasyonBirimView>> astlarList = KisiAstlarListGetir(amirId);
            List<KisiListeModel> result = new();
            var astlar = IdlereGoreKisiListeModelGetir(astlarList.Value.Select(a => a.KisiId).ToList());
            result.AddRange(astlar.Value);
            //Amir müşteri temsilci ise müşteri temsilcisine bağlı kişileri getirir
            if (_kurumlarKisilerService.KisiMusteriTemsilcisiMi(amirId).Value)
            {
                var temp = MusteriTemsilcisiBagliKisilerList(amirId);
                result.AddRange(temp.Value);
            }
            //Kişinin tüm astlarını getiren döngü.
            foreach (var ast in astlarList.Value)
            {
                var kontrol = _kurumlarKisilerService.KisiMusteriTemsilcisiMi(ast.KisiId);
                if (!kontrol.Value) continue;
                var temp1 = MusteriTemsilcisiBagliKisilerList(ast.KisiId);
                if (ast.KisiAstlar.Count > 0)
                {
                    var temp2 = AmirlereAstMusteriTemsilcisiKisileriniGetir(ast.KisiId);
                    result.AddRange(temp2.Value);
                }
                result.AddRange(temp1.Value);
            }
            int i = 1;
            //Tablo ıdye göre distinct yapan döngü
            while (i < result.Count)
            {
                int j = 0;
                bool remove = false;
                while (j < i && !remove)
                {
                    if (result[i].TabloID.Equals(result[j].TabloID))
                    {
                        remove = true;
                    }
                    j++;
                }
                if (remove)
                {
                    result.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            return result.ToResult();
        }

        /// <summary>
        /// kisi Id ile müsteri temsilcileri getirme
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        public Result<List<int>> AmireBagliMusteriTemsilcileriList(int kisiId)
        {
            List<int> result = new();
            var astlar = KisiAstlarListGetir(kisiId);
            foreach (var ast in astlar.Value)
            {
                var kontrol = _kurumlarKisilerService.KisiMusteriTemsilcisiMi(ast.KisiId);
                if (kontrol.Value)
                    result.Add(ast.KisiId);
            }
            return result.ToResult();
        }

        /// <summary>
        /// kisi Id mile amir veya müsteri temsilcisi listeleme
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        public Result<List<int>> AmirveyaMusteriTemsilcisiKurumlariIDGetir(int kisiId)
        {
            var amirMi = AmireBagliMusteriTemsilcileriList(kisiId).Value.Count > 0;
            var musteriTemsilcisiMi = _kurumlarKisilerService.KisiMusteriTemsilcisiMi(kisiId).Value;
            if (amirMi)
            {
                var result = _kurumlarService.AmirlereAstMusteriTemsilcisiKurumlariniGetir(kisiId);
                return result.Value.Select(a => a.TabloID).ToList().ToResult();
            }
            else if (musteriTemsilcisiMi)
            {
                var result = _kurumlarService.MusteriTemsilcisiBagliKurumlarList(kisiId);
                return result.Value.Select(a => a.TabloID).ToList().ToResult();
            }
            else
            {
                var kurum = base.SingleOrDefault(kisiId).Value == null ? 0 : base.SingleOrDefault(kisiId).Value.KurumID;
                if (kurum > 0)
                {
                    var list = _kurumlarService.List(a => a.AktifMi == 1 && a.KurumID == kurum).Value.Select(prop => prop.TabloID).ToList();
                    return list.ToResult(); // new List<int>() { kurum }.ToResult()
                }

                return new List<int>().ToResult();
            }
        }

        /// <summary>
        /// Pozisyona bağlı hiyerarşik ağaçta ast-üst ilişkisi bulunmayan ancak ilgili kişilere irişmesi gereken kullanıcılar için kullanılacak kişi listesi metodu.
        /// </summary>
        /// <returns>KisiListeModel listesi döndürür. <see cref="KisiListeModel"></see></returns>
        public Result<List<KisiListeModel>> HiyerarsiDisiKisilerKisiListesi()
        {
            var login = _loginUser.KurumID;
            var kurumIdList = _kurumlarService.KurumaBagliKurumIdleriList(login).Value;
            var temelbilgiList = this.List(p => p.AktifMi == 1 && p.SilindiMi == 0 && kurumIdList.Contains(p.KurumID)).Value;
            var kisiListModel = new List<KisiListeModel>();
            var amirler = KisiAmirlerListGetir(_loginUser.KisiID);
            var amirVarmi = amirler.Value.Any();
            temelbilgiList = KisiResimUrlGetir(temelbilgiList);
            foreach (var kisi in temelbilgiList)
            {
                var kisiHassasBilgi = _kisiHassasBilgilerService.List(x => x.KisiID == kisi.TabloID).Value.FirstOrDefault();
                var kurumModel = _kurumlarService.SingleOrDefault(kisi.KisiBagliOlduguKurumId.Value).Value;
                if (kurumModel != null)
                {
                    var kisiListItem = new KisiListeModel()
                    {
                        TabloID = kisi.TabloID,
                        KisiAdi = kisi.KisiAdi,
                        KisiSoyadi = kisi.KisiSoyadi,
                        KisiKullaniciAdi = kisi.KisiKullaniciAdi,
                        KisiEkranAdi = kisi.KisiEkranAdi,
                        KisiEposta = kisi.KisiEposta,
                        KurumAdi = kurumModel.KurumTicariUnvani,
                        KisiResimUrl = kisi.KisiResimUrl,
                        sifreVarMi = amirVarmi,
                        KurumID = kisi.KurumID,
                        KisiBagliOlduguKurumId = kisi.KisiBagliOlduguKurumId
                    };
                    kisiListModel.Add(kisiListItem);
                }
            }
            return kisiListModel.ToResult();
        }

        /// <summary>
        /// İlgili kurum Id ve aktif kişi Id değerine göre o kurumda tanımlı sanal kişi verilerini getiren metot.
        /// </summary>
        /// <param name="kurumId"></param>
        /// <param name="aktifKisiId"></param>
        /// <returns></returns>
        public Result<KisiTemelBilgiler> KurumaBagliSanalKisiGetir(int kurumId, int aktifKisiId)
        {
            var kisi = _repository.List(a => a.KurumID == kurumId && a.AktifMi == 1).FirstOrDefault();
            return kisi.ToResult();
        }

        /// <summary>
        /// Kişinin firebase token nını günceller
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Result<bool> UpdateFirebaseToken(string token)
        {
            var kisi = SingleOrDefault(_loginUser.KisiID).Value;
            kisi.FirebaseToken = token;
            Update(kisi);
            return true.ToResult();
        }

        /// <summary>
        /// Kişinin firebase token nını temizler
        /// </summary>
        /// <returns></returns>
        public Result<bool> DeleteFirebaseToken()
        {
            var kisi = SingleOrDefault(_loginUser.KisiID).Value;
            kisi.FirebaseToken = null;
            Update(kisi);
            return true.ToResult();
        }
    }
}