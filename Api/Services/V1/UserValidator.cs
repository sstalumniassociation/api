using FluentValidation;
using User.V1;

namespace Api.Services.V1;

public class BindUserValidator : AbstractValidator<BindUserRequest>
{
    public BindUserValidator()
    {
        
    }
}