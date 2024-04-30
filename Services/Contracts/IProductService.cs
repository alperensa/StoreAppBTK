﻿using System.Xml.Serialization;
using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts(bool trackChanges);
        Product? GetOneProduct(int id, bool trackChanges);
        void CreateOneProduct(ProductDtoForInsertion productDto);
		void UpdateOneProduct(Product product);
		void DeleteOneProduct(int id);
	}

}