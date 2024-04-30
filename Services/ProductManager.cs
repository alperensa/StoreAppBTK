using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
	public class ProductManager : IProductService
	{

		private readonly IRepositoryManager _manager;
		private readonly IMapper _mapper;
        public ProductManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public void CreateOneProduct(ProductDtoForInsertion productDto)
		{	
			// without using AutoMapper

			/* Product product = new Product(){
				ProductName = productDto.ProductName,
				Price = productDto.Price,
				CategoryId = productDto.CategoryId,
			}; */
			// using AutoMapper
			Product product = _mapper.Map<Product>(productDto);
			_manager.Product.Create(product);
			_manager.Save();
		}

		public void DeleteOneProduct(int id)
		{
			Product product = GetOneProduct(id, false);
			if (product != null)
			{
				_manager.Product.DeleteOneProduct(product);
				_manager.Save();
			}
		}

		public IEnumerable<Product> GetAllProducts(bool trackChanges)
		{
			return _manager.Product.GetAllProducts(trackChanges);
		}

		public Product? GetOneProduct(int id, bool trackChanges)
		{
			var product = _manager.Product.GetOneProduct(id, trackChanges);
			if (product is not null)
				return product;
			throw new Exception("Product not found!");
		}

		public void UpdateOneProduct(Product product)
		{
			var entity = _manager.Product.GetOneProduct(product.ProductId, true);
			entity.ProductName = product.ProductName;
			entity.Price = product.Price;
			_manager.Save();
		}
	}
}