using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateArea;

public class UpdateAreaCommandHandler(IAreaRepository areaRepository) : IRequestHandler<UpdateAreaCommand, Unit?> {
    private readonly IAreaRepository _areaRepository = areaRepository;

    public async Task<Unit?> Handle(UpdateAreaCommand request, CancellationToken cancellationToken) {
        Area? area = await this._areaRepository.GetByIdAsync(request.AreaId, true);

        if (area is null) return null;

        area.Update(request.Description);

        await this._areaRepository.SaveChangesAsync();
        return Unit.Value;
    }
}