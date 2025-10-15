using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Baz.Model.Pattern;

namespace Baz.Service
{
    /// <summary>
    /// Kişi Grup Ekip İşlemlerinin yapıldığı interface
    /// </summary>
    public interface IKisiGrupService : IService<KurumlarKisiler>
    {
        /// <summary>
        /// Kişi Grup Kaydeden Method.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Result<KisiGrupKayitViewModel> KisiGrupKaydet(KisiGrupKayitViewModel list);

        /// <summary>
        /// KisiGrup güncelleme
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Result<KisiGrupKayitViewModel> KisiGrupGuncelle(KisiGrupKayitViewModel model);

        /// <summary>
        /// Kişi Grup Listesi getiren method.
        /// </summary>
        /// <returns></returns>
        Result<List<KisiGrupListViewModel>> KisiGrupListesiGetir(int kurumId);

        /// <summary>
        /// Kişi Grup tanimini silen method.
        /// </summary>
        /// <returns></returns>
        Result<bool> KisiGrupTanimSil(int id);

        /// <summary>
        /// Id'ye göre Kişi Grup tanimini getiren method.
        /// </summary>
        /// <returns></returns>
        Result<KisiGrupListViewModel> KisiGrupGetir(int id);

        /// <summary>
        /// Ekip Idye göre ekip içindeki kişileri getiren metod
        /// </summary>
        /// <returns></returns>
        Result<List<KisiListeModel>> EkipIdyeGoreEkipKisileriniGetir(int ekipId);
    }

    /// <summary>
    /// Kişi Grup Ekip İşlemlerinin yapıldığı service
    /// </summary>
    public class KisiGrupService : Service<KurumlarKisiler>, IKisiGrupService
    {
        private readonly IKurumOrganizasyonBirimTanimlariService _kurumOrganizasyonBirimTanimlariService;
        private readonly IKisiService _kisiService;
        private readonly ILoginUser _loginUser;

        /// <summary>
        /// Kişi Grup Ekip İşlemlerinin yapıldığı service yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="kurumOrganizasyonBirimTanimlariService"></param>
        /// <param name="kisiService"></param>
        public KisiGrupService(IRepository<KurumlarKisiler> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<KurumlarKisilerService> logger, IKurumOrganizasyonBirimTanimlariService kurumOrganizasyonBirimTanimlariService, IKisiService kisiService, ILoginUser loginUser) : base(repository, dataMapper, serviceProvider, logger)
        {
            _kurumOrganizasyonBirimTanimlariService = kurumOrganizasyonBirimTanimlariService;
            _kisiService = kisiService;
            _loginUser = loginUser;
        }

        /// <summary>
        /// Kişi Grup Kaydeden Method.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Result<KisiGrupKayitViewModel> KisiGrupKaydet(KisiGrupKayitViewModel model)
        {
            _repository.DataContextConfiguration().BeginNewTransactionIsNotFound();
            //Ekibin organizasyon olarak kaydı
            KurumOrganizasyonBirimTanimlari orgTanim = new()
            {
                KisiID = _loginUser.KisiID,
                BirimTanim = model.EkipIsmi,
                IlgiliKurumId = _loginUser.KurumID,
                KayitTarihi = DateTime.Now,
                KurumID = _loginUser.KurumID,
                OrganizasyonBirimTipiId = (int)OrganizasyonBirimTipi.Ekip, //8
                GuncellenmeTarihi = DateTime.Now,
                AktifMi = 1,
                SilindiMi = 0
            };
            var orgKayit = _kurumOrganizasyonBirimTanimlariService.Add(orgTanim);
            //Ekipteki kişilerin kaydı
            List<KurumlarKisiler> list = new();
            foreach (var kisi in model.KisiIdList)
            {
                var ekipTanim = new KurumlarKisiler()
                {
                    KurumOrganizasyonBirimTanimId = orgKayit.Value.TabloID,
                    IlgiliKisiId = kisi,
                    KurumID = _loginUser.KurumID,
                    IlgiliKurumId = _loginUser.KurumID,
                    KisiID = _loginUser.KisiID,
                    KayitEdenID = _loginUser.KisiID,
                    AktifMi = 1,
                    SilindiMi = 0,
                    GuncellenmeTarihi = DateTime.Now,
                    KayitTarihi = DateTime.Now,
                    AtanmaninSonlanmaZamani = DateTime.Now,
                    AtanmaZamani = DateTime.Now
                };
                list.Add(ekipTanim);
            }
            foreach (var item in list)
            {
                this.Add(item);
            }

            model.OrganizasyonBirimiID = orgKayit.Value.TabloID;
            _repository.DataContextConfiguration().Commit();
            return model.ToResult();
        }

        /// <summary>
        /// Kişi Grup Listesi getiren method.
        /// </summary>
        /// <returns></returns>
        public Result<List<KisiGrupListViewModel>> KisiGrupListesiGetir(int kurumId)
        {
            // Ekipleri belirle

            var kisiGrupViewList = new List<KisiGrupListViewModel>();
            var ekipList = _kurumOrganizasyonBirimTanimlariService.List(x => x.OrganizasyonBirimTipiId == (int)OrganizasyonBirimTipi.Ekip /*8*/ && x.AktifMi == 1 && x.SilindiMi == 0 && x.KurumID == kurumId).Value; //ekipler
            var kisiGrupList = this.List(x => x.AktifMi == 1 && x.KisiID != 0 && x.IlgiliKisiId != 0).Value;

            foreach (var ekip in ekipList)
            {
                var grupKisiler = kisiGrupList.Where(x => x.KurumOrganizasyonBirimTanimId == ekip.TabloID).ToList();

                var kisiGrupView = new KisiGrupListViewModel()
                {
                    KisiIdList = grupKisiler.Select(x => x.IlgiliKisiId).ToList(),
                    EkipAciklama = ekip.BirimTanim,
                    KisiSayisi = grupKisiler.Count,
                    OrganizasyonBirimiID = ekip.TabloID
                };
                kisiGrupViewList.Add(kisiGrupView);
            }
            return kisiGrupViewList.ToResult();
        }

