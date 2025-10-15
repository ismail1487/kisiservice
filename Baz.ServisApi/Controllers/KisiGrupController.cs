using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.Service;
using Gelf.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Baz.KisiServisApi.Controllers
{
    /// <summary>
    /// Kişi Grup İlemlerinin yapıldığı api.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class KisiGrupController : ControllerBase
    {
        private readonly IKisiGrupService _kisiGrupService;
        private readonly IKurumOrganizasyonBirimTanimlariService _kurumOrganizasyonBirimTanimlariService;

        /// <summary>
        ///  Kişi Grup İlemlerinin yapıldığı api sınıfının yapıcı methodu
        /// </summary>
        /// <param name="kisiGrupService"></param>
        /// <param name="logger"></param>
        /// <param name="kurumOrganizasyonBirimTanimlariService"></param>
        public KisiGrupController(IKisiGrupService kisiGrupService, ILogger<KisiGrupController> logger, IKurumOrganizasyonBirimTanimlariService kurumOrganizasyonBirimTanimlariService)
        {
            List<KeyValuePair<string, object>> keys = new();
            keys.Add(new KeyValuePair<string, object>("Model", JsonConvert.SerializeObject(new LoginModel())));

            GelfLogScope.Push(keys);
            logger.LogInformation(LogLevel.Information.ToString());
            _kisiGrupService = kisiGrupService;
            _kurumOrganizasyonBirimTanimlariService = kurumOrganizasyonBirimTanimlariService;
        }

        /// <summary>
        /// Kişi Grup Kaydetme işlemini yapan method.
        /// </summary>
        [HttpPost]
        [Route("KisiGrupKaydet")]
        public Result<KisiGrupKayitViewModel> KisiGrupKaydet([FromBody] KisiGrupKayitViewModel model)
        {
            var result = _kisiGrupService.KisiGrupKaydet(model);
            return result;
        }

        /// <summary>
        /// Kişi Grup güncelle işlemini yapan method.
        /// </summary>
        [HttpPost]
        [Route("KisiGrupGuncelle")]
        public Result<KisiGrupKayitViewModel> KisiGrupGuncelle([FromBody] KisiGrupKayitViewModel model)
        {
            var result = _kisiGrupService.KisiGrupGuncelle(model);
            return result;
        }

        /// <summary>
        /// Kişi Grup Listeleme işlemini yapan method.
        /// </summary>
        [HttpGet]
        [Route("KisiGrupListeleme/{kurumId}")]
        public Result<List<KisiGrupListViewModel>> KisiGrupListeleme(int kurumId)
        {
            var result = _kisiGrupService.KisiGrupListesiGetir(kurumId);
            return result;
        }

        /// <summary>
        /// Kişi Grup Silme işlemini yapan method.
        /// </summary>
        [HttpGet]
        [Route("KisiGrupSil/{id}")]
        public Result<bool> KisiGrupSil(int id)
        {
            var result = _kisiGrupService.KisiGrupTanimSil(id);
            return result;
        }

        /// <summary>
        /// Idye göre Kişi Grup getiren işlemini yapan method.
        /// </summary>
        [HttpGet]
        [Route("KisiGrupGetir/{id}")]
        public Result<KisiGrupListViewModel> KisiGrupGetir(int id)
        {
            var result = _kisiGrupService.KisiGrupGetir(id);
            return result;
        }

        /// <summary>
        /// Ekip Idye göre ekip içindeki kişileri getiren metod
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("EkipDetayGetir/{ekipId}")]
        public Result<List<KisiListeModel>> EkipIdyeGoreEkipKisileriniGetir(int ekipId)
        {
            var result = _kisiGrupService.EkipIdyeGoreEkipKisileriniGetir(ekipId);
            return result;
        }

        /// <summary>
        /// Ekip Idye göre ekip içindeki kişileri getiren metod
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("KurumOrganizasyonTanimEklemeTest")]
        public Result<KurumOrganizasyonBirimTanimlari> KurumOrganizasyonTanimEklemeTest(KurumOrganizasyonBirimTanimlari model)
        {
            var result = _kurumOrganizasyonBirimTanimlariService.Add(model);
            return result;
        }

        /// <summary>
        /// Ekip Idye göre ekip içindeki kişileri getiren metod
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("KurumOrganizasyonTanimSilmeTest/{tabloID}")]
        public Result<KurumOrganizasyonBirimTanimlari> KurumOrganizasyonTanimSilmeTest(int tabloID)
        {
            var result = _kurumOrganizasyonBirimTanimlariService.Delete(tabloID);
            return result;
        }
    }
}