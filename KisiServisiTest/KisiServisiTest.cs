using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.RequestManager.Abstracts;
using KisiServisiTest.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;

namespace KisiServisiTest
{
    /// <summary>
    /// Kisi Servisinin testlerinin yapıldığı sınıftır.
    /// Listeleme, Silme metodlarında olumsuz test senaryosuna gerek duyulmamıştır.
    /// </summary>
    [TestClass()]
    public class KisiServisiTest
    {
        private readonly IRequestHelper _helper;

        /// <summary>
        /// Kisi Servisinin testlerinin yapıldığı sınıftır.
        /// </summary>
        public KisiServisiTest()
        {
            _helper = TestServerRequestHelper.CreateHelper();
        }

        #region KisiTemel(Profil Sayfası)

        /// <summary>
        /// KisiTemel(Profil Sayfası) testleri
        /// </summary>
        [TestMethod()]
        public void KisiTemelProfilSayfasi()
        {
            //Assert-1 Add
            var kisitemelkaydet = _helper.Post<Result<KisiTemelKayitModel>>($"/api/KisiService/KisiTemelKaydet",
               new KisiTemelKayitModel
               {
                   Adi = "Unit Yeni12",
                   Soyadi = "Test",
                   AnneAdi = "Hacer",
                   BabaAdi = "Mustafa",
                   Cinsiyeti = 1,
                   Departman = 4,
                   Dini = 1,
                   DogduguSehir = 1,
                   DogduguUlke = 1,
                   DogumTarihi = DateTime.Now,
                   Kurum = 82,
                   Rol = 825,
                   Pozisyon = 1969,
                   TCKimlikNo = "234242",
                   EpostaAdresi = "caner12@mail.com",
                   MedeniHali = 1,
                   IseGirisTarihi = DateTime.Now,
                   Lokasyon = 1,
                   SicilNo = "233432",
                   //TemelBilgiTabloID = 97,
                   DilListesi = new List<DilModel>()
                   {
                        new DilModel()
                        {
                            DilSeviye = "",
                            YabanciDilTipi = 1
                        }
                   },
                   AdresListesi = new List<AdresModel>()
                   {
                        new AdresModel()
                        {
                            Adres = "sdffd",
                            AdresTipi = 1,
                            Sehir = 1,
                            Ulke = 1
                        }
                   },
                   OkulListesi = new List<OkulModel>()
                   {
                        new OkulModel()
                        {
                            Fakulte = "MUhendislik",
                            MezuniyetTarihi = DateTime.Now,
                            OkulAdi = "Istanbul",
                            OkulTipi = 1
                        }
                   },
                   TelefonListesi = new List<TelefonModel>()
                   {
                        new TelefonModel()
                        {
                            TelefonNo = "",
                            TelefonTipi = 1
                        }
                   }
               });
            Assert.AreEqual(kisitemelkaydet.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisitemelkaydet.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisitemelkaydet.Result);

            //Asssert-3 update

            var update = _helper.Post<Result<KisiTemelKayitModel>>($"/api/KisiService/KisiTemelKayitVerileriGuncelle", new KisiTemelKayitModel
            {
                TemelBilgiTabloID = kisitemelkaydet.Result.Value.TemelBilgiTabloID,
                Adi = "Unit Yeni12",
                Soyadi = "Test",
                AnneAdi = "Hacer1",
                BabaAdi = "Mustafa",
                Cinsiyeti = 1,
                Departman = 4,
                Dini = 1,
                DogduguSehir = 1,
                DogduguUlke = 1,
                DogumTarihi = DateTime.Now,
                Kurum = 82,
                Rol = 825,
                Pozisyon = 1969,
                TCKimlikNo = "234242",
                EpostaAdresi = "caner12@mail.com",
                MedeniHali = 1,
                IseGirisTarihi = DateTime.Now,
                Lokasyon = 1,
                SicilNo = "233432",
                //TemelBilgiTabloID = 97,
                DilListesi = new List<DilModel>()
                   {
                        new DilModel()
                        {
                            DilSeviye = "",
                            YabanciDilTipi = 1
                        }
                   },
                AdresListesi = new List<AdresModel>()
                   {
                        new AdresModel()
                        {
                            Adres = "sdffd",
                            AdresTipi = 1,
                            Sehir = 1,
                            Ulke = 1
                        }
                   },
                OkulListesi = new List<OkulModel>()
                   {
                        new OkulModel()
                        {
                            Fakulte = "MUhendislik",
                            MezuniyetTarihi = DateTime.Now,
                            OkulAdi = "Istanbul",
                            OkulTipi = 1
                        }
                   },
                TelefonListesi = new List<TelefonModel>()
                   {
                        new TelefonModel()
                        {
                            TelefonNo = "",
                            TelefonTipi = 1
                        }
                   }
            });
            Assert.AreEqual(update.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(update.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(update.Result);

            //Asssert-4 negativeUpdate

            var negativeUpdate = _helper.Post<Result<KisiTemelKayitModel>>($"/api/KisiService/KisiTemelKayitVerileriGuncelle", new KisiTemelKayitModel
            {
            });
            Assert.IsFalse(negativeUpdate.Result.IsSuccess);

            //Asssert-6 getbyıd

            var getbyıd = _helper.Get<Result<KisiTemelBilgiler>>($"/api/KisiService/TemelBilgiGetir/" + kisitemelkaydet.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(getbyıd.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(getbyıd.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(getbyıd.Result);

            //Asssert-6 getbyıdNegative

            //var getbyıdNegative = _helper.Get<Result<KisiTemelBilgiler>>($"/api/KisiService/TemelBilgiGetir/" + 0);
            //Assert.IsNull(getbyıdNegative.Result.Value);

            //Assert- KullanıcıAdiIleGetir
            var KullanıcıAdiIleGetir = _helper.Get<Result<BasicKisiModel>>($"/api/KisiService/KullanıcıAdiIleGetir/" + update.Result.Value.EpostaAdresi);
            Assert.AreEqual(KullanıcıAdiIleGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(KullanıcıAdiIleGetir.Result.Value);

            //Asssert-7 delete

            var delete = _helper.Get<Result<KisiTemelBilgiler>>($"/api/KisiService/TemelBilgilerSil/" + kisitemelkaydet.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(delete.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(delete.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(delete.Result);
        }

        #endregion KisiTemel(Profil Sayfası)

        #region KisiTemelVeriCrud(Kişi İşlemlerinin testidir. Kişi Temel ve Hassas Bilgilerinin işlendiğği methodların testi yapılmıştır.)

        /// <summary>
        /// KisiTemelVeriCrud(Kişi İşlemlerinin testidir. Kişi Temel ve Hassas Bilgilerinin işlendiğği methodların testi yapılmıştır.)
        /// </summary>
        [TestMethod()]
        public void CrudTestsKisiTemel()
        {
            //Assert-1 Add
            var kisitemelkaydet = _helper.Post<Result<KisiTemelKayitModel>>($"/api/KisiService/KisiTemelKaydet",
               new KisiTemelKayitModel
               {
                   Adi = "Unit Yeni",
                   Soyadi = "Test",
                   AnneAdi = "Hacer",
                   BabaAdi = "Mustafa",
                   Cinsiyeti = 1,
                   Departman = 4,
                   Dini = 1,
                   DogduguSehir = 1,
                   DogduguUlke = 1,
                   DogumTarihi = DateTime.Now,
                   Kurum = 82,
                   Rol = 825,
                   Pozisyon = 1969,
                   TCKimlikNo = "234242",
                   EpostaAdresi = "caner1@mail.com",
                   MedeniHali = 1,
                   IseGirisTarihi = DateTime.Now,
                   Lokasyon = 1,
                   SicilNo = "233432",
                   //TemelBilgiTabloID = 97,
                   DilListesi = new List<DilModel>()
                   {
                        new DilModel()
                        {
                            DilSeviye = "",
                            YabanciDilTipi = 1
                        }
                   },
                   AdresListesi = new List<AdresModel>()
                   {
                        new AdresModel()
                        {
                            Adres = "sdffd",
                            AdresTipi = 1,
                            Sehir = 1,
                            Ulke = 1
                        }
                   },
                   OkulListesi = new List<OkulModel>()
                   {
                        new OkulModel()
                        {
                            Fakulte = "MUhendislik",
                            MezuniyetTarihi = DateTime.Now,
                            OkulAdi = "Istanbul",
                            OkulTipi = 1
                        }
                   },
                   TelefonListesi = new List<TelefonModel>()
                   {
                        new TelefonModel()
                        {
                            TelefonNo = "",
                            TelefonTipi = 1
                        }
                   }
               });
            Assert.AreEqual(kisitemelkaydet.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisitemelkaydet.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisitemelkaydet.Result);

            var üst = _helper.Post<Result<KisiTemelKayitModel>>($"/api/KisiService/KisiTemelKaydet",
               new KisiTemelKayitModel
               {
                   Adi = "Unit Yeni",
                   Soyadi = "Test",
                   AnneAdi = "Hacer",
                   BabaAdi = "Mustafa",
                   Cinsiyeti = 1,
                   Departman = 4,
                   Dini = 1,
                   DogduguSehir = 1,
                   DogduguUlke = 1,
                   DogumTarihi = DateTime.Now,
                   Kurum = 82,
                   Rol = 825,
                   Pozisyon = 430,
                   TCKimlikNo = "234242",
                   EpostaAdresi = "caner1@mail.com",
                   MedeniHali = 1,
                   IseGirisTarihi = DateTime.Now,
                   Lokasyon = 1,
                   SicilNo = "233432",
                   //TemelBilgiTabloID = 97,
                   DilListesi = new List<DilModel>()
                   {
                        new DilModel()
                        {
                            DilSeviye = "",
                            YabanciDilTipi = 1
                        }
                   },
                   AdresListesi = new List<AdresModel>()
                   {
                        new AdresModel()
                        {
                            Adres = "sdffd",
                            AdresTipi = 1,
                            Sehir = 1,
                            Ulke = 1
                        }
                   },
                   OkulListesi = new List<OkulModel>()
                   {
                        new OkulModel()
                        {
                            Fakulte = "Muhendislik",
                            MezuniyetTarihi = DateTime.Now,
                            OkulAdi = "Istanbul",
                            OkulTipi = 1
                        }
                   },
                   TelefonListesi = new List<TelefonModel>()
                   {
                        new TelefonModel()
                        {
                            TelefonNo = "",
                            TelefonTipi = 1
                        }
                   },
               });
            Assert.AreEqual(üst.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(üst.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(üst.Result);

            // Mail ile kişi getirme testi
            // Assert
            var BasicKisiModelSingleOrDefaultTest =
                _helper.Get<Result<BasicKisiModel>>($"/api/KisiService/MailileGetir/" + "caner1@mail.com");
            Assert.AreEqual(BasicKisiModelSingleOrDefaultTest.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(BasicKisiModelSingleOrDefaultTest.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(BasicKisiModelSingleOrDefaultTest.Result.Value);

            // Kişi bilgilerinin kaydedildiği olumsuz test metodu
            //Assert-2 negativeAdd
            var kisitemelkaydetolumsuz = _helper.Post<Result<KisiTemelKayitModel>>($"/api/KisiService/KisiTemelKaydet",
                 new KisiTemelKayitModel
                 {
                     Adi = "Murat",
                     Soyadi = "ArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslanArslan",
                     AnneAdi = "Hacer",
                     BabaAdi = "Mustafa",
                     Cinsiyeti = 1,
                     Departman = 4,
                     Dini = 1,
                     DogduguSehir = 1,
                     DogduguUlke = 1,
                     DogumTarihi = DateTime.Now,
                     Kurum = 82,
                     Rol = 1,
                     Pozisyon = 1,
                     TCKimlikNo = "234242",
                     EpostaAdresi = "murat1@mail.com",
                     MedeniHali = 1,
                     IseGirisTarihi = DateTime.Now,
                     Lokasyon = 1,
                     SicilNo = "233432",
                     //TemelBilgiTabloID = 97,
                     DilListesi = new List<DilModel>()
                     {
                        new DilModel()
                        {
                            DilSeviye = "",
                            YabanciDilTipi = 1
                        }
                     },
                     AdresListesi = new List<AdresModel>()
                     {
                        new AdresModel()
                        {
                            Adres = "sdffd",
                            AdresTipi = 1,
                            Sehir = 1,
                            Ulke = 1
                        }
                     },
                     OkulListesi = new List<OkulModel>()
                     {
                        new OkulModel()
                        {
                            Fakulte = "Muhendislik",
                            MezuniyetTarihi = DateTime.Now,
                            OkulAdi = "Istanbul",
                            OkulTipi = 1
                        }
                     },
                     TelefonListesi = new List<TelefonModel>()
                     {
                        new TelefonModel()
                        {
                            TelefonNo = "",
                            TelefonTipi = 1
                        }
                     }
                 });
            Assert.IsFalse(kisitemelkaydetolumsuz.Result.IsSuccess);

            // Kişi bilgilerinin güncellendiği test methodu
            //Assert-3 update
            var kisiguncelle = _helper.Post<Result<KisiTemelKayitModel>>($"/api/KisiService/KisiTemelKayitVerileriGuncelle", new KisiTemelKayitModel
            {
                TemelBilgiTabloID = kisitemelkaydet.Result.Value.TemelBilgiTabloID,
                Adi = "Guncel Unit Kisi",
                Soyadi = "Kişi",
                AnneAdi = "hj",
                BabaAdi = "hj",
                Cinsiyeti = 1,
                Departman = 4,
                Dini = 1,
                DogduguSehir = 1,
                DogduguUlke = 1,
                DogumTarihi = DateTime.Now,
                Kurum = 82,
                Rol = 825,
                Pozisyon = 1969,
                TCKimlikNo = "67876878",
                EpostaAdresi = "mehmet@mail.com",
                MedeniHali = 1,
                IseGirisTarihi = DateTime.Now,
                Lokasyon = 1,
                SicilNo = "578768",
                DilListesi = new List<DilModel>()
                    {
                        new DilModel()
                        {
                            DilSeviye = "orta",
                            YabanciDilTipi = 1
                        }
                    },
                AdresListesi = new List<AdresModel>()
                    {
                        new AdresModel()
                        {
                            Adres = "dghfgh",
                            AdresTipi = 1,
                            Sehir = 1,
                            Ulke = 1
                        }
                    },
                OkulListesi = new List<OkulModel>()
                    {
                        new OkulModel()
                        {
                            Fakulte = "",
                            MezuniyetTarihi = DateTime.Now,
                            OkulAdi = "",
                            OkulTipi = 1
                        }
                    },
                TelefonListesi = new List<TelefonModel>()
                    {
                        new TelefonModel()
                        {
                            TelefonNo = "5555555555",
                            TelefonTipi = 1
                        }
                    }
            });
            Assert.AreEqual(kisiguncelle.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisiguncelle.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisiguncelle.Result);

            // Kişi bilgilerinin güncellendiği test methodu
            //Assert-4 negativeUpdate
            var kisiguncelleolumsuz = _helper.Post<Result<KisiTemelKayitModel>>($"/api/KisiService/KisiTemelKayitVerileriGuncelle", new KisiTemelKayitModel
            {
            });
            Assert.IsFalse(kisiguncelleolumsuz.Result.IsSuccess);

            // Kişi listesi getiren test methodu
            //Assert-5 List
            var listegetir = _helper.Get<Result<List<KisiListeModel>>>($"/api/KisiService/TemelKisiListesiGetir/{kisitemelkaydet.Result.Value.Kurum}");
            Assert.AreEqual(listegetir.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(listegetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(listegetir.Result);

            // Kişi temel listesi getiren test methodu
            //Assert 5.1 Tüm Kisiler
            var getir = _helper.Get<Result<List<KisiTemelBilgiler>>>($"/api/KisiService/KisiListesiGetir");
            Assert.AreEqual(getir.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(getir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(getir.Result);

            //Kisi temel bilgiler tablo Id değeri ile kişi hassas bilgilerinin getirildiği HTTPGet test metodu.
            //Assert-6 GetById
            var kisihassasbilgilergetir = _helper.Get<Result<KisiHassasBilgiler>>($"/api/KisiService/KisiHassasBilgilerGetir/" + kisitemelkaydet.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(kisihassasbilgilergetir.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisihassasbilgilergetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisihassasbilgilergetir.Result);

            // Kisi temel bilgiler tablo Id değeri ile kişi hassas bilgilerinin getirildiği HTTPGet test metodu.
            //Assert-6.1 GetById
            var kisihassasbilgilergetirOlumsuz = _helper.Get<Result<KisiHassasBilgiler>>($"/api/KisiService/KisiHassasBilgilerGetir/" + 0);
            Assert.IsNull(kisihassasbilgilergetirOlumsuz.Result.Value);

            //  Id'ye göre kişiyi döndür test metodu.
            //Assert- IdyeGoreKisiGetir
            var IdyeGoreKisiGetir = _helper.Get<Result<KisiTemelBilgiler>>($"/api/KisiService/IdyeGoreKisiGetir/" + kisitemelkaydet.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(IdyeGoreKisiGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(IdyeGoreKisiGetir.Result.Value);

            //  Id'ye göre kişiyi döndür test metodu.
            //Assert- KisiTemelKayitVerileriGetir
            var KisiTemelKayitVerileriGetir = _helper.Post<Result<KisiTemelKayitModel>>($"/api/KisiService/KisiTemelKayitVerileriGetir", kisitemelkaydet.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(KisiTemelKayitVerileriGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(KisiTemelKayitVerileriGetir.Result.Value);

            //  Id'ye göre kişiyi döndür test metodu. olumsuz
            //Assert- IdyeGoreKisiGetirOlumsuz
            //var IdyeGoreKisiGetirOlumsuz = _helper.Get<Result<KisiTemelBilgiler>>($"/api/KisiService/IdyeGoreKisiGetir/" + 0);
            //Assert.IsNull(IdyeGoreKisiGetirOlumsuz.Result.Value);

            // Kurum organizasyon birimlerine göre kişinin amirlerini getiren test methodu
            //Assert-7 KisiAmirlerListGetir
            var listgetir = _helper.Get<Result<List<KisiOrganizasyonBirimView>>>($"/api/KisiService/KisiAmirlerListGetir/{kisitemelkaydet.Result.Value.TemelBilgiTabloID}");
            Assert.AreEqual(listgetir.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(listgetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(listgetir.Result);

            // Kurum organizasyon birimlerine göre kişinin astlarını getiren test methodu
            //Assert-8 KisiAstlarListGetir
            var astlistgetir = _helper.Get<Result<List<KisiOrganizasyonBirimView>>>($"/api/KisiService/KisiAstlarListGetir/" + üst.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(astlistgetir.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(astlistgetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(astlistgetir.Result);

            // KisiId değeri ile ilgili kişinin sifre son yenileme tarihi değerini getiren test methodu
            //Assert-9 KisiSifreSonYenilemeTarihiGetir
            var tarihgetir = _helper.Get<Result<DateTime>>($"/api/KisiService/KisiSifreSonYenilemeTarihiGetir/{kisitemelkaydet.Result.Value.TemelBilgiTabloID}");
            Assert.AreEqual(tarihgetir.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(tarihgetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(tarihgetir.Result);

            // Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini pasif eden test metotu.
            //Assert-10 HesabiPasifEt
            var pasifet = _helper.Get<Result<KisiTemelBilgiler>>($"/api/KisiService/HesabiPasifEt/{kisitemelkaydet.Result.Value.TemelBilgiTabloID}");
            Assert.AreEqual(pasifet.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(pasifet.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(pasifet.Result.Value);

            // Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini pasif eden test metotu.
            //Assert-10.1 HesabiPasifEtOlumsuz
            var pasifetOlumsuz = _helper.Get<Result<KisiTemelBilgiler>>($"/api/KisiService/HesabiPasifEt/" + 0);
            Assert.IsNull(pasifetOlumsuz.Result.Value);

            //  Kuruma ait kişileri kurumId'ye göre listeleyen method
            //Assert KurumaBagliKisilerList
            var KurumaBagliKisilerList = _helper.Get<Result<List<KisiTemelBilgiler>>>($"/api/KisiService/KurumaBagliKisilerList/" + kisitemelkaydet.Result.Value.Kurum);
            Assert.AreEqual(KurumaBagliKisilerList.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(KurumaBagliKisilerList.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(KurumaBagliKisilerList.Result.Value);

            //Idlere göre kişiyi döndürür.
            var kisiIds = new List<int>()
            {
                129,130
            };
            var IdlereGoreKisileriGetir = _helper.Post<Result<List<KisiTemelBilgiler>>>($"/api/KisiService/IdlereGoreKisileriGetir", kisiIds);
            Assert.AreEqual(IdlereGoreKisileriGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(IdlereGoreKisileriGetir.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(IdlereGoreKisileriGetir.Result.Value);

            // Kişi bilgilerini aktif yapan method
            var TemelKisiAktifYap = _helper.Get<Result<bool>>($"/api/KisiService/TemelKisiAktifYap/" + kisitemelkaydet.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(TemelKisiAktifYap.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsTrue(TemelKisiAktifYap.Result.Value);

            // Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini pasif eden  test metod
            var KisiPasifEtme = _helper.Get<Result<KisiTemelBilgiler>>($"/api/KisiService/PasifEt/" + kisitemelkaydet.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(KisiPasifEtme.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(KisiPasifEtme.Result.Value);

            // Kisi id ile KisiTemelBilgiler ve KisiHassasBilgiler tablosundaki kayıtları bulan ve AktifMi değerini aktif eden  test metod
            var KisiAktiveEtme = _helper.Post<Result<KisiTemelBilgiler>>($"/api/KisiService/AktifEt/" + kisitemelkaydet.Result.Value.TemelBilgiTabloID, kisitemelkaydet.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(KisiAktiveEtme.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(KisiAktiveEtme.Result.Value);

            // Kişi bilgilerini aktif yap olumsuz
            var TemelKisiAktifYapnegative = _helper.Get<Result<bool>>($"/api/KisiService/TemelKisiAktifYap/" + 0);
            Assert.IsFalse(TemelKisiAktifYapnegative.Result.Value);

            // Pasif Kişi Listesini getiren test metotu.
            var PasifKisiListesiGetir = _helper.Get<Result<List<KisiTemelBilgiler>>>($"/api/KisiService/PasifKisiListesiGetir");
            Assert.AreEqual(PasifKisiListesiGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(PasifKisiListesiGetir.Result.Value);

            // MusteriTemsilcisiBagliKisileri getiren test metotu.
            var MusteriTemsilcisiBagliKisilerList = _helper.Get<Result<List<KisiListeModel>>>($"/api/KisiService/MusteriTemsilcisiBagliKisilerList/" + üst.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(MusteriTemsilcisiBagliKisilerList.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(MusteriTemsilcisiBagliKisilerList.Result.Value);

            // AmirlereAstMusteriTemsilcisiKisileriniGetir
            var AmirlereAstMusteriTemsilcisiKisileriniGetir = _helper.Get<Result<List<KisiListeModel>>>($"/api/KisiService/AmirlereAstMusteriTemsilcisiKisileriniGetir/" + üst.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(AmirlereAstMusteriTemsilcisiKisileriniGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(AmirlereAstMusteriTemsilcisiKisileriniGetir.Result.Value);

            //AmireBagliMusteriTemsilcileriList
            var AmireBagliMusteriTemsilcileriList = _helper.Get<Result<List<int>>>($"/api/KisiService/AmireBagliMusteriTemsilcileriList/" + üst.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(AmireBagliMusteriTemsilcileriList.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(AmireBagliMusteriTemsilcileriList.Result.Value);

            // AmirveyaMusteriTemsilcisiKurumlariIDGetir
            var AmirveyaMusteriTemsilcisiKurumlariIDGetir = _helper.Get<Result<List<int>>>($"/api/KisiService/AmirveyaMusteriTemsilcisiKurumlariIDGetir/" + kisitemelkaydet.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(AmirveyaMusteriTemsilcisiKurumlariIDGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(AmirveyaMusteriTemsilcisiKurumlariIDGetir.Result.Value);

            // AmirveyaMusteriTemsilcisiKurumlariIDGetir2
            var AmirveyaMusteriTemsilcisiKurumlariIDGetir2 = _helper.Get<Result<List<int>>>($"/api/KisiService/AmirveyaMusteriTemsilcisiKurumlariIDGetir/" + 130);
            Assert.AreEqual(AmirveyaMusteriTemsilcisiKurumlariIDGetir2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(AmirveyaMusteriTemsilcisiKurumlariIDGetir2.Result.Value);

            // IdlereGoreKisiListeModelGetir
            var IdlereGoreKisiListeModelGetir = _helper.Post<Result<List<KisiListeModel>>>($"/api/KisiService/IdlereGoreKisiListeModelGetir", new List<int>()
            {
                kisitemelkaydet.Result.Value.TemelBilgiTabloID
            });
            Assert.AreEqual(IdlereGoreKisiListeModelGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(IdlereGoreKisiListeModelGetir.Result.Value);

            // SifreAtama
            var SifreAtama = _helper.Post<Result<bool>>($"/api/KisiService/SifreAtama", new SifreAtamaModel()
            {
                KisiId = kisitemelkaydet.Result.Value.TemelBilgiTabloID,
                KisiSifre = "12345aA!"
            });
            Assert.AreEqual(SifreAtama.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(SifreAtama.Result.Value);

            // HiyerarsiDisiKisilerKisiListesi
            var HiyerarsiDisiKisilerKisiListesi =
                _helper.Get<Result<List<KisiListeModel>>>($"/api/KisiService/HiyerarsiDisiKisilerKisiListesi");
            Assert.AreEqual(HiyerarsiDisiKisilerKisiListesi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(HiyerarsiDisiKisilerKisiListesi.Result.Value);

            // Kişi bilgilerini silindi yapan test methodu
            //Assert-11 Delete
            var kisisilindi = _helper.Get<Result<bool>>($"/api/KisiService/TemelKisiSilindiYap/{kisitemelkaydet.Result.Value.TemelBilgiTabloID}");
            Assert.AreEqual(kisisilindi.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisisilindi.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsTrue(kisisilindi.Result.Value);

            // Kişi bilgilerini silindi yapan test methodu
            //Assert-11.1 DeleteOlumsuz
            var kisisilindiOlumsuz = _helper.Get<Result<bool>>($"/api/KisiService/TemelKisiSilindiYap/" + 0);
            Assert.IsFalse(kisisilindiOlumsuz.Result.Value);

            //  Excel dosyasından import edilen kişi listesinin kaydedilmesi test methodu
            //Assert ListeIleTemelKisikaydet
            var kisi1 = new KisiTemelKayitModel
            {
                Adi = "Unit Yeni",
                Soyadi = "Test",
                AnneAdi = "Hacer",
                BabaAdi = "Mustafa",
                Cinsiyeti = 1,
                Departman = 4,
                Dini = 1,
                DogduguSehir = 1,
                DogduguUlke = 1,
                DogumTarihi = DateTime.Now,
                Kurum = 82,
                Rol = 1,
                Pozisyon = 1,
                TCKimlikNo = "234242",
                EpostaAdresi = "caner1@mail.com",
                MedeniHali = 1,
                IseGirisTarihi = DateTime.Now,
                Lokasyon = 1,
                SicilNo = "233432",
                //TemelBilgiTabloID = 97,
                DilListesi = new List<DilModel>()
                {
                    new DilModel()
                    {
                        DilSeviye = "",
                        YabanciDilTipi = 1
                    }
                },
                AdresListesi = new List<AdresModel>()
                {
                    new AdresModel()
                    {
                        Adres = "sdffd",
                        AdresTipi = 1,
                        Sehir = 1,
                        Ulke = 1
                    }
                },
                OkulListesi = new List<OkulModel>()
                {
                    new OkulModel()
                    {
                        Fakulte = "Muhendislik",
                        MezuniyetTarihi = DateTime.Now,
                        OkulAdi = "Istanbul",
                        OkulTipi = 1
                    }
                },
                TelefonListesi = new List<TelefonModel>()
                {
                    new TelefonModel()
                    {
                        TelefonNo = "",
                        TelefonTipi = 1
                    }
                },
            };
            var kisi2 = new KisiTemelKayitModel
            {
                Adi = "Unit Yeni",
                Soyadi = "Test",
                AnneAdi = "Hacer",
                BabaAdi = "Mustafa",
                Cinsiyeti = 1,
                Departman = 4,
                Dini = 1,
                DogduguSehir = 1,
                DogduguUlke = 1,
                DogumTarihi = DateTime.Now,
                Kurum = 82,
                Rol = 1,
                Pozisyon = 1,
                TCKimlikNo = "234242",
                EpostaAdresi = "caner1@mail.com",
                MedeniHali = 1,
                IseGirisTarihi = DateTime.Now,
                Lokasyon = 1,
                SicilNo = "233432",
                //TemelBilgiTabloID = 97,
                DilListesi = new List<DilModel>()
                {
                    new DilModel()
                    {
                        DilSeviye = "",
                        YabanciDilTipi = 1
                    }
                },
                AdresListesi = new List<AdresModel>()
                {
                    new AdresModel()
                    {
                        Adres = "sdffd",
                        AdresTipi = 1,
                        Sehir = 1,
                        Ulke = 1
                    }
                },
                OkulListesi = new List<OkulModel>()
                {
                    new OkulModel()
                    {
                        Fakulte = "Muhendislik",
                        MezuniyetTarihi = DateTime.Now,
                        OkulAdi = "Istanbul",
                        OkulTipi = 1
                    }
                },
                TelefonListesi = new List<TelefonModel>()
                {
                    new TelefonModel()
                    {
                        TelefonNo = "",
                        TelefonTipi = 1
                    }
                },
            };
            List<KisiTemelKayitModel> kisikayitlar = new();
            kisikayitlar.Add(kisi1);
            kisikayitlar.Add(kisi2);
            var ListeIleTemelKisikaydet = _helper.Post<Result<List<KisiTemelKayitModel>>>($"/api/KisiService/ListeIleTemelKisikaydet", kisikayitlar);
            Assert.AreEqual(ListeIleTemelKisikaydet.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(ListeIleTemelKisikaydet.Result.Value);

            var kisisilindi2 = _helper.Get<Result<bool>>($"/api/KisiService/TemelKisiSilindiYap/{ListeIleTemelKisikaydet.Result.Value[0].TemelBilgiTabloID}");
            Assert.AreEqual(kisisilindi2.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisisilindi2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsTrue(kisisilindi2.Result.Value);

            var kisisilindi3 = _helper.Get<Result<bool>>($"/api/KisiService/TemelKisiSilindiYap/{ListeIleTemelKisikaydet.Result.Value[1].TemelBilgiTabloID}");
            Assert.AreEqual(kisisilindi3.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisisilindi3.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsTrue(kisisilindi3.Result.Value);

            var kisisilindi4 = _helper.Get<Result<bool>>($"/api/KisiService/TemelKisiSilindiYap/" + üst.Result.Value.TemelBilgiTabloID);
            Assert.AreEqual(kisisilindi4.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisisilindi4.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsTrue(kisisilindi4.Result.Value);

            var kisisilindiNegative = _helper.Get<Result<bool>>($"/api/KisiService/TemelKisiSilindiYap/" + 0);
            Assert.IsFalse(kisisilindiNegative.Result.Value);
        }

        #endregion KisiTemelVeriCrud(Kişi İşlemlerinin testidir. Kişi Temel ve Hassas Bilgilerinin işlendiğği methodların testi yapılmıştır.)

        #region Sifre Testleri

        /// <summary>
        /// Mail ile şifre yenileme testi
        /// </summary>
        [TestMethod()]
        public void SifreYenilemeTestiMail()
        {
            //Assert sifreyenilememail
            var sifreyenilememail = _helper.Get<Result<SistemLoginSifreYenilemeAktivasyonHareketleri>>($"/api/KisiService/SifreYenileMailile/" + "tester@mail.com");
            Assert.AreEqual(sifreyenilememail.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(sifreyenilememail.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(sifreyenilememail.Result);
            var guidarray = sifreyenilememail.Result.Value.SifreYenilemeSayfasiGeciciUrl.Split("=");
            var guid = guidarray[1];

            //Assert SifreYenilemeGecerlilikTesti
            var SifreYenilemeGecerlilikTesti = _helper.Post<Result<bool>>($"/api/KisiService/SifreYenilemeGecerliMi", guid);
            Assert.AreEqual(SifreYenilemeGecerlilikTesti.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(SifreYenilemeGecerlilikTesti.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(SifreYenilemeGecerlilikTesti.Result.Value);

            string[] value2 = sifreyenilememail.Result.Value.SifreYenilemeSayfasiGeciciUrl.Split('=');
            var model1 = new SifreModel()
            {
                //"123456Aa!"
                //"123456aA!"
                Email = "tester@mail.com",
                KisiID = 3603,
                GecerlilikZamani = DateTime.Now,
                OldPassword = "12345Aa!",
                NewPassword = "123456Aa!",
                NewPasswordTekrar = "123456Aa!",
                kontrolGUID = value2[1]
            };

            //Assert SifreDegistirmeTesti
            var SifreDegistirmeTesti = _helper.Post<Result<SifreModel>>($"/api/KisiService/SifreDegistir", model1);
            Assert.AreEqual(SifreDegistirmeTesti.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(SifreDegistirmeTesti.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(SifreDegistirmeTesti.Result);

            var model2 = new SifreModel()
            {
                //"123456Aa!"
                //"123456aA!"
                Email = "tester@mail.com",
                KisiID = 342,
                GecerlilikZamani = DateTime.Now,
                OldPassword = model1.NewPassword,
                NewPassword = model1.OldPassword,
                NewPasswordTekrar = model1.OldPassword,
                kontrolGUID = value2[1]
            };

            // ŞifreYenilemeTesti
            var SifreYenilemeTesti = _helper.Post<Result<SifreModel>>($"/api/KisiService/SifreYenile", model2);
            Assert.AreEqual(SifreDegistirmeTesti.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(SifreDegistirmeTesti.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(SifreYenilemeTesti.Result);

            //Assert ŞifreYenilemeTesti Olumsuz
            var sifreModelOlumsuz = _helper.Post<Result<SifreModel>>($"/api/KisiService/SifreYenile", new SifreModel
            {
                Email = "bilal",
                KisiID = 129,
                GecerlilikZamani = DateTime.Parse("2020-10-11"),
                NewPassword = "12345Aa!",
                NewPasswordTekrar = "12345Aa!",
                kontrolGUID = "eed41b51-42fd-4784-93e9-12b35d222e67"
            });
            Assert.IsFalse(sifreModelOlumsuz.Result.IsSuccess);

            //pasif kisi sifre yenileme
            //kisiPasifEt
            var pasifet = _helper.Get<Result<KisiTemelBilgiler>>($"/api/KisiService/HesabiPasifEt/{sifreyenilememail.Result.Value.KisiID}");
            Assert.AreEqual(pasifet.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(pasifet.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(pasifet.Result.Value);

            //Assert sifreyenilememail pasif
            var sifreyenilememail2 = _helper.Get<Result<SistemLoginSifreYenilemeAktivasyonHareketleri>>($"/api/KisiService/SifreYenileMailile/" + "tester@mail.com");
            Assert.AreEqual(sifreyenilememail2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(sifreyenilememail2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(sifreyenilememail2.Result);

            var TemelKisiAktifYap = _helper.Get<Result<bool>>($"/api/KisiService/TemelKisiAktifYap/" + sifreyenilememail.Result.Value.KisiID);
            Assert.AreEqual(TemelKisiAktifYap.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsTrue(TemelKisiAktifYap.Result.Value);
        }

        #endregion Sifre Testleri

        #region CrudTestKisiIliski

        /// <summary>
        /// Kişiler arası ilişkiyi  test metodu
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public void CrudTestKisiIliski()
        {
            //Assert-1 Add //130
            var kisiiliskikaydet = _helper.Post<Result<Iliskiler>>($"/api/KisiService/KisiIliskiKaydet", new KisiIliskiKayitModel
            {
                BuKisiID = 130,
                BununKisiID = 129,
                IliskiTuruID = 7,
                KurumID = 82
            });
            Assert.AreEqual(kisiiliskikaydet.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisiiliskikaydet.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisiiliskikaydet.Result);

            //Assert-1.1 Add //130
            var kisiiliskikaydet1 = _helper.Post<Result<Iliskiler>>($"/api/KisiService/KisiIliskiKaydet", new KisiIliskiKayitModel
            {
                BuKisiID = 130,
                BununKisiID = 82,
                IliskiTuruID = 11,
                KurumID = 82
            });
            Assert.AreEqual(kisiiliskikaydet1.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisiiliskikaydet1.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisiiliskikaydet1.Result);

            //Assert-2 negativeAdd

            var kisiiliskikaydetolumsuz = _helper.Post<Result<Iliskiler>>($"/api/KisiService/KisiIliskiKaydet", new KisiIliskiKayitModel
            {
                BuKisiID = 130,
                BununKisiID = 129,
                IliskiTuruID = 7,
                KurumID = 82
            });
            Assert.IsFalse(kisiiliskikaydetolumsuz.Result.IsSuccess);

            //Assert-21 negativeAdd

            var kisiiliskikaydetolumsuz1 = _helper.Post<Result<Iliskiler>>($"/api/KisiService/KisiIliskiKaydet", new KisiIliskiKayitModel
            {
                BuKisiID = 129,
                BununKisiID = 130,
                IliskiTuruID = 7,
                KurumID = 82
            });
            Assert.IsFalse(kisiiliskikaydetolumsuz1.Result.IsSuccess);

            //Assert-3 update

            var kisiiliskiguncelle = _helper.Post<Result<Iliskiler>>($"/api/KisiService/KisiIliskiGuncelle", new KisiIliskiKayitModel
            {
                TabloID = kisiiliskikaydet.Result.Value.TabloID,
                BuKisiID = 129,
                BununKisiID = 210,
                IliskiTuruID = 7,
                KurumID = 82
            });
            Assert.AreEqual(kisiiliskiguncelle.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kisiiliskiguncelle.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kisiiliskiguncelle.Result);

            //Assert-3.0 update

            var kisiiliskiguncelle1 = _helper.Post<Result<Iliskiler>>($"/api/KisiService/KisiIliskiGuncelle", new KisiIliskiKayitModel
            {
                TabloID = kisiiliskikaydet1.Result.Value.TabloID,
                BuKisiID = 130,
                BununKisiID = 85,
                IliskiTuruID = 11,
                KurumID = 82
            });
            Assert.AreEqual(kisiiliskiguncelle1.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kisiiliskiguncelle1.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kisiiliskiguncelle1.Result);

            //Assert-3.1 updateOlumsuz

            var kisiiliskiguncelleOlumsuz = _helper.Post<Result<Iliskiler>>($"/api/KisiService/KisiIliskiGuncelle", new KisiIliskiKayitModel
            {
                TabloID = kisiiliskikaydet.Result.Value.TabloID,
                BuKisiID = 129,
                BununKisiID = 210,
                IliskiTuruID = 7,
                KurumID = 82
            });
            Assert.IsFalse(kisiiliskiguncelleOlumsuz.Result.IsSuccess);

            //Assert-3.2 updateOlumsuz

            var kisiiliskiguncelleOlumsuz1 = _helper.Post<Result<Iliskiler>>($"/api/KisiService/KisiIliskiGuncelle", new KisiIliskiKayitModel
            {
                TabloID = kisiiliskikaydet.Result.Value.TabloID,
                BuKisiID = 210,
                BununKisiID = 129,
                IliskiTuruID = 7,
                KurumID = 82
            });
            Assert.IsFalse(kisiiliskiguncelleOlumsuz1.Result.IsSuccess);

            //Assert-4 MusteriList
            var MusteriList = _helper.Get<Result<List<Iliskiler>>>($"/api/KisiService/MusteriList/" + kisiiliskikaydet.Result.Value.KurumID);
            Assert.AreEqual(MusteriList.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(MusteriList.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(MusteriList.Result);

            //Assert-4 list
            var kisiiliskilist = _helper.Get<Result<List<Iliskiler>>>($"/api/KisiService/KisiIliskiList/" + kisiiliskikaydet.Result.Value.KurumID);
            Assert.AreEqual(kisiiliskilist.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisiiliskilist.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisiiliskilist.Result);

            //Assert-5 deleteOlumsuz
            var deleteOlumsuz = _helper.Get<Result<bool>>($"/api/KisiService/KisiIliskiSil/" + 0);
            Assert.IsNotNull(deleteOlumsuz.Result);

            //Assert-5 delete
            var kisiiliskisil = _helper.Get<Result<bool>>($"/api/KisiService/KisiIliskiSil/" + kisiiliskikaydet.Result.Value.TabloID);
            Assert.AreEqual(kisiiliskisil.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisiiliskisil.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisiiliskisil.Result);

            //Assert-5.1 delete
            var kisiiliskisil1 = _helper.Get<Result<bool>>($"/api/KisiService/KisiIliskiSil/" + kisiiliskikaydet1.Result.Value.TabloID);
            Assert.AreEqual(kisiiliskisil1.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisiiliskisil1.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisiiliskisil1.Result);
        }

        #endregion CrudTestKisiIliski

        /// <summary>
        /// Parametre tanımına göre TipId döndüren methot testi olumsuz
        /// </summary>
        [TestMethod()]
        public void GetTipId()
        {
            //Assert Parametre tanımına göre TipId döndüren methot testi olumsuz
            var GetTipId = _helper.Get<Result<int>>($"/api/KisiService/GetTipId/" + "yokkiboylebisey");
            Assert.IsFalse(GetTipId.Result.IsSuccess);
            Assert.AreEqual(GetTipId.Result.Value, 0);
        }
    }
}