﻿using Microsoft.AspNetCore.Mvc;
using Orders.Data.Models;
using System.Threading.Tasks;

namespace Orders.App.Services
{
	public interface IInvoicesService
	{

		public Task<bool> PostOrder(Order order, string email);

	}
}