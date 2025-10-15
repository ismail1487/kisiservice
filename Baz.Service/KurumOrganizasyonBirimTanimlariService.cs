using Baz.Mapper.Pattern;
using Baz.Model.Entity;
using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.Repository.Pattern;
using Baz.RequestManager.Abstracts;
using Baz.Service.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Baz.Model.Pattern;

namespace Baz.Service
{
    /// <summary>
    /// Kurum Organizasyon Birim Tanımları için oluşturulmuş Interfacedir.
    /// </summary>
    public interface IKurumOrganizasyonBirimTanimlariService : IService<KurumOrganizasyonBirimTanimlari>
    {
        /// <summary>
        /// KurumId ve Name e göre listeleme yapan method.
        /// </summary>
        /// <param name="request"></param>
        public Result<List<KurumOrganizasyonBirimView>> ListTip(KurumOrganizasyonBirimRequest request);

        /// <summary>
        /// Kurum Organizasyon Birim Ekleme Methodu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result<int> Add(KurumOrganizasyonBirimView item);

        /// <summary>
        /// Kurum organizasyon biriminin adını değiştiren method
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public Result<bool> UpdateName(KurumOrganizasyonBirimView item);
    }

    /// <summary>
    /// Kurumların alt birimlerinin (Departman/Pozisyon/Rol/Lokasyon/Takım/vs) istendiği şekilde tanımlanabildiği KurumOrganizasyonBirimTanimlari tablosuyla ilgili işlemleri barındıran class.
    /// </summary>
    public class KurumOrganizasyonBirimTanimlariService : Service<KurumOrganizasyonBirimTanimlari>, IKurumOrganizasyonBirimTanimlariService
    {
        private readonly IParamOrganizasyonBirimleriService _paramOrg;
        private readonly IRequestHelper _requestHelper;
        private readonly ILoginUser _loginUser;

        /// <summary>
        /// Kurumların alt birimlerinin (Departman/Pozisyon/Rol/Lokasyon/Takım/vs) istendiği şekilde tanımlanabildiği KurumOrganizasyonBirimTanimlari tablosuyla ilgili işlemleri barındıran classın yapıcı metodu
        /// </summary>
        public KurumOrganizasyonBirimTanimlariService(IRepository<KurumOrganizasyonBirimTanimlari> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<KurumOrganizasyonBirimTanimlariService> logger, IParamOrganizasyonBirimleriService paramOrganizasyonBirimleriService, IRequestHelper requestHelper, ILoginUser loginUser) : base(repository, dataMapper, serviceProvider, logger)
        {
            _requestHelper = requestHelper;
            _loginUser = loginUser;
            _paramOrg = paramOrganizasyonBirimleriService;
        }

        /// <summary>
        /// KurumId ve Name e göre listeleme yapan method.
        /// </summary>
        /// <param name="request"></param>
        public Result<List<KurumOrganizasyonBirimView>> ListTip(KurumOrganizasyonBirimRequest request)
        {
            var r = _paramOrg.GetTipId(request.Name);
            int departmanId = r.Value;
            var result = _repository.List(p => p.IlgiliKurumId == request.KurumId && p.OrganizasyonBirimTipiId == departmanId && p.AktifMi == 1 && p.SilindiMi == 0).Select(p => new KurumOrganizasyonBirimView()
            {
                IlgiliKurumID = request.IlgiliKurumID,
                Tanim = p.BirimTanim,
                TabloId = p.TabloID,
                TipId = departmanId,
                KurumId = request.KurumId,
                UstId = p.UstId.Value,
                Koordinat = p.Koordinat
            }).ToList();

            var yeniList = result.ToResult().Value.OrderBy(x => x.Tanim).ToList();

            return yeniList.ToResult();
        }

        /// <summary>
        /// Kurum Organizasyon Birim Ekleme Methodu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result<int> Add(KurumOrganizasyonBirimView item)
        {
            var list = List(o => o.AktifMi == 1 && o.SilindiMi == 0 && o.OrganizasyonBirimTipiId == item.TipId && o.KurumID == _loginUser.KurumID).Value;
            bool varMi = list.Any(o => o.BirimTanim == item.Tanim);
            if (varMi)
            {
                return Results.Fail("Aynı kayıt bulunmaktadır.", ResultStatusCode.CreateError);
            }
            var entity = new KurumOrganizasyonBirimTanimlari()
            {
                KurumID = _loginUser.KurumID,
                IlgiliKurumId = item.KurumId,
                BirimTanim = item.Tanim,
                OrganizasyonBirimTipiId = item.TipId,
                UstId = item.UstId,
                GuncellenmeTarihi = DateTime.Now,
                KayitTarihi = DateTime.Now,
                AktifMi = 1
            };
            if (item.TipId == 4)
            {
                entity.Koordinat = item.Koordinat;
            }
            if (item.UstId == 0)
                entity.BirimKisaTanim = Guid.NewGuid().ToString() + "level1";//Hiyerarşik yapıda en üstte bulunması için "level1" verilmiştir.
            else
            {
                var ustItem = base.SingleOrDefault(item.UstId).Value;
                int level = Convert.ToInt32(ustItem.BirimKisaTanim.Split("level")[1]);
                var group = ustItem.BirimKisaTanim.Split("level")[0];
                entity.BirimKisaTanim = group + "level" + (level + 1);
            }
            var result = base.Add(entity);
            return result.Value.TabloID.ToResult();
        }

        /// <summary>
        /// Kurum organizasyon biriminin adını değiştiren method
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public Result<bool> UpdateName(KurumOrganizasyonBirimView item)
        {
            if (item.TabloId == 0 || item.Tanim == null)
            {
                return Results.Fail("Güncelleme işleminiz başarısız!", ResultStatusCode.UpdateError);
            }
            var list = this.List(o => o.AktifMi == 1 && o.SilindiMi == 0 && o.TabloID != item.TabloId).Value;
            bool varMi = list.Any(o => o.BirimTanim == item.Tanim);
            if (varMi)
            {
                return Results.Fail("Aynı isimden kayıt bulunmaktadır.", ResultStatusCode.UpdateError);
            }
            var birim = this.SingleOrDefault(item.TabloId).Value;
            birim.BirimTanim = item.Tanim;
            this.Update(birim);

            return true.ToResult();
        }
    }
}