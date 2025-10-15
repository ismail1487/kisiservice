using Baz.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baz.Model.Entity;
using Baz.Repository.Pattern;
using Baz.Mapper.Pattern;
using Microsoft.Extensions.Logging;
using Baz.ProcessResult;
using Baz.Model.Entity.ViewModel;
using Baz.AletKutusu;
using Microsoft.AspNetCore.Mvc;

namespace Baz.Service
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMesajSeriGenelTanimlarService : IService<MesajSeriGenelTanimlar>
    {
        /// <summary>
        /// Okunmamış Son bildirim veya mesajların listesi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result<NotificationList> List(int id);

        /// <summary>
        /// Okundu bilgisinin gönderilmesi.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result<bool> SetSeen(int id);
    }
    
    
    /// <summary>
    /// 
    /// </summary>
    public class MesajSeriGenelTanimlarService : Service<MesajSeriGenelTanimlar>, IMesajSeriGenelTanimlarService
    {
        private readonly IMesajAkisIcerikService _mesajAkis;
        private readonly IKisiService _kisiService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public MesajSeriGenelTanimlarService(IRepository<MesajSeriGenelTanimlar> repository, IMesajAkisIcerikService mesajAkisIcerikService, IKisiService kisiService,  IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<MesajSeriGenelTanimlarService> logger) : base(repository, dataMapper, serviceProvider, logger)
        {
            _mesajAkis = mesajAkisIcerikService;
            _kisiService = kisiService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result<NotificationList> List(int id)
        {
            /*
             
            
            select * from  MesajAkisIcerik where AktifMi=1 and MesajHedefKisiID=3090 and MesajGorulmeTarihi is null 
            and TabloID in (select SeriSonMesajID from MesajSeriGenelTanimlar where MesajSeriIlgiliKisi1ID=3090 or MesajSeriIlgiliKisi2ID=3090)

             */
            // Bu kişinin içerisinde bulunduğu bir mesaj başlığı, aslında MesajSeriIlgiliKisi2Id kendisidir,
            var genel = List(x => x.AktifMi == 1 && (x.MesajSeriIlgiliKisi1Id == id || x.MesajSeriIlgiliKisi2Id == id)).Value.Select(x=>x.SeriSonMesajId);
            
            // Okunmamış ve SonMesajID si belli olan kayıtlar seçilir.
            var mesajlar = _mesajAkis.List(x => x.AktifMi == 1 && x.MesajHedefKisiId==id && x.MesajGorulmeTarihi==null && genel.Contains(x.TabloID)).Value;

            //var kisi = _kisiTemelBilgilerService.SingleOrDefault(item.KisiID).Value;



            var notifications = new NotificationList();
            notifications.Item = mesajlar.Select(p => new NotificationItem()
            {
                Id = p.TabloID,
                Date = p.KayitTarihi.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = _kisiService.GetAdSoyad(p.MesajAtanKisiId),
                SeenStatus = 0,
                Url = "/panel/mesajlar"
            }).ToList();
            notifications.Count = notifications.Item.Count;

            


            return notifications.ToResult();
        }

        public Result<bool> SetSeen(int id)
        {

            return true.ToResult();
        }
    }


}