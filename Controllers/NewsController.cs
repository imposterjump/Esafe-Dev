using BankProject.Data;
using BankProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public NewsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetNews")]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            if (_dbContext.News == null)
            {
                return NotFound();
            }
            return await _dbContext.News.ToListAsync();
        }

        [HttpGet("GetNewsby{ID}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            if (_dbContext.News == null)
            {
                return NotFound();
            }
            var news = await _dbContext.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();

            }
            return news;

        }


        [HttpPost("AddNews")]
        public async Task<ActionResult<News>> AddNews(News news)
        {
            _dbContext.News.Add(news);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNews), new { id = news.Id }, news);

        }

        [HttpPut("UpdateNews")]
        public async Task<ActionResult<News>> UpdateNews(News news, int id)
        {
            if (id != news.Id)
            {
                return BadRequest();
            }
            _dbContext.Entry(news).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsAvail(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok();

        }


        private bool NewsAvail(int id)
        {
            return (_dbContext.News?.Any(x => x.Id == id)).GetValueOrDefault();
        }


        [HttpDelete("DeleteNewsby{ID}")]
        public async Task<ActionResult<News>> DeleteNews(int id)
        {
            if (_dbContext.News == null)
            {
                return NotFound();
            }

            var news = await _dbContext.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            _dbContext.News.Remove(news);

            await _dbContext.SaveChangesAsync();
            return Ok();

        }


    }

}
