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
    /// KisiGrup  testlerinin yapıldığı sınıftır.
    /// Listeleme, Silme metodlarında olumsuz test senaryosuna gerek duyulmamıştır.
    /// </summary>
    [TestClass()]
    public class KisiGrupTest
    {
        private readonly IRequestHelper _helper;

        /// <summary>
        /// KisiGrup  testlerinin yapıldığı sınıftır.
        /// </summary>
        public KisiGrupTest()
        {
            _helper = TestServerRequestHelper.CreateHelper();
        }

        /// <summary>
        /// Ekip Crud işlemleri Test methodu.
        /// </summary>
        [TestMethod()]
        public void KisiGrupCrudTests()
        {
            var guid = "Ekleme" + Guid.NewGuid().ToString().Substring(0, 5);
            var guid2 = "Güncelleme" + Guid.NewGuid().ToString().Substring(0, 5);
            //Kayıt
            var kisiGrupKaydet = _helper.Post<Result<KisiGrupKayitViewModel>>($"/api/KisiGrup/KisiGrupKaydet", new KisiGrupKayitViewModel
            {
                KisiIdList = new List<int> { 129, 130 },
                EkipIsmi = guid,
                KurumID = 82
            });
            Assert.AreEqual(kisiGrupKaydet.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisiGrupKaydet.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisiGrupKaydet.Result);

            //Olumsuz Kayıt (Gönderilen listenin sayısının 0 olması kontrol edilmiştir.)
            var kisiGrupKaydetOlumsuz = _helper.Post<Result<KisiGrupKayitViewModel>>($"/api/KisiGrup/KisiGrupKaydet", new KisiGrupKayitViewModel
            {
                KisiIdList = new List<int> { },
                EkipIsmi = guid,
                KurumID = 82
            });
            Assert.IsFalse(kisiGrupKaydetOlumsuz.Result.IsSuccess);

            //Güncelleme
            var kisiGrupGuncelle = _helper.Post<Result<KisiGrupKayitViewModel>>($"/api/KisiGrup/KisiGrupGuncelle", new KisiGrupKayitViewModel
            {
                KisiIdList = new List<int> { 130, 210 },
                OrganizasyonBirimiID = kisiGrupKaydet.Result.Value.OrganizasyonBirimiID,
                EkipIsmi = guid2,
                KurumID = 82
            });
            Assert.AreEqual(kisiGrupGuncelle.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.AreEqual(kisiGrupGuncelle.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(kisiGrupGuncelle.Result);

            //Olumsuz Güncelleme (Gönderilen listenin sayısının 0 olması kontrol edilmiştir.)
            var kisiGrupGuncelleOlumsuz = _helper.Post<Result<KisiGrupKayitViewModel>>($"/api/KisiGrup/KisiGrupGuncelle", new KisiGrupKayitViewModel
            {
            });
            Assert.IsFalse(kisiGrupGuncelleOlumsuz.Result.IsSuccess);

            // Listeleme
            var kisiGrupListeleme = _helper.Get<Result<List<KisiGrupKayitViewModel>>>($"/api/KisiGrup/KisiGrupListeleme/" + 82);
            Assert.AreEqual(kisiGrupListeleme.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisiGrupListeleme.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisiGrupListeleme.Result);

            //GetById
            var kisiGrupGetir = _helper.Get<Result<KisiGrupListViewModel>>($"/api/KisiGrup/KisiGrupGetir/" + kisiGrupKaydet.Result.Value.OrganizasyonBirimiID);
            Assert.AreEqual(kisiGrupGetir.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisiGrupGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(kisiGrupGetir.Result);

            //EkipDetayGetir
            var EkipDetayGetir = _helper.Get<Result<List<KisiListeModel>>>($"/api/KisiGrup/EkipDetayGetir/" + kisiGrupKaydet.Result.Value.OrganizasyonBirimiID);
            Assert.AreEqual(EkipDetayGetir.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(EkipDetayGetir.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(EkipDetayGetir.Result);

            //EkipDetayGetirOlumsuz
            var EkipDetayGetirOlumsuz = _helper.Get<Result<List<KisiListeModel>>>($"/api/KisiGrup/EkipDetayGetir/" + 0);
            Assert.AreEqual(EkipDetayGetirOlumsuz.StatusCode, HttpStatusCode.OK);
            Assert.IsNull(EkipDetayGetirOlumsuz.Result.Value);

            //GetByIdNegative
            var kisiGrupGetirOlumsuz = _helper.Get<Result<KisiGrupListViewModel>>($"/api/KisiGrup/KisiGrupGetir/" + 0);
            Assert.IsNull(kisiGrupGetirOlumsuz.Result.Value);

            //Silmeolumsuz
            var kisiGrupTanimSilOlumsuz = _helper.Get<Result<bool>>($"/api/KisiGrup/KisiGrupSil/" + 0);
            Assert.IsFalse(kisiGrupTanimSilOlumsuz.Result.Value);

            //Silme
            var kisiGrupTanimSil = _helper.Get<Result<bool>>($"/api/KisiGrup/KisiGrupSil/" + kisiGrupKaydet.Result.Value.OrganizasyonBirimiID);
            Assert.AreEqual(kisiGrupTanimSil.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(kisiGrupTanimSil.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsTrue(kisiGrupTanimSil.Result.Value);
        }
    }
}