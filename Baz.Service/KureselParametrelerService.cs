using Baz.Model.Entity.Constants;
using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.RequestManager.Abstracts;
using System;
using System.Net;

namespace Baz.Service
{
    /// <summary>
    /// Küresel Parametreler için oluşturulan methodların yer aldığı sınıftır.
    /// </summary>
    public interface IKureselParametrelerService
    {
        /// <summary>
        /// Zorunlu şifere yenileme aralığı parametresini getiren servis methodudur.
        /// </summary>
        /// <param name="paramTanim">The parameter tanim.</param>
        /// <returns></returns>
        Result<KureselParametreModel> ZorunluSifreYenilemeAraligiGetir(string paramTanim = "ZorunluŞifreYenilemeAralığı");
    }

    /// <summary>
    /// Küresel Parametreler için oluşturulan methodların yer aldığı sınıftır.
    /// </summary>
    /// <seealso cref="IKureselParametrelerService" />
    public class KureselParametrelerService : IKureselParametrelerService
    {
        private readonly IRequestHelper _requestHelper;

        /// <summary>
        ///  Küresel Parametreler için oluşturulan methodların yer aldığı sınıfının yapıcı metodu
        /// </summary>
        /// <param name="requestHelper"></param>
        public KureselParametrelerService(IRequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
        }

        /// <summary>
        /// Zorunlu şifere yenileme aralığı parametresini getiren servis methodudur.
        /// </summary>
        /// <param name="paramTanim">The parameter tanim.</param>
        /// <returns></returns>
        public Result<KureselParametreModel> ZorunluSifreYenilemeAraligiGetir(string paramTanim = "ZorunluŞifreYenilemeAralığı")
        {
            var result = _requestHelper.Post<Result<KureselParametreModel>>(LocalPortlar.IYSService + "/api/KureselParametreler/IsmeGoreParamGetir", paramTanim);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return result.Result;
            }
            else return Results.Fail("API bağlantısında bir sorun yaşandı.");
        }
    }
}