using MovieApi.Entities;
using MovieApi.Responses.Genre;

namespace MovieApi.Mappers
{
    public class GenreMapper
    {
        public async Task<GenreResponse> ToDto(Genre genre)
        {
            return await Task.Run(() =>
            {
                return new GenreResponse
                {
                    Id = genre.Id,
                    Name = genre.Name,
                    CreatedAt = genre.CreatedAt?.ToString("dd MMM yyyy HH:mm:ss"),
                    UpdatedAt = genre.UpdatedAt?.ToString("dd MMM yyyy HH:mm:ss")
                };
            });
        }

        public async Task<List<GenreResponse>> ToDtos(List<Genre> genres)
        {
            return await Task.Run(() =>
            {
                var listGenreResponse = new List<GenreResponse>();
                foreach (var genre in genres)
                {
                    listGenreResponse.Add(ToDto(genre).Result);
                }
                return listGenreResponse;
            });
        }
    }
}