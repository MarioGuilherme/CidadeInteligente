using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Models;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;

public class GetRelatedProjectsFromUserQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetRelatedProjectsFromUserQuery, GetRelatedProjectsFromUserQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetRelatedProjectsFromUserQueryResult?> Handle(GetRelatedProjectsFromUserQuery request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.UserIdExistAsync(request.UserId))
        {
            Log.Warning("User with ID {UserId} not found.", request.UserId);
            _notification.AddNotification(NotificationType.UserNotFound);
            return null;
        }

        PaginationResult<Project> paginationResult = await _unitOfWork.Users.GetInvolvedAndCreatedProjectsFromUser(request.UserId, request.Page);

        if (!paginationResult.Data.Any())
            paginationResult = await _unitOfWork.Users.GetInvolvedAndCreatedProjectsFromUser(request.UserId, paginationResult.TotalPages);

        return new(
            paginationResult.CurrentPage,
            paginationResult.TotalPages,
            paginationResult.ItemsCount,
            [.. paginationResult.Data.Select(p => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel(p.ProjectId,
                p.Title,
                p.Description,
                [.. p.Medias.Select(m => new GetRelatedProjectsFromUserQueryResult.ProjectViewModel.MediaViewModel(m.MediaId, m.FileName))]))]
        );
    }
}