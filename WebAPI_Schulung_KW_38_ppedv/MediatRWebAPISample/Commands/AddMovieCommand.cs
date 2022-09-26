using MediatR;
using MediatRWebAPISample.Models;

namespace MediatRWebAPISample.Commands
{
    public record AddMovieCommand(Movie Movie) : IRequest<Movie>;
}
