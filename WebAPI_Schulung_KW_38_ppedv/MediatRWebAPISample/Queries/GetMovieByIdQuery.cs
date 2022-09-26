using MediatR;
using MediatRWebAPISample.Models;

namespace MediatRWebAPISample.Queries
{
    public record GetMovieByIdQuery(int Id) : IRequest<Movie>;
}
