namespace BookDepositoryApi.Models
{
    public class ConcreteBook : Book
    {
        public int Id { get; set; } // Первичный ключ для базы данных

        // Конструктор без параметров для EF Core
        public ConcreteBook() { }

        // Конструктор с параметрами для создания объектов
        public ConcreteBook(string title, string author, string[] genres, int publicationYear, string annotation, string isbn)
            : base(title, author, genres, publicationYear, annotation, isbn)
        {
        }

        // Метод для обновления книги
        public void UpdateBook(string title, string author, string[] genres, int publicationYear, string annotation, string isbn)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("Title, Author, and ISBN cannot be empty.");
            }

            Title = title;
            Author = author;
            Genres = genres;
            PublicationYear = publicationYear;
            Annotation = annotation;
            ISBN = isbn;
        }
    }
}