using FluentValidation;
using MiniLibrary.Application.DTOs;
namespace MiniLibrary.Application.Validators;
public class  CreateLoanRequestValidator : AbstractValidator<CreateLoanRequest>{
    public CreateLoanRequestValidator()
    {
       
        RuleFor(x => x.BookId).GreaterThan(0).WithMessage("BookId is invalid.");
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId is invalid.");
    }
}