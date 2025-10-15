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
    public class KisiMesajController : Controller
    {
        private readonly IKisiService _kisiService;
        private readonly ILoginUser _loginUser;
        private readonly IMesajSeriGenelTanimlarService _mesajSeriGenelTanimlarService;
        private readonly IMesajAkisIcerikService _mesajAkisIcerikService;
        /// <summary>
        /// Kişi Mesaj ve bildirimler ile ilgilenecek Servis.
        /// </summary>
        /// <param name="kisiService"></param>
        /// <param name="mesajAkisIcerikService"></param>
        /// <param name="mesajSeriGenelTanimlarService"></param>
        /// <param name="loginUser"></param>
        public KisiMesajController(IKisiService kisiService, 
            IMesajAkisIcerikService mesajAkisIcerikService,
            IMesajSeriGenelTanimlarService  mesajSeriGenelTanimlarService,
            ILoginUser loginUser)
        {
            _loginUser = loginUser;
            _kisiService = kisiService;
            _mesajAkisIcerikService = mesajAkisIcerikService;
            _mesajSeriGenelTanimlarService = mesajSeriGenelTanimlarService;
        }

        [HttpGet]
        [Route("List/{id}")]
        [ProcessName(Name = "Bekleyen Bildirimlerin listesi")]
        public Result<NotificationList> List(int id)
        {
            return _mesajSeriGenelTanimlarService.List(id);
        }

        [HttpGet]
        [Route("SetSeen/{id}")]
        [ProcessName(Name = "SetSeen")]
        public Result<bool> SetSeen(int id)
        {
            //return _hatirlatmaKayitlarService.SetSeen(id);
            return true.ToResult();
        }

        //[HttpGet]
        //[Route("Delete/{id}")]
        //[ProcessName(Name = "Delete")]
        //public Result<PostaciBekleyenIslemlerGenel> Delete(int id)
        //{
        //    //var response = _postaciBekleyenIslemlerGenelService.Delete(id);
        //    return response;
        //}




        /// <summary>
        /// Kişinin firebase token nını temizler
        /// </summary>
        /// <returns></returns>
        //[Route("AktifPasifKisiList")]
        //[HttpGet]
        //public Result<List<KisiTemelBilgiler>> AktifPasifKisiList()
        //{
        //    var result = _kisiService.List(x => x.KurumID == _loginUser.KurumID && x.KisiBagliOlduguKurumId == _loginUser.KurumID);
        //    return result;
        //}
    }
}