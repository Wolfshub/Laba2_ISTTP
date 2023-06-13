﻿namespace Laba_ISTP_2.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public int Year { get; set; }
    public int LibraryId { get; set; }
    public Library? Library { get; set; }
}
