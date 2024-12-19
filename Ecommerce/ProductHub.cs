using Microsoft.AspNetCore.SignalR;

namespace Ecommerce
{
	public class ProductHub: Hub
	{
		public async Task UpdateStockQuantity(int productId, int newQuantity)
		{
			await Clients.All.SendAsync("ReceiveStockUpdate", productId, newQuantity);
		}
	}
}
