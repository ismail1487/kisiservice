/*
using Baz.Model.Entity;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Baz.Repository.Pattern;
*/
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
    /// Yetki Merkezi işlemlerini gerçekleştiren methodları barındıran interface.
    /// </summary>
    public interface IYetkiMerkeziService : IService<ErisimYetkilendirmeTanimlari>
    {

        /// <summary>
        /// Erişim yetkilendirme tanımlarını kaydeden method.
        /// </summary>
        /// <param name="list"> kaydedilecek erişim yetkilendirme tanımları listesi</param>
        /// <returns>kaydedilen verileri döndürür.</returns>
        public Result<List<ErisimYetkilendirmeTanimlari>> ErisimYetkilendirmeTanimlariKaydet(
            List<ErisimYetkilendirmeTanimlari> list);
    }

    /// <summary>
    /// Yetki Merkezi işlemlerini gerçekleştiren methodları barındıran class.
    /// </summary>
    
    public class YetkiMerkeziService : Service<ErisimYetkilendirmeTanimlari> ,IYetkiMerkeziService
    {
        private readonly IErisimYetkilendirmeTanimlariService _erisimYetkilendirmeTanimlariService;
        private readonly ILoginUser _loginUser;


        /// <summary>
        /// Yetki Merkezi işlemlerini gerçekleştiren methodları barındıran classın yapıcı metodu
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dataMapper"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="erisimYetkilendirmeTanimlariService"></param>
        /// <param name="loginUser"></param>
        public YetkiMerkeziService(IRepository<ErisimYetkilendirmeTanimlari> repository, IDataMapper dataMapper, IServiceProvider serviceProvider, ILogger<YetkiMerkeziService> logger, IErisimYetkilendirmeTanimlariService erisimYetkilendirmeTanimlariService, ILoginUser loginUser) : base(repository, dataMapper, serviceProvider, logger)
        {
            _erisimYetkilendirmeTanimlariService = erisimYetkilendirmeTanimlariService;
            
            _loginUser = loginUser;
        }


        /// <summary>
        /// Erişim yetkilendirme tanımlarını kaydeden method.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Result<List<ErisimYetkilendirmeTanimlari>> ErisimYetkilendirmeTanimlariKaydet(List<ErisimYetkilendirmeTanimlari> list)
        {
            if (list.Count == 0)
            {
                return Results.Fail("Kaydetme işleminiz gerçekleşmemiştir!", ResultStatusCode.CreateError);
            }
            var result1 = _erisimYetkilendirmeTanimlariService.ErisimYetkilendirmeTanimlariListesi();

            bool benzerKayitVarMi = false;
            var returnList = new List<ErisimYetkilendirmeTanimlari>();
            foreach (var item in list)
            {
                var varMi = result1.Any(x =>
                    x.ErisimYetkisiVerilenSayfaId == item.ErisimYetkisiVerilenSayfaId &&
                    x.IlgiliKurumOrganizasyonBirimTanimiId == item.IlgiliKurumOrganizasyonBirimTanimiId);

                if (varMi)
                {
                    benzerKayitVarMi = true;
                }
                else
                {
                    var result = _erisimYetkilendirmeTanimlariService.Add(item);
                    returnList.Add(result.Value);
                }
            }

            if (benzerKayitVarMi)
            {
                if (list.Count == 1)
                {
                    return returnList.ToResult().WithSuccess(new Success("Benzer kayıt mevcuttur. İşleminiz gerçekleştirilemedi."));
                }
                return returnList.ToResult().WithSuccess(new Success("Benzer kayıtlar mevcuttur. Haricindekiler kayıt edilmiştir."));
            }

            return returnList.ToResult().WithSuccess(new Success("Kayıt başarılı."));
        }
    }
}