        /// <summary>
        /// Kişi Grup tanimini silen method.
        /// </summary>
        /// <returns></returns>
        public Result<bool> KisiGrupTanimSil(int id)
        {
            _repository.DataContextConfiguration().BeginNewTransactionIsNotFound();
            var tanim = _kurumOrganizasyonBirimTanimlariService.SingleOrDefault(id).Value;
            tanim.AktifMi = 0;
            tanim.SilindiMi = 1;
            tanim.SilinmeTarihi = DateTime.Now;

            _kurumOrganizasyonBirimTanimlariService.Update(tanim);
            var ekip = List(X => X.KurumOrganizasyonBirimTanimId == id && X.AktifMi == 1).Value;
            foreach (var kurumKisi in ekip)
            {
                kurumKisi.AktifMi = 0;
                kurumKisi.SilindiMi = 1;
                kurumKisi.SilinmeTarihi = DateTime.Now; 
                this.Update(kurumKisi);
            }

            _repository.DataContextConfiguration().Commit();
            return true.ToResult();
        }

        /// <summary>
        /// Id'ye göre Kişi Grup tanimini getiren method.
        /// </summary>
        /// <returns></returns>
        public Result<KisiGrupListViewModel> KisiGrupGetir(int id)
        {
            // Ekipleri belirle

            var ekip = _kurumOrganizasyonBirimTanimlariService.SingleOrDefault(id).Value; //ekipler
            if (ekip == null)
            {
                return Results.Fail("Kayıt bulunamadı.", ResultStatusCode.ReadError);
            }
            var grupKisiler = this.List(x => x.AktifMi == 1 && x.KisiID != 0 && x.IlgiliKisiId != 0 && x.KurumOrganizasyonBirimTanimId == ekip.TabloID).Value;

            var kisiGrupView = new KisiGrupListViewModel()
            {
                KisiIdList = grupKisiler.Select(x => x.IlgiliKisiId).ToList(),
                EkipAciklama = ekip.BirimTanim,
                KisiSayisi = grupKisiler.Count,
                OrganizasyonBirimiID = ekip.TabloID
            };
            return kisiGrupView.ToResult();
        }

        /// <summary>
        /// KisiGrup güncelleme
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Result<KisiGrupKayitViewModel> KisiGrupGuncelle(KisiGrupKayitViewModel model)
        {
            _repository.DataContextConfiguration().BeginNewTransactionIsNotFound();
            List<KurumlarKisiler> list = new();
            //Ekip organizasyonunun güncellenmesi
            var orgGuncel = new KurumOrganizasyonBirimView()
            {
                TabloId = model.OrganizasyonBirimiID.Value,
                Tanim = model.EkipIsmi,
            };
            _kurumOrganizasyonBirimTanimlariService.UpdateName(orgGuncel);
            //Ekipteki kşişilerin kaydı
            foreach (var kisi in model.KisiIdList)
            {
                var ekipTanim = new KurumlarKisiler()

                {
                    KurumOrganizasyonBirimTanimId = model.OrganizasyonBirimiID.Value,
                    IlgiliKisiId = kisi,
                    KurumID = _loginUser.KurumID,
                    IlgiliKurumId = _loginUser.KurumID,
                    KisiID = _loginUser.KisiID,
                    KayitEdenID = _loginUser.KisiID,
                    AktifMi = 1,
                    SilindiMi = 0,
                    GuncellenmeTarihi = DateTime.Now,
                    KayitTarihi = DateTime.Now,
                    AtanmaninSonlanmaZamani = DateTime.Now,
                    AtanmaZamani = DateTime.Now
                };

                list.Add(ekipTanim);
            }

            var kurumlarKisilerModel = this.List().Value.Where(x => x.KurumOrganizasyonBirimTanimId == model.OrganizasyonBirimiID && x.SilindiMi == 0 && x.AktifMi == 1).ToList();
            //Eski kişilerin pasife alınması
            foreach (var kurumKisi in kurumlarKisilerModel)
            {
                kurumKisi.AktifMi = 0;
                kurumKisi.SilindiMi = 1;
                this.Update(kurumKisi);
            }

            foreach (var item in list)
            {
                this.Add(item);
            }

            _repository.DataContextConfiguration().Commit();
            return model.ToResult();
        }

        /// <summary>
        /// Ekip Idye göre ekip içindeki kişileri getiren metod
        /// </summary>
        /// <returns></returns>
        public Result<List<KisiListeModel>> EkipIdyeGoreEkipKisileriniGetir(int ekipId)
        {
            var ekip = _kurumOrganizasyonBirimTanimlariService.SingleOrDefault(ekipId).Value; //ekipler
            if (ekip == null)
            {
                return Results.Fail("Kayıt bulunamadı.", ResultStatusCode.ReadError);
            }
            var grupKisiler = this.List(x => x.AktifMi == 1 && x.KisiID != 0 && x.IlgiliKisiId != 0 && x.KurumOrganizasyonBirimTanimId == ekip.TabloID).Value;
            List<KisiListeModel> kisiList = new();
            foreach (var item in grupKisiler)
            {
                var kisi = _kisiService.SingleOrDefaultListeModel(item.IlgiliKisiId).Value;
                if (kisi != null)
                {
                    kisiList.Add(kisi);
                }
            }
            return kisiList.ToResult();
        }
    }
}