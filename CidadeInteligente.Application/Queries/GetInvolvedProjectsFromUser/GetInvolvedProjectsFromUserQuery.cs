﻿using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Models;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetInvolvedProjectsFromUser;

public class GetInvolvedProjectsFromUserQuery(long userId, int page) : IRequest<PaginationResult<ProjectViewModel>> {
    public long UserId { get; private set; } = userId;
    public int Page { get; private set; } = page;
}