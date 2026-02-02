using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCheckoutSystem
{
	public enum ItemType
	{
		Starter,
		Main,
		Drink
	}

	public class OrderItem
	{
		public ItemType Type { get; }
		public DateTime OrderedAt { get; }

		public OrderItem(ItemType type, DateTime orderedAt)
		{
			Type = type;
			OrderedAt = orderedAt;
		}
	}

}
