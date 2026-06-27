using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreaById;

public class GetAreaByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAreaByIdQuery, GetAreaByIdQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetAreaByIdQueryResult> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken)
    {
        Area area = await _unitOfWork.Areas.GetByIdAsync(request.AreaId) ?? throw new AreaNotExistException();
        return new(area.AreaId, area.Description);
    }
}
