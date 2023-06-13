using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Laba_ISTP_2.Context;
using Laba_ISTP_2.Models;

namespace Laba_ISTP_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public LoansController(ProjectDbContext context)
        {
            _context = context;
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {

            var loans = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .ToListAsync();

            var loanDTOs = loans.Select(loan => new LoanDto
            {
                BookId = loan.BookId,
                MemberId = loan.MemberId,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate
            });

            return Ok(loanDTOs);
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            if (_context.Loans == null)
            {
                return NotFound();
            }
            var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            return loan;
        }

        // PUT: api/Loans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(int id, Loan loan)
        {
            if (id != loan.Id)
            {
                return BadRequest();
            }

            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
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


        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] LoanDto loanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _context.Books.FindAsync(loanDto.BookId);
            if (book == null)
            {
                return BadRequest("Invalid BookId");
            }

            var member = await _context.Members.FindAsync(loanDto.MemberId);
            if (member == null)
            {
                return BadRequest("Invalid MemberId");
            }

            var loan = new Loan
            {
                BookId = loanDto.BookId,
                MemberId = loanDto.MemberId,
                LoanDate = DateTime.UtcNow,
                ReturnDate = loanDto.ReturnDate,
                Book = book,
                Member = member
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            if (_context.Loans == null)
            {
                return NotFound();
            }
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanExists(int id)
        {
            return (_context.Loans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
