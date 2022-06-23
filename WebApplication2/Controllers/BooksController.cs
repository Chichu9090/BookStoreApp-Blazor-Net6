using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models.Book;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDBContext _context;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BooksController(BookStoreDBContext context, IMapper mapper,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDTO>>> GetBooks()
        {
            var books = await _context.Books
                .Include(q => q.Author)
                .ProjectTo<BookReadOnlyDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
            //var bookDTO = mapper.Map< IEnumerable<BookReadOnlyDTO>>(books);   
            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDTO>> GetBook(int id)
        {
          var book = await _context.Books
                .Include(q =>q.Author)  
                .ProjectTo<BookDetailsDTO>(mapper.ConfigurationProvider)    
                .FirstOrDefaultAsync(q => q.Id == id); 

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDTO bookUpdateDTO)
        {
            if (id != bookUpdateDTO.Id)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null) return NotFound();

            if(string.IsNullOrEmpty(bookUpdateDTO.ImageData) == false)
            {
                bookUpdateDTO.Image = CreateFile(bookUpdateDTO.ImageData,bookUpdateDTO.OriginalImageName);

                var picName = Path.GetFileName(book.Image);
                var path = $"{webHostEnvironment.WebRootPath}\\bookcoverimages\\{picName}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

            }

            mapper.Map(bookUpdateDTO, book);

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<BookCreateDTO>> PostBook(BookCreateDTO bookCreateDTO)
        {
          if (_context.Books == null)
          {
              return Problem("Entity set 'BookStoreDBContext.Books'  is null.");
          }

           
            var book = mapper.Map<Book>(bookCreateDTO);  
            book.Image = CreateFile(bookCreateDTO.ImageData, bookCreateDTO.OriginalImageName);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string CreateFile(string imageBase64, string imageName)
        {
            var url = HttpContext.Request.Host.Value;
            var ext = Path.GetExtension(imageName);
            var fileName = $"{Guid.NewGuid()}{ext}";

            var path = $"{webHostEnvironment.WebRootPath}\\bookcoverimages\\{fileName}";

            byte[] image = Convert.FromBase64String(imageBase64);

            var fileStream = System.IO.File.Create(path);
            fileStream.Write(image, 0, image.Length);
            fileStream.Close();

            return $"https://{url}/bookcoverimages/{fileName}";
        }
    }
}
