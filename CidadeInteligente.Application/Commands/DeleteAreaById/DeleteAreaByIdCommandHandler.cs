using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteAreaById;

public class DeleteAreaByIdCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteAreaByIdCommand, Unit> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteAreaByIdCommand request, CancellationToken cancellationToken) {
        Area area = await this._unitOfWork.Areas.GetByIdAsync(request.AreaId) ?? throw new AreaNotExistException();

        if (await this._unitOfWork.Areas.HaveProjectsAsync(request.AreaId))
            throw new AreaWithDepedentProjectsException();

        this._unitOfWork.Areas.Delete(area);
        await this._unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}