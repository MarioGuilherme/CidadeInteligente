using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateArea;

public class CreateAreaCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateAreaCommand, long> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<long> Handle(CreateAreaCommand request, CancellationToken cancellationToken) {
        Area area = new(request.Description);

        await this._unitOfWork.Areas.AddAsync(area);
        await this._unitOfWork.CompleteAsync();

        return area.AreaId;
    }
}