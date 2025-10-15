using Baz.Model.Entity.ViewModel;
using Baz.ProcessResult;
using Baz.RequestManager.Abstracts;
using KisiServisiTest.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;

namespace KisiServisiTest
{
    /// <summary>
    /// Hedef kitle test classı
    /// </summary>
    [TestClass()]
    public class HedefKitleTest
    {
        private readonly IRequestHelper _helper;

        /// <summary>
        /// Hedef kitle test classı yapıcı metodu
        /// </summary>
        public HedefKitleTest()
        {
            _helper = TestServerRequestHelper.CreateHelper();
        }

        /// <summary>
        /// hedef kitle crud testi
        /// </summary>
        [TestMethod()]
        public void HedefKitleCrudTests()
        {
            //Unit testlerde kullanılmak üzere 1195 Id'li hedef kitle tanımlanmıştır.
            //Assert-1 RunExpressionTest
            var runExpressionTest = _helper.Get<Result<List<KeyValueModel>>>($"/api/HedefKitle/RunExpression/" + 1195);
            Assert.AreEqual(runExpressionTest.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(runExpressionTest.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(runExpressionTest.Result);

            //Assert-2
            var getFields = _helper.Get<Result<List<HedefKitleField>>>($"/api/HedefKitle/GetFields/" + 1195);
            Assert.AreEqual(getFields.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(getFields.Result);

            //TargetGroupMembersListByGroupId
            var TargetGroupMembersListByGroupId = _helper.Get<Result<List<BasicKisiModel>>>($"/api/HedefKitle/TargetGroupMembersListByGroupId/" + 1195);
            Assert.AreEqual(TargetGroupMembersListByGroupId.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(TargetGroupMembersListByGroupId.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(TargetGroupMembersListByGroupId.Result);

            //RunExpressionReturnUser
            var RunExpressionReturnUser = _helper.Get<Result<List<KeyValueModel>>>($"/api/HedefKitle/RunExpressionReturnUser/" + 1195);
            Assert.AreEqual(RunExpressionReturnUser.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(RunExpressionReturnUser.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(RunExpressionReturnUser.Result);

            //Unit testlerde kullanılmak üzere 2200 Id'li hedef kitle tanımlanmıştır.
            //Assert-1 RunExpressionTest
            var runExpressionTest2 = _helper.Get<Result<List<KeyValueModel>>>($"/api/HedefKitle/RunExpression/" + 2200);
            Assert.AreEqual(runExpressionTest2.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(runExpressionTest2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(runExpressionTest2.Result);

            //Assert-2
            var getFields2 = _helper.Get<Result<List<HedefKitleField>>>($"/api/HedefKitle/GetFields/" + 2200);
            Assert.AreEqual(getFields2.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(getFields2.Result);

            //TargetGroupMembersListByGroupId
            var TargetGroupMembersListByGroupId2 = _helper.Get<Result<List<BasicKisiModel>>>($"/api/HedefKitle/TargetGroupMembersListByGroupId/" + 2200);
            Assert.AreEqual(TargetGroupMembersListByGroupId2.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(TargetGroupMembersListByGroupId2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(TargetGroupMembersListByGroupId2.Result);

            //RunExpressionReturnUser
            var RunExpressionReturnUser2 = _helper.Get<Result<List<KeyValueModel>>>($"/api/HedefKitle/RunExpressionReturnUser/" + 2200);
            Assert.AreEqual(RunExpressionReturnUser2.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(RunExpressionReturnUser2.Result.StatusCode, (int)ResultStatusCode.Success);
            Assert.IsNotNull(RunExpressionReturnUser2.Result);
        }
    }
}