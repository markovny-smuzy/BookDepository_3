using BookDepositoryApi.Interfaces;

namespace BookDepositoryApi.Models
{
    public class Book : IBook
    {
        public string Title { get; protected internal set; } = string.Empty;
        public string Author { get; protected internal set; } = string.Empty;
        public string GenresSerialized { get; protected internal set; } = string.Empty;
        public int PublicationYear { get; protected internal set; }
        public string Annotation { get; protected internal set; } = string.Empty;
        public string ISBN { get; protected internal set; } = string.Empty;

        public string[] Genres
        {
            get => GenresSerialized.Split(',', StringSplitOptions.RemoveEmptyEntries);
            set => GenresSerialized = string.Join(",", value);
        }

        // Конструктор без параметров для EF Core
        protected Book() { }

        // Конструктор для пользовательского создания объектов
        protected Book(string title, string author, string[] genres, int publicationYear, string annotation, string isbn)
        {
            Title = title;
            Author = author;
            Genres = genres;
            PublicationYear = publicationYear;
            Annotation = annotation;
            ISBN = isbn;
        }

        // Проверка ключевых слов
        public bool ContainsKeyword(string keyword)
        {
            keyword = keyword.ToLower();
            return Title.ToLower().Contains(keyword) ||
                   Author.ToLower().Contains(keyword) ||
                   Annotation.ToLower().Contains(keyword);
        }
    }
}