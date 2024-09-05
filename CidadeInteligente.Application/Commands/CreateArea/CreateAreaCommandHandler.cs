using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateArea;

public class CreateAreaCommandHandler(IAreaRepository areaRepository) : IRequestHandler<CreateAreaCommand, long> {
    private readonly IAreaRepository _areaRepository = areaRepository;

    public async Task<long> Handle(CreateAreaCommand request, CancellationToken cancellationToken) {
        Area area = new(request.Description);

        await this._areaRepository.AddAsync(area);

        return area.AreaId;
    }
}