using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using TechTalk.SpecFlow;
using RestaurantCheckoutSystem;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace SpecFlowTests.StepDefinitions
{
	[Binding]
	public class OrderBillingSteps
	{
		private Order _order;
		private DateTime _currentOrderTime;
		private decimal _billTotal;
		private static PricingOptions pricingOptions;

		[BeforeTestRun]
		public static void BeforeTestRun()
		{
			IConfiguration config = new ConfigurationBuilder()
		   .AddJsonFile("appsettings.json", optional: false)
		   .Build();

			pricingOptions = config
				.GetSection("Pricing")
				.Get<PricingOptions>();
		}

		[Given(@"an order for a group of (\d+) people placed at (.*)")]
		public void GivenAnOrderPlacedAt(int people, string time)
		{
			_currentOrderTime = DateTime.Today.Add(TimeSpan.Parse(time));
			_order = new Order(pricingOptions);
		}

		[Given(@"the order contains (\d+) starter, (\d+) mains and (\d+) drinks")]
		public void GivenTheOrderContainsItems(int starters, int mains, int drinks)
		{
			AddItems(ItemType.Starter, starters);
			AddItems(ItemType.Main, mains);
			AddItems(ItemType.Drink, drinks);

			/*
			 * 
			 * var orderData = new
				{
					orderId = "1234567890",
					orderTime = "18:00",
					starters = starters,
            		mains = mains,
            		drinks = drinks
				};

				var json = System.Text.Json.JsonSerializer.Serialize(data);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await client.PostAsync(url, content);
			 * 
			 * 
			 * 
			 * */
		}

		[Given(@"a time-based discount applies to items ordered before (.*)")]
		public void GivenTimeBasedDiscountApplies(string time)
		{
			// is calculated in WhenGuestsRequestFinalBill()
		}

		[Given(@"no time-based discount applies to items ordered after (.*)")]
		public void GivenNoTimeBasedDiscountApplies(string time)
		{
			// is calculated in WhenGuestsRequestFinalBill()
		}

		[Then("service charge is applied")]
		public void ThenServiceChargeIsApplied()
		{
			// is calculated in WhenGuestsRequestFinalBill()
		}


		[When(@"the guests request the bill")]
		public void WhenGuestsRequestTheBill()
		{
			_billTotal = _order.CalculateTotal();

			/*
			 * 
			 * var orderId = "1234567890";
				var url = $"https://newOrderEndpoint.com/api/GetOrderBill?orderId={orderId}";

				var response = await client.GetAsync(url);

				response.EnsureSuccessStatusCode();

				var responseBody = await response.Content.ReadAsStringAsync();

				var billTotalFromResponse = JsonSerializer.Deserialize<GetOrderBillResponse>(responseBody);
			 
				_billTotal = billTotalFromResponse;
			 * 
			 * 
			 * 
			 * */
		}

		[When(@"(\d+) more people join the group at (.*)")]
		public void WhenMorePeopleJoinAt(int people, string time)
		{
			_currentOrderTime = DateTime.Today.Add(TimeSpan.Parse(time));
		}

		[When(@"they order (\d+) mains and (\d+) drinks")]
		public void WhenTheyOrderMoreItems(int mains, int drinks)
		{
			AddItems(ItemType.Main, mains);
			AddItems(ItemType.Drink, drinks);
		}

		[Then(@"no discount should be applied to the newly ordered items")]
		public void ThenNoDiscountApplied()
		{
			// is calculated in WhenGuestsRequestFinalBill()
		}

		[When(@"the guests request the final bill")]
		public void WhenGuestsRequestFinalBill()
		{
			_billTotal = _order.CalculateTotal();
		}

		[Then(@"the bill total should be ([0-9]*\.?[0-9]+)")]
		public void ThenFinalBillIsCorrect(decimal expectedTotal)
		{
			Assert.AreEqual(expectedTotal, _billTotal);
		}

		private void AddItems(ItemType type, int count)
		{
			for (int i = 0; i < count; i++)
			{
				_order.AddItem(type, _currentOrderTime);
			}
		}

		[When(@"(\d+) person cancels their order at (.*)")]
		public void WhenOrderIsPartiallyCancelled(int people, string time)
		{
			// Placeholder step
		}

		[When(@"the order is updated to remove (\d+) starter, (\d+) main and (\d+) drink ordered at (.*)")]
		public void WhenTheOrderIsUpdated(int starters, int mains, int drinks, string time)
		{
			var orderedAt = DateTime.Today.Add(TimeSpan.Parse(time));

			_order.CancelSpecificItems(ItemType.Starter, starters, orderedAt);
			_order.CancelSpecificItems(ItemType.Main, mains, orderedAt);
			_order.CancelSpecificItems(ItemType.Drink, drinks, orderedAt);
		}

	}
}

