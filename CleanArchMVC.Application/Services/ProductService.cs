using AutoMapper;
using CleanArchMVC.Application.DTOs;
using CleanArchMVC.Application.Interfaces;
using CleanArchMVC.Application.Products.Queries;
using CleanArchMVC.Domain.Entities;
using CleanArchMVC.Domain.Interfaces;
using MediatR;

namespace CleanArchMVC.Application.Services;
public class ProductService : IProductService
{
    
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public ProductService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<IEnumerable<ProductDTO>> GetProducts()
    {
        var productsQuery = new GetProductsQuery();

        if (productsQuery == null)
            throw new Exception($"Entity coud not be loaded");

        var result = await _mediator.Send(productsQuery);

        return _mapper.Map<IEnumerable<ProductDTO>>(result);
    }

    //public async Task<ProductDTO> GetById(int? id)
    //{
    //    var productEntity = await _productRepository.GetByIdAsync(id);
    //    return _mapper.Map<ProductDTO>(productEntity);
    //}

    //public async Task<ProductDTO> GetProductCategory(int? id)
    //{
    //    var productEntity = await _productRepository.GetProductCategoryAsync(id);
    //    return _mapper.Map<ProductDTO>(productEntity);
    //}

    //public async Task Add(ProductDTO productDto)
    //{
    //    var productEntity = _mapper.Map<Product>(productDto);
    //    await _productRepository.CreateAsync(productEntity);
    //}
    //public async Task Update(ProductDTO productDto)
    //{
    //    var productEntity = _mapper.Map<Product>(productDto);
    //    await _productRepository.UpdateAsync(productEntity);
    //}
    //public async Task Remove(int? id)
    //{ 
    //        var productEntity = _productRepository.GetByIdAsync(id).Result;
    //        await _productRepository.RemoveAsync(productEntity);
    //}
}
