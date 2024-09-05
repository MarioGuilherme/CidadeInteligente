using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteAreaById;

public class DeleteAreaByIdCommandHandler(IAreaRepository areaRepository) : IRequestHandler<DeleteAreaByIdCommand, Unit?> {
    private readonly IAreaRepository _areaRepository = areaRepository;

    public async Task<Unit?> Handle(DeleteAreaByIdCommand request, CancellationToken cancellationToken) {
        Area? area = await this._areaRepository.GetByIdAsync(request.AreaId);

        if (area is null) return null;

        await this._areaRepository.DeleteAreaAsync(area);

        return Unit.Value;
    }
}