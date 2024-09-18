using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreaById;

public class GetAreaByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAreaByIdQuery, AreaViewModel> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<AreaViewModel> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken) {
        Area area = await this._unitOfWork.Areas.GetByIdAsync(request.AreaId) ?? throw new AreaNotExistException();
        return this._mapper.Map<AreaViewModel>(area);
    }
}