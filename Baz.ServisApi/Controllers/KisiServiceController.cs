using Baz.Attributes;
using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.Model.Pattern;
using Baz.ProcessResult;
using Baz.Service;
using Baz.Service.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Baz.KisiServisApi.Controllers
{
    /// <summary>
    /// KişiService ile ilgili web request işlemlerini yöneten API Controller.
    /// </summary>
    [Route("api/[Controller]")]
    public class KisiServiceController : Controller
    {
        private readonly IKisiService _kisiService;
        private readonly ILoginUser _loginUser;
        private readonly IKisiIliskiService _kisiIliskiService;
        private readonly IKisiHassasBilgilerService _kisiHassasBilgilerService;
        private readonly IParamOrganizasyonBirimleriService _paramOrganizasyonBirimleriService;
        private readonly IKisiBildigiDillerService _kisiBildigiDillerService;

        /// <summary>
        /// KişiService ile ilgili web request işlemlerini yöneten API Controller sınıfının yapıcı methodu
        /// </summary>
        /// <param name="kisiService"></param>
        /// <param name="kisiHassasBilgilerService"></param>
        /// <param name="kisiIliskiService"></param>
        /// <param name="paramOrganizasyonBirimleriService"></param>
        /// <param name="loginUser"></param>
        public KisiServiceController(IKisiService kisiService,IKisiBildigiDillerService kisiBildigiDillerService, IKisiHassasBilgilerService kisiHassasBilgilerService, IKisiIliskiService kisiIliskiService, IParamOrganizasyonBirimleriService paramOrganizasyonBirimleriService, ILoginUser loginUser)
        {
            _loginUser = loginUser;
            _kisiService = kisiService;
            _kisiHassasBilgilerService = kisiHassasBilgilerService;
            _kisiIliskiService = kisiIliskiService;
            _paramOrganizasyonBirimleriService = paramOrganizasyonBirimleriService;
            _kisiBildigiDillerService = kisiBildigiDillerService;
        }

        #region Profil Sayfası Kişi Profil ve Güncelleme

        /// <summary>
        /// KişiID değeriyle bir kişiye ait veriler getiren HTTPPost methodu.
        /// </summary>
        /// <param name="id">Kişiye ait Id değeri</param>
        /// <returns>İlgili kişinin verilerini döndürür.</returns>
        [ProcessName(Name = "Kişi Id ile temel bilgilerinin getirilmesi")]
        [Route("TemelBilgiGetir/{id}")]
        [HttpGet]
        public Result<KisiTemelBilgiler> TemelBilgilerGetir(int id)
        {
            var result = _kisiService.SingleOrDefault(id);
            return result;
        }

        /// <summary>
        /// Kişi verilerini güncelleyen HTTPPOST methodu.
        /// </summary>
        /// <param name="model"> güncellenecek kişi bilgileri parametresi</param>
        /// <returns>Güncellenen kişi verilerini döndürür.</returns>
        [ProcessName(Name = "Kişi verilerinin güncellenmesi")]
        [Route("TemelBilgiGuncelle")]
        [HttpPost]
        public Result<KisiTemelBilgiler> TemelBilgilerGuncelle([FromBody] KisiTemelBilgiler model)
        {
            var rs = _kisiService.SingleOrDefault(model.TabloID).Value;
            rs.KisiAdi = model.KisiAdi;
            rs.KisiSoyadi = model.KisiSoyadi;
            rs.KisiKullaniciAdi = model.KisiEposta;
            rs.KisiEkranAdi = model.KisiEposta;
            rs.KisiEposta = model.KisiEposta;
            rs.GuncellenmeTarihi = model.GuncellenmeTarihi;
            rs.KisiResimId = model.KisiResimId;
            rs.KisiTelefon1 = model.KisiTelefon1;

            var result = _kisiService.Update(rs);
            return result;
        }

        #endregion Profil Sayfası Kişi Profil ve Güncelleme

        #region Sifreİslemleri

        /// <summary>
        ///  kişi mail adresi değeriyle bir şifre yenileme talebi oluşturan HTTPPost methodu.
        /// </summary>
        /// <param name="mail"> ilgili kişinin Id değeri</param>
        /// <returns>sonucu döndürür.</returns>
        [ProcessName(Name = "Mail adresi ile bir şifre yenileme talebi oluşturulması")]
        [Route("SifreYenileMailile/{mail}")]
        [HttpGet]
        [AllowAnonymous]
        public Result<SistemLoginSifreYenilemeAktivasyonHareketleri> SifreYenilemeIstegi(string mail)
        {
            var isMail = Regex.IsMatch(mail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if (!isMail)
                return Results.Fail("Geçersiz mail adresi.");

            var result = _kisiService.SifreYenilemeIstegi(mail);
            return result;
        }

        /// <summary>
        /// İlgili şifre yenileme maili geçerli mi kontrolü sağlayan HTTPPost methodu.
        /// </summary>
        /// <param name="guid">şifre yenileme için gereke GUID değerini içeren string parametre.</param>
        /// <returns>Geçerliyse true, değilse false döndürür.</returns>
        [ProcessName(Name = "İlgili GUID ile oluşturulan şifre yenileme talebi halen geçerli mi kontrol edilmesi")]
        [Route("SifreYenilemeGecerliMi")]
        [HttpPost]
        [AllowAnonymous]
        public Result<bool> SifreYenilemeGecerliMi([FromBody] string guid)
        {
            var result = _kisiService.SifreYenilemeGecerliMi(guid);
            return result;
        }


        /// <summary>
        /// Kişi şifre değişim işlemi yapan HTTPPost methodu.
        /// </summary>
        /// <param name="model">Şifre model türünde model parametresi</param>
        /// <returns>şifre değişim işlemi sonrası, yeni verileri döndürür.</returns>
        [ProcessName(Name = "b@mail.com için şifre belirler")]
        [Route("SifreVer")]
        [HttpPost]
        public Result<string> SifreVer(string sifre)
        {

            return Results.Ok(sifre);
            //var result = _kisiService.SifreVer(sifre);
            //return result;
        }

        /// <summary>
        /// Kişi şifre değişim işlemi yapan HTTPPost methodu.
        /// </summary>
        /// <param name="model">Şifre model türünde model parametresi</param>
        /// <returns>şifre değişim işlemi sonrası, yeni verileri döndürür.</returns>
        [ProcessName(Name = "Profilde eski ve yeni şifre kullanılarak şifrenin güncellenmesi")]
        [Route("SifreDegistir")]
        [HttpPost]
        public Result<SifreModel> SifreDegistir([FromBody] SifreModel model)
        {
            var result = _kisiService.SifreDegistir(model);
            return result;
        }

        /// <summary>
        /// Kişi şifre yenileme işlemi yapan HTTPPost methodu.
        /// </summary>
        /// <param name="model">Şifre model türünde model parametresi</param>
        /// <returns>şifre yenileme işlemi sonrası, yeni verileri döndürür.</returns>
        [ProcessName(Name = "Şifre yenileme talebi sonrası şifrenin güncellenmesi")]
        [Route("SifreYenile")]
        [HttpPost]
        [AllowAnonymous]
        public Result<SifreModel> SifreYenile([FromBody] SifreModel model)
        {
            var result = _kisiService.SifreYenile(model);
            return result;
        }

        #endregion Sifreİslemleri

        #region KisiTemel

        /// <summary>
        /// Kişi bilgilerinin kaydedildiği method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProcessName(Name = "Temel kişi kaydı işlemi")]
        [Route("KisiTemelKaydet")]
        [HttpPost]
        public Result<KisiTemelKayitModel> KisiTemelKaydet([FromBody] KisiTemelKayitModel model)
        {
            var result = _kisiService.KisiTemelKaydet(model);
            return result;
        }

        ///// <summary>
        ///// Kişi bilgilerinin getirildiği method
        ///// </summary>
        ///// <param name="kisiID"></param>
        ///// <returns></returns>
        //[ProcessName(Name = "Kişi temel kayıt verilerini getirme işlemi")]
        //[Route("KisiTemelKayitVerileriGetir")]
        //[HttpPost]
        //public Result<KisiTemelKayitModel> KisiTemelKayitVerileriGetir([FromBody] int kisiID)
        //{
        //    var result = _kisiService.KisiTemelKayitVerileriGetir(kisiID);
        //    return result;
        //}

        /// <summary>
        /// Kişi bilgilerinin güncellendiği method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kişi temel kayıt güncelleme işlemi")]
        [Route("KisiTemelKayitVerileriGuncelle")]
        [HttpPost]
        public Result<KisiTemelKayitModel> KisiTemelKayitVerileriGuncelle([FromBody] KisiTemelKayitModel model)
        {
            var result = _kisiService.KisiTemelKayitVerileriGuncelle(model);
            return result;
        }

        /// <summary>
        /// Kişi listesi getiren method
        /// </summary>
        /// <returns></returns>
        [ProcessName(Name = "Kişi liste verilerini getirme işlemi")]
        [Route("TemelKisiListesiGetir/{id}")]
        [HttpGet]
        public Result<List<KisiListeModel>> TemelKisiListesiGetir(int id)
        {
            var result = _kisiService.KisiListesiGetir(id);
            return result;
        }

        /// <summary>
        /// Kurum Id'ye göre pasif kişileri getirme metodu
        /// </summary>
        /// <returns></returns>
        [ProcessName(Name = "Pasif Kişi Listesini getiren metod")]
        [Route("PasifKisiListesiGetir")]
        [HttpGet]
        public Result<List<KisiTemelBilgiler>> PasifKisiListesiGetir()
        {
            var result = _kisiService.PasifKisiListesiGetir();
            return result;
        }

        /// <summary>
        /// Kişi bilgilerini silindi yapan method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kişi bilgilerini silindi yapma işlemi")]
        [Route("TemelKisiSilindiYap/{kisiID}")]
        [HttpGet]
        public Result<bool> TemelKisiSilindiYap(int kisiID)
        {
            var result = _kisiService.TemelKisiSilindiYap(kisiID);
            return result;
        }

        /// <summary>
        /// Kişi bilgilerinin getirildiği method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kişi temel kayıt verilerini getirme işlemi")]
        [Route("KisiTemelKayitVerileriGetir")]
        [HttpPost]
        public Result<KisiTemelKayitModel> KisiTemelKayitVerileriGetir([FromBody] int kisiID)
        {
            var result = _kisiService.KisiTemelKayitVerileriGetir(kisiID);
            return result;
        }

        /// <summary>
        /// Kişi bilgilerini aktif yapan method
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kişi bilgilerini aktif yapma işlemi")]
        [Route("TemelKisiAktifYap/{kisiID}")]
        [HttpGet]
        public Result<bool> TemelKisiAktifYap(int kisiID)
        {
            var result = _kisiService.TemelKisiAktifYap(kisiID);
            return result;
        }

        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin astlarını getiren method
        /// </summary>
        /// <param name="kisiID"> astları getirilecek kişiId</param>
        /// <returns>kişinin astları listesi.</returns>
        [ProcessName(Name = "Kişi astlarının getirilmesi işlemi")]
        [Route("KisiAstlarListGetir/{kisiID}")]
        [HttpGet]
        public Result<List<KisiOrganizasyonBirimView>> KisiAstlarListGetir(int kisiID)
        {
            var result = _kisiService.KisiAstlarListGetir(kisiID);
            return result;
        }

        /// <summary>
        /// Mail adresine göre kişi verilerini getiren HTTPPost methodu.
        /// </summary>
        /// <param name="mail"> kişi mail adresi.</param>
        /// <returns>BasicKisiModel türünde ilgili kişi verilerini içeren modeli döndürür</returns>
        [ProcessName(Name = "Mail adresi ile kişi bilgilerinin getirilmesi")]
        [Route("MailileGetir/{mail}")]
        [HttpGet]
        [AllowAnonymous]
        public Result<BasicKisiModel> SingleOrDefault(string mail)
        {
            var result = _kisiService.SingleOrDefault(mail);
            return result;
        }

        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin amirlerini getiren method
        /// </summary>
        /// <param name="kisiID"> amirleri getirilecek kişiId</param>
        /// <returns>kişinin amirlerinin listesi</returns>
        [ProcessName(Name = "Kişi amirlerinin getirilmesi işlemi")]
        [Route("KisiAmirlerListGetir/{kisiID}")]
        [HttpGet]
        public Result<List<KisiOrganizasyonBirimView>> KisiAmirlerListGetir(int kisiID)
        {
            var result = _kisiService.KisiAmirlerListGetir(kisiID);
            return result;
        }

        /// <summary>
        /// Kurum organizasyon birimlerine göre kişinin amirlerini getiren method
        /// </summary>
        /// <param name="kisiID"> amirleri getirilecek kişiId</param>
        /// <returns>kişinin amirlerinin listesi</returns>
        [ProcessName(Name = "Kişi Organizasyon bilgilerinin getirilmesi işlemi")]
        [Route("KisiOrganizasyonListGetir/{kisiID}")]
        [HttpGet]
        public Result<List<KurumOrganizasyonBirimTanimlari>> KisiOrganizasyonListGetir(int kisiID)
        {
            
            

            var result = _kisiService.KisiOrganizasyonListGetir(kisiID);
            return result;
        }


        /// <summary>
        /// KisiId değeri ile ilgili kişinin sifre son yenileme tarihi değerini getiren method
        /// </summary>
        /// <param name="kisiID"> ilgili kişiId</param>
        /// <returns>şifre son yenileme tarihi</returns>
        [ProcessName(Name = "Kişi amirlerinin getirilmesi işlemi")]
        [Route("KisiSifreSonYenilemeTarihiGetir/{kisiID}")]
        [HttpGet]
        [AllowAnonymous]
        public Result<DateTime> KisiSifreSonYenilemeTarihiGetir(int kisiID)
        {
            var result = _kisiService.KisiSifreSonYenilemeTarihiGetir(kisiID);
            return result;
        }

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini pasif eden HTTPPost methodu.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Pasifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>
        [ProcessName(Name = "Kişi hesabını pasif etme")]
        [Route("HesabiPasifEt/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public Result<KisiTemelBilgiler> KisiHesabiPasifEtme(int id)
        {
            var result = _kisiService.KisiHesabiPasifEtme(id);
            return result;
        }

        /// <summary>
        /// Kisi temel bilgiler tablo Id değeri ile kişi hassas bilgilerinin getirildiği HTTPGet metodu.
        /// </summary>
        /// <param name="kisiId">kişinin temelBİlgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>KişiHassasBİlgiler verilerini döner.</returns>
        [ProcessName(Name = "Kişi hassas bilgiler getirme")]
        [Route("KisiHassasBilgilerGetir/{kisiId}")]
        [HttpGet]
        public Result<KisiHassasBilgiler> KisiHassasBilgilerGetir(int kisiId)
        {
            var result = _kisiHassasBilgilerService.KisiIdileGetir(kisiId);
            return result;
        }

        /// <summary>
        /// Kuruma ait kişileri kurumId'ye göre listeleyen method
        /// </summary>
        /// <param name="kurumId"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kuruma ait kişileri kurumId'ye göre listeleyen method")]
        [Route("KurumaBagliKisilerList/{kurumId}")]
        [HttpGet]
        public Result<List<KisiTemelBilgiler>> KurumaBagliKisilerList(int kurumId)
        {
            return _kisiService.KurumaBagliKisilerList(kurumId);
        }

        /// <summary>
        /// Idlere göre kişiyi döndürür.
        /// </summary>
        /// <param name="model"> Idlere göre kişileri döndürür</param>
        /// <returns>.</returns>
        [ProcessName(Name = "Idlere göre kişiyi döndürür.")]
        [Route("IdlereGoreKisileriGetir")]
        [HttpPost]
        public Result<List<KisiTemelBilgiler>> IdlereGoreKisileriGetir([FromBody] List<int> model)
        {
            var result = _kisiService.IdlereGoreKisileriGetir(model);
            return result;
        }

        /// <summary>
        /// Id'ye göre kişiyi döndürür.
        /// </summary>
        /// <param name="kisiID"></param>
        /// <returns>.</returns>
        [ProcessName(Name = "Idye göre kişiyi döndürür.")]
        [Route("IdyeGoreKisiGetir/{kisiID}")]
        [HttpGet]
        public Result<KisiTemelBilgiler> IdyeGoreKisiGetir(int kisiID)
        {
            var result = _kisiService.IdyeGoreKisiGetir(kisiID);
            return result;
        }

        /// <summary>
        /// Excel dosyasından import edilen kişi listesinin kaydedilmesi.
        /// </summary>
        /// <param name="list"> Kişi listesi</param>
        /// <returns>kaydedilen kişileri döndürür</returns>
        [ProcessName(Name = "Excel dosyasından import edilen kişi listesinin kaydedilmesi.")]
        [Route("ListeIleTemelKisikaydet")]
        [HttpPost]
        public Result<List<KisiTemelKayitModel>> ListeIleTemelKisikaydet([FromBody] List<KisiTemelKayitModel> list)
        {
            var result = _kisiService.ListeIleTemelKisikaydet(list);
            return result;
        }

        #endregion KisiTemel

        #region Kisiİliski

        /// <summary>
        /// Kisiler arası ilişkileri listeleyen metot
        /// </summary>
        /// <param name="kurumID"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kisiler arası ilişkiyi listeleme")]
        [Route("KisiIliskiList/{kurumID}")]
        [HttpGet]
        public Result<List<Iliskiler>> KisiIliskiList(int kurumID)
        {
            var result = _kisiIliskiService.KisiIliskiList(kurumID);
            return result;
        }

        /// <summary>
        /// Kurumdaki Müşteri Temsilcisi ilişkisini listeleyen metot
        /// </summary>
        /// <param name="buKurumID"></param>
        /// <returns></returns>
        [ProcessName(Name = "Müşteri Temsilcisi ilişkisini listeleme")]
        [Route("MusteriList/{buKurumID}")]
        [HttpGet]
        public Result<List<Iliskiler>> MusteriList(int buKurumID)
        {
            var result = _kisiIliskiService.MusteriList(buKurumID);
            return result;
        }

        /// <summary>
        /// Kişiler arası ilişkiyi kaydetme metotu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kişiler arası ilişki kaydetme")]
        [Route("KisiIliskiKaydet")]
        [HttpPost]
        public Result<Iliskiler> KisiIliskiKaydet([FromBody] KisiIliskiKayitModel model)
        {
            var result1 = KisiIliskiList(model.KurumID).Value;

            foreach (var rs1 in result1)
            {
                if (rs1.BuKisiId == model.BuKisiID && rs1.BununKisiId == model.BununKisiID && rs1.AktifMi == 1 && rs1.SilindiMi == 0 && rs1.IliskiTuruId == model.IliskiTuruID)
                {
                    return Results.Fail("Aynı kayıttan ekleyemezsiniz!", ResultStatusCode.CreateError);
                }
                else if (rs1.BununKisiId == model.BuKisiID && rs1.BuKisiId == model.BununKisiID && rs1.AktifMi == 1 && rs1.SilindiMi == 0)
                {
                    return Results.Fail("Bu işlemi gerçekleştiremezsiniz!", ResultStatusCode.CreateError);
                }
            }
            var result = _kisiIliskiService.KisiIliskiKaydet(model);
            return result;
        }

        /// <summary>
        /// Kişiler arası ilişkiyi silindi yapan metot
        /// </summary>
        /// <param name="tabloID"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kişi ilişki bilgilerini silindi yapma işlemi")]
        [Route("KisiIliskiSil/{tabloID}")]
        [HttpGet]
        public Result<bool> KisiIliskiSil(int tabloID)
        {
            var result = _kisiIliskiService.KisiIliskiSil(tabloID);
            return result;
        }

        /// <summary>
        /// Kişiler arası ilişkiyi güncelleyen metot
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kişiler arası ilişki güncelleme")]
        [Route("KisiIliskiGuncelle")]
        [HttpPost]
        public Result<Iliskiler> KisiIliskiGuncelle([FromBody] KisiIliskiKayitModel model)
        {
            var result1 = KisiIliskiList(model.KurumID).Value;

            foreach (var rs1 in result1)
            {
                if (rs1.BuKisiId == model.BuKisiID && rs1.BununKisiId == model.BununKisiID && rs1.AktifMi == 1 && rs1.SilindiMi == 0 && rs1.IliskiTuruId == model.IliskiTuruID)
                {
                    return Results.Fail("Aynı kayıttan ekleyemezsiniz!", ResultStatusCode.UpdateError);
                }
                else if (rs1.BununKisiId == model.BuKisiID && rs1.BuKisiId == model.BununKisiID && rs1.AktifMi == 1 && rs1.SilindiMi == 0)
                {
                    return Results.Fail("Bu işlemi gerçekleştiremezsiniz!", ResultStatusCode.UpdateError);
                }
            }
            var result = _kisiIliskiService.KisiIliskiGuncelle(model);
            return result;
        }

        #endregion Kisiİliski

        /// <summary>
        /// kişiID değeri ile kişi silme işlemini gerçekleştiren HTTPPost Methodu.
        /// </summary>
        /// <param name = "id" > kişiye ait Id değeri</param>
        /// <returns>silinen kişiniin verilerini getirir.</returns>
        [ProcessName(Name = "Kisi verilerinin silinmesi")]
        [Route("TemelBilgilerSil/{id}")]
        [HttpGet]
        public Result<KisiTemelBilgiler> Sil(int id)
        {
            var result = _kisiService.Delete(id);
            return result;
        }

        /// <summary>
        /// Unit test için yazılış, kaydedilen son kişiyi getirme metodu
        /// </summary>
        /// <returns></returns>
        [ProcessName(Name = "SonKisi")]
        [Route("SonKisi")]
        [HttpGet]
        public Result<KisiTemelBilgiler> SonKisi()
        {
            var result = _kisiService.List().Value.LastOrDefault(x => x.AktifMi == 1);
            return result.ToResult();
        }

        /// <summary>
        /// Kullanıcı adına göre kişi verilerini getiren HTTPPost methodu.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>BasicKisiModel türünde ilgili kişi verilerini içeren modeli döndürür</returns>
        [ProcessName(Name = "Kullanıcı adı ile kişi bilgilerinin getirilmesi")]
        [Route("KullanıcıAdiIleGetir/{userName}")]
        [HttpGet]
        public Result<BasicKisiModel> SingleOrDefaultForUserName(string userName)
        {
            var result = _kisiService.SingleOrDefaultForUserName(userName);
            return result;
        }
        [ProcessName(Name = "")]
        [Route("KisiBildigiDilList/{kisiId}")]
        [HttpGet]
        public Result<List<KisiBildigiDiller>> KisiBildigiDilList(int kisiId)
        {
            var result = _kisiBildigiDillerService.KisiBildigiDilList(kisiId);
            return result;
        }

        ///// <summary>
        ///// Kişi verilerini BasicKisiModel türünde alıp güncelleyen HTTPPost methodu.
        ///// </summary>
        ///// <param name="model"> kişi verilerini içeren BasicKisiModel</param>
        ///// <returns></returns>
        //[ProcessName(Name = "Viewmodels altındaki BasicKisiModel türü kullanılarak kişi bilgilerinin güncellenmesi")]
        //[Route("BasicKisiModelileGuncelle")]
        //[HttpPost]
        //public Result<BasicKisiModel> Update([FromBody] BasicKisiModel model)
        //{
        //    var result = _kisiService.Update(model);
        //    return result;
        //}

        ///// <summary>
        ///// Kişi verilerini BasicKisiModel türünde alıp güncelleyen HTTPPut methodu Test için.
        ///// </summary>
        ///// <param name="model"> kişi verilerini içeren BasicKisiModel</param>
        ///// <returns></returns>
        //[ProcessName(Name = "Viewmodels altındaki BasicKisiModel türü kullanılarak kişi bilgilerinin güncellenmesi")]
        //[HttpPut]
        //public Result<BasicKisiModel> Put([FromBody] BasicKisiModel model)
        //{
        //    var result = _kisiService.Update(model);
        //    return result;
        //}

        /// <summary>
        /// Tüm kişlerin bilgilerini listeleyip getiren HttpGet Methodu.
        /// </summary>
        /// <returns>kişi bilgilerinin listesini döndürür.</returns>
        [ProcessName(Name = "Kisiler listesinin getirilmesi")]
        [Route("KisiListesiGetir")]
        [HttpGet]
        public Result<List<KisiTemelBilgiler>> KisiListeGetir()
        {
            var result = _kisiService.List();
            return result;
        }

        ///// <summary>
        ///// KişiID değeriyle bir şifre yenileme talebi oluşturan HTTPPost methodu.
        ///// </summary>
        ///// <param name="KisiID"> ilgili kişinin Id değeri</param>
        ///// <returns>sonucu döndürür.</returns>
        //[ProcessName(Name = "KisiId ile bir şifre yenileme talebi oluşturulması")]
        //[Route("SifreYenileIDile")]
        //[HttpPost]
        //public Result<SistemLoginSifreYenilemeAktivasyonHareketleri> SifreYenilemeIstegi([FromHeader] int KisiID)
        //{
        //    var result = _kisiService.SifreYenilemeIstegi(KisiID);
        //    return result;
        //}

        ///// <summary>
        ///// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve SilindiMi değerini aktif eden HTTPPost methodu.
        ///// </summary>
        ///// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        ///// <returns>Sİlme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>
        //[ProcessName(Name = "Kisi temel ve hassas bilgilerin silindi durumuna alınması")]
        //[Route("TemelveHassasBilgileriSil")]
        //[HttpPost]
        //public Result<KisiTemelBilgiler> KisiBilgileriSilme([FromHeader] int id)
        //{
        //    var result = _kisiService.KisiBilgileriSilme(id);
        //    return result;
        //}

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini aktif eden HTTPPost methodu.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Aktifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>
        [ProcessName(Name = "kişi aktive etme")]
        [Route("AktifEt/{id}")]
        [HttpPost]
        public Result<KisiTemelBilgiler> KisiAktiveEtme(int id)
        {
            var result = _kisiService.KisiAktiveEtme(id);
            return result;
        }

        /// <summary>
        /// Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini pasif eden HTTPPost methodu.
        /// </summary>
        /// <param name="id">Kişinin TemelBilgiler tablosunda kayıtlı Id değeri</param>
        /// <returns>Pasifleştirme işlemi sonucuna göre kişi verilerini içeren modeli döndürür.</returns>
        [ProcessName(Name = "Kişi pasif etme")]
        [Route("PasifEt/{id}")]
        [HttpGet]
        public Result<KisiTemelBilgiler> KisiPasifEtme(int id)
        {
            var result = _kisiService.KisiPasifEtme(id);
            return result;
        }

        /// <summary>
        /// Müşteri temsilcisine atanmış kurumların kişilerini listeleme methodu
        /// </summary>
        /// <param name="musteriTemsilcisiId"></param>
        /// <returns></returns>
        [ProcessName(Name = "Müşteri temsilcisine atanmış kurumların kişilerini listeleme")]
        [Route("MusteriTemsilcisiBagliKisilerList/{musteriTemsilcisiId}")]
        [HttpGet]
        public Result<List<KisiListeModel>> MusteriTemsilcisiBagliKisilerList(int musteriTemsilcisiId)
        {
            var result = _kisiService.MusteriTemsilcisiBagliKisilerList(musteriTemsilcisiId);
            return result;
        }

        /// <summary>
        /// Amirlere altlarındaki kişileri me o kişilere atanmış kişileri listeleme
        /// </summary>
        /// <param name="amirId"></param>
        /// <returns></returns>
        [ProcessName(Name = "Amirlere altlarındaki kişileri me o kişilere atanmış kişileri listeleme")]
        [Route("AmirlereAstMusteriTemsilcisiKisileriniGetir/{amirId}")]
        [HttpGet]
        public Result<List<KisiListeModel>> AmirlereAstMusteriTemsilcisiKisileriniGetir(int amirId)
        {
            var result = _kisiService.AmirlereAstMusteriTemsilcisiKisileriniGetir(amirId);
            return result;
        }

        /// <summary>
        /// Amirlerin altlarındaki müşteri ltemsilcilerinin ıdlerini getirilmesi
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        [ProcessName(Name = "Amirlerin altlarındaki müşteri ltemsilcilerinin ıdlerini getirilmesi")]
        [Route("AmireBagliMusteriTemsilcileriList/{kisiId}")]
        [HttpGet]
        public Result<List<int>> AmireBagliMusteriTemsilcileriList(int kisiId)
        {
            var result = _kisiService.AmireBagliMusteriTemsilcileriList(kisiId);
            return result;
        }

        /// <summary>
        /// Kişi Idye göre amir veya müşteri temsilcisine bağlı kurumların ıdlerini getirilmesi
        /// </summary>
        /// <param name="kisiId"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kişi Idye göre amir veya müşteri temsilcisine bağlı kurumların ıdlerini getirilmesi")]
        [Route("AmirveyaMusteriTemsilcisiKurumlariIDGetir/{kisiId}")]
        [HttpGet]
        public Result<List<int>> AmirveyaMusteriTemsilcisiKurumlariIDGetir(int kisiId)
        {
            var result = _kisiService.AmirveyaMusteriTemsilcisiKurumlariIDGetir(kisiId);
            return result;
        }

        /// <summary>
        /// Kisiye sifre atama islemi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ProcessName(Name = "Kisiye sifre atama islemi")]
        [Route("SifreAtama")]
        [HttpPost]
        public Result<bool> SifreAtama([FromBody] SifreAtamaModel model)
        {
            return _kisiHassasBilgilerService.SifreAtama(model);
        }

        /// <summary>
        /// Id Lere göre kişileri KisiListeModel türünde getirir.
        /// </summary>  
        /// <param name="model"> ilgili kisi Idleri</param>
        /// <returns>listelenen kişileri döndürür.</returns>
        [ProcessName(Name = "Id listesi ile kisi liste modeli döndürülmesi")]
        [Route("IdlereGoreKisiListeModelGetir")]
        [HttpPost]
        public Result<List<KisiListeModel>> IdlereGoreKisiListeModelGetir([FromBody] List<int> model)
        {
            var result = _kisiService.IdlereGoreKisiListeModelGetir(model);
            return result;
        }

        /// <summary>
        /// Pozisyona bağlı hiyerarşik ağaçta ast-üst ilişkisi bulunmayan ancak ilgili kişilere irişmesi gereken kullanıcılar için kullanılacak kişi listesi metodu.
        /// </summary>
        /// <returns>KisiListeModel listesi döndürür. <see cref="KisiListeModel"></see></returns>
        [Route("HiyerarsiDisiKisilerKisiListesi")]
        [HttpGet]
        public Result<List<KisiListeModel>> HiyerarsiDisiKisilerKisiListesi()
        {
            var result = _kisiService.HiyerarsiDisiKisilerKisiListesi();
            return result;
        }

        /// <summary>
        /// İlgili kurum Id ve aktif kişi Id değerine göre o kurumda tanımlı sanal kişi verilerini getiren metot.
        /// </summary>
        /// <param name="kurumId"></param>
        /// <param name="aktifKisiId"></param>
        /// <returns></returns>
        [Route("KurumaBagliSanalKisiGetir/{kurumId}/{aktifKisiId}")]
        [HttpGet]
        public Result<KisiTemelBilgiler> KurumaBagliSanalKisiGetir(int kurumId, int aktifKisiId)
        {
            var result = _kisiService.KurumaBagliSanalKisiGetir(kurumId, aktifKisiId);
            return result;
        }

        /// <summary>
        /// Parametre tanımına göre TipId döndüren method
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("GetTipId/{name}")]
        [HttpGet]
        public Result<int> GetTipId(string name)
        {
            var result = _paramOrganizasyonBirimleriService.GetTipId(name);
            return result;
        }

        /// <summary>
        /// Kişinin firebase token nını günceller
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("UpdateFirebaseToken/{token}")]
        [HttpGet]
        public Result<bool> UpdateFirebaseToken(string token)
        {
            var result = _kisiService.UpdateFirebaseToken(token);
            return result;
        }

        /// <summary>
        /// Kişinin firebase token nını temizler
        /// </summary>
        /// <returns></returns>
        [Route("DeleteFirebaseToken")]
        [HttpGet]
        public Result<bool> DeleteFirebaseToken()
        {
            var result = _kisiService.DeleteFirebaseToken();
            return result;
        }

        /// <summary>
        /// Kişinin firebase token nını temizler
        /// </summary>
        /// <returns></returns>
        [Route("AktifPasifKisiList")]
        [HttpGet]
        public Result<List<KisiTemelBilgiler>> AktifPasifKisiList()
        {
            var result = _kisiService.List(x => x.KurumID == _loginUser.KurumID && x.KisiBagliOlduguKurumId == _loginUser.KurumID);
            return result;
        }
    }
}