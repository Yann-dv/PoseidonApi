using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PoseidonApi.Models;

namespace PoseidonApiTests;

public abstract class ApiTests
{
    private static readonly ApiDbContext TestDbContext = Setup.FakeDbContext();
    

    [TestFixture]
    public class AuthControllerTests
    {
        [Test]
        public void Should_return_ok_status_when_expected_authorized_user_login()
        {
            var authController = new PoseidonApi.Controllers.AuthController();

            var result = authController.Login(new LoginModel()
            {
                UserName = "johndoe",
                Password = "John_doe99$"
            });

            Assert.That(result.GetType(), Is.EqualTo(typeof(OkObjectResult)));
        }

        [Test]
        public void Should_return_unauthorized_status_when_unexpected_user_try_to_login()
        {
            var authController = new PoseidonApi.Controllers.AuthController();

            var result = authController.Login(new LoginModel()
            {
                UserName = "randomGuy",
                Password = "random_pwd*102"
            });

            Assert.That(result.GetType(), Is.EqualTo(typeof(UnauthorizedResult)));
        }
    }

    [TestFixture]
    public class BidControllerTests
    {
        [Test]
        public void Should_return_all_bids_when_read_action()
        {

            var bidController = new PoseidonApi.Controllers.BidController(TestDbContext);

            var getAllBids = bidController.GetBids();

            Assert.That(getAllBids.Result.Value!.Count(), Is.EqualTo(2));
        }

        [TestCase(1)]
        [TestCase(13748)]
        [Test]
        public void Should_return_expected_bid_when_get_bid_by_id(int id)
        {
            var bidController = new PoseidonApi.Controllers.BidController(TestDbContext);

            var getBidById = bidController.GetBid(id);

            Assert.That(getBidById.Result.Value!.Id, Is.EqualTo(id));
        }

        [Test]
        public void Should_return_expected_newly_created_bid_when_post()
        {
            var bidController = new PoseidonApi.Controllers.BidController(TestDbContext);

            var bidToAdd = bidController.PostBid(
                new BidDTO()
                {
                    Status = "Testing",
                    BidListDate = DateTime.Now,
                    Account = "TestAcocount",
                    Type = "TestType2",
                    BidQuantity = 1,
                    AskQuantity = 1,
                    BidValue = 1,
                    Ask = 1,
                    Benchmark = "TestBenchmark2",
                    Commentary = "TestCommentary2",
                    Security = "TestSecurity2",
                    Trader = "TestTrader2",
                    Book = "Book2",
                    CreationName = "TestCreationName2",
                    CreationDate = DateTime.Now,
                    RevisionName = "TestRevisionName2",
                    RevisionDate = DateTime.Now,
                    DealName = "TestDealName2",
                    DealType = "TestDealType2",
                    SourceListId = "TestSourceListId2",
                    Side = "TestSide2"
                });

            Assert.That(bidToAdd.Result.Result!.GetType(), Is.EqualTo(typeof(CreatedAtActionResult)));
            Assert.That(TestDbContext.Bids.Count(), Is.EqualTo(3));
            Assert.That(TestDbContext.Bids.ToList()[2].Id, Is.EqualTo(13749));
        }

        [Test]
        public void Should_return_expected_updated_bid_when_put()
        {
            var bidController = new PoseidonApi.Controllers.BidController(TestDbContext);

            var bidToUpdate = bidController.PutBid(id: 1,
                new BidDTO()
                {
                    Id = 1,
                    Status = "Updated",
                    BidListDate = DateTime.Now,
                    Account = "UpdatedAccount1",
                    Type = "UpdatedType1",
                    BidQuantity = 1,
                    AskQuantity = 1,
                    BidValue = 1,
                    Ask = 1,
                    Benchmark = "UpdatedBenchmark1",
                    Commentary = "UpdatedCommentary1",
                    Security = "UpdatedSecurity1",
                    Trader = "UpdatedTrader1",
                    Book = "UpdatedBook1",
                    CreationName = "UpdatedCreationName1",
                    CreationDate = DateTime.Now,
                    RevisionName = "UpdatedRevisionName1",
                    RevisionDate = DateTime.Now,
                    DealName = "UpdatedDealName1",
                    DealType = "UpdatedDealType1",
                    SourceListId = "UpdatedSourceListId1",
                    Side = "UpdatedSide1"
                }, TestDbContext);

            Assert.That(bidToUpdate.Result.GetType(), Is.EqualTo(typeof(NoContentResult)));
            Assert.That(TestDbContext.Bids.Count(), Is.EqualTo(2)); //Ensure no POST
            Assert.That(TestDbContext.Bids.ToList().Find(c => c.Id == 1).Status, Is.EqualTo("Updated"));
        }

