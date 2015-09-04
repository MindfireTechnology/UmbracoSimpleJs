using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;
using UmbracoSimpleJsProcessor;

namespace StripeProcessor
{
	public class StripePaymentProcessor : IPaymentProcessor
	{
		public IEnumerable<string> Validate(PaymentInfo value)
		{
			// Check required fields
			if (value.ChargeAmount < 0m)
				yield return "Invalid charge amount";

			if (string.IsNullOrWhiteSpace(value.Currency) || value.Currency.Length != 3)
				yield return "Invalid Currenty Code";

			if (string.IsNullOrWhiteSpace(value.CardNumber) || value.CardNumber.Length < 15 || value.CardNumber.Length > 16)
				yield return "Invalid format for Card Number";

			if (value.ExpirationYear < DateTime.Today.Year)
				yield return "Invalid Expiration Year";

			if (value.ExpirationMonth < 1 || value.ExpirationMonth > 12)
				yield return "Invalid Expiration Month";

			if (string.IsNullOrWhiteSpace(value.NameOnCard))
				yield return "NameOnCard is required!";
		}

		public bool Charge(PaymentInfo payment, out string errorString, out string transactionId)
		{
			// Create the customer
			// Create the charge
			var service = new StripeChargeService();
			service.ApiKey = ConfigurationManager.AppSettings["StripeApiKey"];
			var charge = service.Create(new StripeChargeCreateOptions
			{
				Amount = (int)(payment.ChargeAmount * 100),
				Currency = "USD",
				Description = payment.Description,
				Source = new StripeSourceOptions
				{
					Name = payment.NameOnCard,
					Number = payment.CardNumber,
					Cvc = payment.CVV,
					ExpirationYear = payment.ExpirationYear.ToString(),
					ExpirationMonth = payment.ExpirationMonth.ToString().PadLeft(2, '0'),
					AddressLine1 = payment.AddressLine1,
					AddressLine2 = payment.AddressLine2,
					AddressCity = payment.City,
					AddressState = payment.State,
					AddressZip = payment.PostCodeOrZip
				}
			});

			if (!charge.Paid)
			{
				transactionId = null;
				errorString = charge.FailureMessage ?? "Unable to process this transaction at this time.";
				return false;
			} 
			else
			{
				transactionId = charge.Id;
				errorString = null;
				return true;
			}

		}
	}
}
