﻿using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateProject;

public class CreateProjectCommandHandler(IUnitOfWork unitOfWork, IFileStorage fileStorage) : IRequestHandler<CreateProjectCommand, long> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<long> Handle(CreateProjectCommand request, CancellationToken cancellationToken) {
        Project project = new(
            request.CreatorUserId,
            request.AreaId,
            request.CourseId,
            request.Title,
            request.Description,
            DateOnly.FromDateTime(DateTime.Now),
            request.StartedAt,
            request.FinishedAt
        );

        request.InvolvedUsers.ForEach(userId => project.InvolvedUsers.Add(new(userId)));
        request.Medias.ForEach(m => project.Medias.Add(new(
            m.Title,
            m.Description,
            this._fileStorage.UploadOrUpdateFileAsync($"{Guid.NewGuid():N}.{m.Extension}", m.Base64).Result,
            m.Size
        )));

        await this._unitOfWork.Projects.AddAsync(project);
        await this._unitOfWork.CompleteAsync();

        return project.ProjectId;
    }
}