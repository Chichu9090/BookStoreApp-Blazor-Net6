using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models.Author;
using WebApplication2.Static;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDBContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(BookStoreDBContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDTO>>> GetAuthors()
        {
            try
            {
                var author = await _context.Authors.ToListAsync();
                var authorsDTO = mapper.Map<IEnumerable<AuthorReadOnlyDTO>>(author);
                return Ok(author);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error performing GET in {nameof(GetAuthors)}");
                return StatusCode(500,Messages.Error505Message);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDTO>> GetAuthor(int id)
        {

            try
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    return NotFound();
                }


                return Ok(mapper.Map<AuthorReadOnlyDTO>(author));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error performing GET in {nameof(GetAuthor)}");
                return StatusCode(500, Messages.Error505Message);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDTO authorUpdateDTO)
        {

            try
            {
                if (id != authorUpdateDTO.Id)
                {
                    return BadRequest();
                }

                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    return NotFound();
                }

                mapper.Map(authorUpdateDTO, author);

                _context.Entry(author).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error performing GET in {nameof(PutAuthor)}");
                return StatusCode(500, Messages.Error505Message);
            }
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<AuthorCreateDTO>> PostAuthor(AuthorCreateDTO authorCreateDTO)
        {
            try
            {
                var author = mapper.Map<Author>(authorCreateDTO);
                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error performing GET in {nameof(PostAuthor)}");
                return StatusCode(500, Messages.Error505Message);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                if (_context.Authors == null)
                {
                    return NotFound();
                }
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error performing GET in {nameof(DeleteAuthor)}");
                return StatusCode(500, Messages.Error505Message);
            }
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
