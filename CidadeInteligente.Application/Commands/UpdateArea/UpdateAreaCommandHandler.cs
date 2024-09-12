using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateArea;

public class UpdateAreaCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateAreaCommand, Unit> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdateAreaCommand request, CancellationToken cancellationToken) {
        Area area = await this._unitOfWork.Areas.GetByIdAsync(request.AreaId, true) ?? throw new AreaNotExistException();

        area.Update(request.Description);

        await this._unitOfWork.CompleteAsync();
        return Unit.Value;
    }
}