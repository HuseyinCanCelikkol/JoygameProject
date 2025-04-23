using FluentValidation;
using JoygameProject.Application.Features.Commands.Product.Update;

namespace JoygameProject.Application.Validators.Product
{
    public class ProductUpdateCommandValidator : AbstractValidator<ProductUpdateCommandRequest>
    {
        public ProductUpdateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz");
            RuleFor(x => x.CatId)
                .NotEmpty().WithMessage("Kategori boş olamaz.");
            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Resim boş olamaz");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat sıfırdan büyük olmalı");
        }
    }
}
