namespace Laba_ISTP_2.Models;

public class LoanDto
{
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime ReturnDate { get; set; }
}
