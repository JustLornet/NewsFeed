using Microsoft.EntityFrameworkCore;
using NewsFeedEngine.Controllers;
using NewsFeedEngine.DataAccess;
using NewsFeedEngine.Service;

namespace NewsFeedEngine.Tests
{
    public class NewsFeedControllerTests
    {
        private NewsFeedController _testingController;

        [SetUp]
        public void ControllerActivator()
        {
            var connectionString = Config.GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;

            var context = new Db(options);

            _testingController = new NewsFeedController(context);
        }

        [Test]
        public void GetOnePostForOneUser()
        {
            // act
            var post = _testingController.GetPostsByUserId(1, 1).Result;

            Assert.Multiple(() =>
            {
                Assert.That(post?.Count(), Is.EqualTo(1));
                Assert.That(post?.FirstOrDefault()?.Id, Is.EqualTo(1));
            });
        }

        [Test]
        public void GetAllPostsForOneUser()
        {
            // act
            var posts = _testingController.GetPostsByUserId(1).Result;

            // assert
            var expectingPostIds = new List<int> { 1, 2 };

            Assert.Multiple(() =>
            {
                Assert.That(posts?.Count(), Is.EqualTo(2));
                Assert.That(posts?.Select(p => p.Id), Is.EqualTo(expectingPostIds));
            });
        }

        [Test]
        public void GetOnePostForEachOfSeveralUsers()
        {
            // arrange
            var usersIds = new List<int> { 1, 2, 3 };

            // act
            var posts = _testingController.GetPostsByUserId(usersIds, 1).Result;

            // assert
            _ = posts!.TryGetValue(1, out var postIdFirstUser);
            _ = posts!.TryGetValue(2, out var postIdSecondUser);
            _ = posts!.TryGetValue(3, out var postIdThirdUser);

            Assert.Multiple(() =>
            {
                Assert.That(posts?.Count(), Is.EqualTo(3));
                // пользователь с id = 1
                Assert.That(postIdFirstUser!.Count(), Is.EqualTo(1));
                Assert.That(postIdFirstUser!.FirstOrDefault()!.Id, Is.EqualTo(1));
                // пользователь с id = 2
                Assert.That(postIdSecondUser!.Count(), Is.EqualTo(1));
                Assert.That(postIdSecondUser!.FirstOrDefault()!.Id, Is.EqualTo(3));
                // пользователь с id = 3
                Assert.That(postIdThirdUser!.Count(), Is.EqualTo(1));
                Assert.That(postIdThirdUser!.FirstOrDefault()!.Id, Is.EqualTo(4));
            });
        }

        [Test]
        public void GetAllPostsForEachOfSeveralUsers()
        {
            // arrange
            var usersIds = new List<int> { 1, 2 };

            // act
            var posts = _testingController.GetPostsByUserId(usersIds).Result;

            // assert
            _ = posts!.TryGetValue(1, out var postIdFirstUser);
            _ = posts!.TryGetValue(2, out var postIdSecondUser);

            Assert.Multiple(() =>
            {
                Assert.That(posts?.Count(), Is.EqualTo(2));
                // пользователь с id = 1
                Assert.That(postIdFirstUser!.Count(), Is.EqualTo(2));
                // пользователь с id = 2
                Assert.That(postIdSecondUser!.Count(), Is.EqualTo(1));
                Assert.That(postIdSecondUser!.FirstOrDefault()!.Id, Is.EqualTo(3));
            });
        }
    }
}
