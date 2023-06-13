namespace Laba_ISTP_2.Models;

public class LibraryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public List<BookDto>? Books { get; set; }
}
