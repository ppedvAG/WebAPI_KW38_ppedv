using MediatR;
using MediatRWebAPISample.Models;

namespace MediatRWebAPISample.Queries
{
    public class GetMoviesQuery : IRequest<IEnumerable<Movie>>
    {

    }
}
