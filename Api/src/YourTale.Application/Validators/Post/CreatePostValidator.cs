using FluentValidation;
using YourTale.Application.Contracts.Documents.Requests.Post;

namespace YourTale.Application.Validators.Post;

public class CreatePostValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.Description).NotNull().MaximumLength(255);
        RuleFor(x => x.Picture).NotNull().MaximumLength(512);
    }
}