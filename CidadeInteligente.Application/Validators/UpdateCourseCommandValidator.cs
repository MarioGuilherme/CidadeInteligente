﻿using CidadeInteligente.Application.Commands.UpdateCourse;
using FluentValidation;

namespace CidadeInteligente.Application.Validators;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand> {
    public UpdateCourseCommandValidator() {
        this.RuleFor(c => c.Description)
            .NotNull().WithMessage("É necessário informar a descrição do curso!")
            .NotEmpty().WithMessage("A descrição do curso não pode estar em branco!")
            .MaximumLength(45).WithMessage("A descrição do curso não pode exceder 45 caracteres!");
    }
}