using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateArea;

public class CreateAreaCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateAreaCommand, int?>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int?> Handle(CreateAreaCommand request, CancellationToken cancellationToken)
    {
        Area area = new(request.Description);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        await _unitOfWork.Areas.AddAsync(area);
        await _unitOfWork.CommitAsync(cancellationToken);

        return area.AreaId;
    }
}
