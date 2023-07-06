using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsFeedEngine.DataAccess;
using NewsFeedEngine.Domain.Aggregates;

namespace NewsFeedEngine.Controllers
{
    public class NewsFeedController : Controller
    {
        private readonly Db _dbContext;

        public NewsFeedController(Db db)
        {
            _dbContext = db;
        }

        /// <summary>
        /// Получение постов из БД для конкретного пользователя
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="postsAmount">Кол-во постов к получению. По-умолчанию - все посты</param>
        /// <returns>Заданное кол-во постов конкретного пользователя</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts(int userId, int postsAmount = -1)
        {
            try
            {
                var posts = await this.GetPostsByUserId(userId, postsAmount);

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получение постов из БД для списка пользователнй
        /// </summary>
        /// <param name="usersIds">Id пользователей, чьи посты запрашиваются</param>
        /// <param name="postsAmount">Кол-во постов к получению. По-умолчанию - все посты</param>
        /// <returns>Заданное кол-во постов списка пользователей</returns>
        [HttpGet]
        public async Task<ActionResult<Dictionary<int, IEnumerable<Post>>>> GetPosts(IEnumerable<int> usersIds, int postsAmoun = -1)
        {
            try
            {
                var postsWithUsers = GetPostsByUserId(usersIds, postsAmoun);

                return Ok(postsWithUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        internal async Task<Dictionary<int, IEnumerable<Post>>> GetPostsByUserId(IEnumerable<int> usersIds, int postsAmoun = -1)
        {
            Dictionary<int, IEnumerable<Post>> postsWithUsers = new();

            foreach (var userId in usersIds)
            {
                var posts = await this.GetPostsByUserId(userId, postsAmoun);

                postsWithUsers.Add(userId, posts);
            }

            return postsWithUsers;
        }

        internal async Task<IEnumerable<Post>> GetPostsByUserId(int userId, int postsAmount = -1)
        {
            var user = await _dbContext.Users.Where(u => u.Id == userId).Include("Posts").FirstOrDefaultAsync();

            if (user is null) throw new KeyNotFoundException($"Пользователь с Id {userId} не был найден");

            var posts = user.Posts.OrderBy(p => p.CreateAt);
            var postsWithAmount = (postsAmount > 0 ? posts.Take(postsAmount) : posts).ToList();

            return postsWithAmount;
        }
    }
}