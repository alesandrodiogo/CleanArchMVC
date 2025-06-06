﻿using CleanArchMVC.Domain.Entities;
using FluentAssertions;
using System.Diagnostics;
using System.Xml.Linq;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace CleanArchMVC.Domain.Tests;
public class ProductUnitTest1
{
    [Fact]
    public void CreateProduct_WithValidParameters_ResultObjectValidState()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.99m,
            99, "Product Image");
        action.Should().NotThrow<CleanArchMVC.Domain.Validation.DomainExceptionValidation>();
    }
    [Fact]
    public void CreateProduct_NegativeIdValue_DomainExceptionInvalidI()
    {
        Action action = () => new Product(-1, "Product Name", "Product Description", 9.99m,
            99, "Product Image");
        action.Should().Throw<CleanArchMVC.Domain.Validation.DomainExceptionValidation>()
            .WithMessage("Invalid Id value");
    }
    [Fact]
    public void CreateProduct_ShortNameValue_DomainExceptionShortName()
    {
        Action action = () => new Product(1, "Pr", "Product Description", 9.99m, 99, "Product Image");
        action.Should()
            .Throw<CleanArchMVC.Domain.Validation.DomainExceptionValidation>()
               .WithMessage("Invalid name, too short, minimum 3 characteres.");
    }
    [Fact]
    public void CreateProduct_LongImageName_DomainExceptionLongImageName()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.99m,
            99, "Product Image tooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo" +
            "loooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooonggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");
        action.Should()
            .Throw<CleanArchMVC.Domain.Validation.DomainExceptionValidation>()
            .WithMessage("Invalid image name, too long, maximum 250 characteres.");
    }
    [Fact]
    public void CreateProduct_WithNullImageName_NoDomainException()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 99, null);
        action.Should()
            .NotThrow<CleanArchMVC.Domain.Validation.DomainExceptionValidation>();
    }
    [Fact]
    public void CreateProduct_WithNullImageName_NoNullReferenceException()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 99, null);
        action.Should()
            .NotThrow<NullReferenceException>();
    }
    [Fact]
    public void CreateProduct_WithEmptyImageName_NoDomainException()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.99m, 99, "");
        action.Should()
            .NotThrow<CleanArchMVC.Domain.Validation.DomainExceptionValidation>();
    }
    [Fact]
    public void CreateProduct_InvalidPriceValue_DomainException()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", -9.99m, 99, "");
        action.Should()
            .Throw<CleanArchMVC.Domain.Validation.DomainExceptionValidation>()
            .WithMessage("Invalid price value.");
    }
    [Theory]
    [InlineData(-5)]
    public void CreateProduct_InvalidStockValue_ExceptionDomainNegativeValue(int value)
    {
        Action action = () => new Product(1, "Pro", "Product Description", 9.99m,
            value, "Product Image");
        action.Should()
            .Throw<CleanArchMVC.Domain.Validation.DomainExceptionValidation>()
            .WithMessage("Invalid stock value.");
    }
}