        [Test]
        public void Should_return_expected_deleted_bid_when_delete()
        {
            var bidController = new PoseidonApi.Controllers.BidController(TestDbContext);

            var bidToDelete = bidController.DeleteBid(1);

            Assert.That(bidToDelete.Result.GetType(), Is.EqualTo(typeof(NoContentResult)));
            Assert.That(TestDbContext.Bids.Count(), Is.EqualTo(1));
        }

    }

    [TestFixture]
    public class CurvePointControllerTests
    {
        
        [Test]
        public void Should_return_all_curve_points_when_read_action()
        {
            var curvePointController = new PoseidonApi.Controllers.CurvePointController(TestDbContext);

            var getAllCurvePoints = curvePointController.GetCurvePoints();

            Assert.That(getAllCurvePoints.Result.Value!.Count(), Is.EqualTo(2));
        }

        [TestCase(1)]
        [TestCase(12)]
        [Test]
        public void Should_return_expected_curve_point_when_get_curve_point_by_id(int id)
        {
            var curvePointController = new PoseidonApi.Controllers.CurvePointController(TestDbContext);

            var getCurvePointById = curvePointController.GetCurvePoint(id);

            Assert.That(getCurvePointById.Result.Value!.Id, Is.EqualTo(id));
        }

        [Test]
        public void Should_return_expected_newly_created_curve_point_when_post()
        {
            var curvePointController = new PoseidonApi.Controllers.CurvePointController(TestDbContext);

            var curvePointToAdd = curvePointController.PostCurvePoint(
                new CurvePointDTO()
                {
                    CurveId = 3,
                    Term = 3,
                    Value = 3,
                    CreationDate = DateTime.Now
                });

            Assert.That(curvePointToAdd.Result.Result!.GetType(), Is.EqualTo(typeof(CreatedAtActionResult)));
            Assert.That(TestDbContext.CurvePoints.Count(), Is.EqualTo(3));
            Assert.That(TestDbContext.CurvePoints.ToList()[2].Id, Is.EqualTo(13));
        }

        [Test]
        public void Should_return_expected_updated_curve_point_when_put()
        {
            var curvePointController = new PoseidonApi.Controllers.CurvePointController(TestDbContext);

            var curvePointToUpdate = curvePointController.PutCurvePoint(id: 1,
                new CurvePointDTO()
                {
                    Id = 1,
                    CurveId = 999,
                    Term = 1,
                    Value = 1,
                    CreationDate = DateTime.Now
                }, TestDbContext);

            Assert.That(curvePointToUpdate.Result.GetType(), Is.EqualTo(typeof(NoContentResult)));
            Assert.That(TestDbContext.CurvePoints.Count(), Is.EqualTo(2)); //Ensure no POST
            Assert.That(TestDbContext.CurvePoints.ToList().Find(c => c.Id == 1).CurveId, Is.EqualTo(999));
        }

        [Test]
        public void Should_return_expected_deleted_curve_point_when_delete()
        {
            var curvePointController = new PoseidonApi.Controllers.CurvePointController(TestDbContext);

            var curvePointToDelete = curvePointController.DeleteCurvePoint(1);

            Assert.That(curvePointToDelete.Result.GetType(), Is.EqualTo(typeof(NoContentResult)));
            Assert.That(TestDbContext.CurvePoints.Count(), Is.EqualTo(1));
        }
    }
}