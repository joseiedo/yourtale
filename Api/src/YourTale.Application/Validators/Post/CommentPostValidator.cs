using FluentValidation;
using YourTale.Application.Contracts.Documents.Responses.Post;

namespace YourTale.Application.Validators.Post;

public class CommentPostValidator : AbstractValidator<CommentPostRequest>
{
    public CommentPostValidator()
    {
        RuleFor(x => x.PostId).NotNull().GreaterThan(0);
        RuleFor(x => x.Text).NotNull().MaximumLength(150);
    }
}