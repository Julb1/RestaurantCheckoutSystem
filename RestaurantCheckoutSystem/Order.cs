using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCheckoutSystem
{
	public class PricingOptions
	{
		public Dictionary<ItemType, decimal> Prices { get; set; } = new();
		public DrinkDiscountOptions DrinkDiscount { get; set; } = new();

		public decimal ServiceCharge { get; set; }
	}

	public class DrinkDiscountOptions
	{
		public decimal Percentage { get; set; }
		public TimeSpan CutoffTime { get; set; }
	}

	public class Order
	{
		private readonly List<OrderItem> _items = new();

		private readonly Dictionary<ItemType, decimal> _prices;
		private readonly TimeSpan _discountCutoff;
		private readonly decimal _drinkDiscount;
		private readonly decimal _serviceCharge;

		public Order(PricingOptions pricing)
		{
			_prices = pricing.Prices;
			_discountCutoff = pricing.DrinkDiscount.CutoffTime;
			_drinkDiscount = pricing.DrinkDiscount.Percentage;
			_serviceCharge = pricing.ServiceCharge;
		}

		public void AddItem(ItemType type, DateTime orderedAt)
		{
			_items.Add(new OrderItem(type, orderedAt));
		}

		public void CancelSpecificItems(
			ItemType type,
			int count,
			DateTime orderedAt)
				{
					var itemsToRemove = _items
						.Where(i => i.Type == type && i.OrderedAt == orderedAt)
						.Take(count)
						.ToList();

					foreach (var item in itemsToRemove)
					{
						_items.Remove(item);
					}
		}

		public decimal CalculateTotal()
		{
			decimal total = 0;

			foreach (var item in _items)
			{
				var price = _prices[item.Type];

				if (item.Type == ItemType.Drink &&
					item.OrderedAt.TimeOfDay < _discountCutoff)
				{
					price -= price * _drinkDiscount;
				}

				total += price;
			}

			total -= total * _serviceCharge;

			return Math.Round(total, 2);
		}
	}

}
