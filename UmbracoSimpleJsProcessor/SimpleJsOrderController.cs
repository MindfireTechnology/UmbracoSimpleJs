using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;

namespace UmbracoSimpleJsProcessor
{
	public class SimpleJsOrderController : UmbracoApiController
	{
		private const string OrderNumber = "OrderNumber";

		//currency=USD
		//shipping=0
		//tax=0
		//taxRate=0
		//itemCount=2
		//item_name_1=Another+Item&item_quantity_1=8
		//item_price_1=7.98
		//item_options_1=
		//item_name_2=Item+Name+Goes+Here
		//item_quantity_2=9
		//item_price_2=19.99
		//item_options_2=
		//orderId=1
		//OrderNumber=6789 {CUSTOM DATA}
		public string Process([FromBody] Dictionary<string, string> values)
		{
			if (!values.ContainsKey(OrderNumber))
				throw new ArgumentNullException(OrderNumber);

			int orderId;
			if (!int.TryParse(values[OrderNumber], out orderId))
				throw new ArgumentNullException(OrderNumber);

			if (!values.ContainsKey("CardNumber"))
				throw new ArgumentException("CardNumber");

			if (!values.ContainsKey("CVV"))
				throw new ArgumentException("CVV");

			if (!values.ContainsKey("ExpMonth"))
				throw new ArgumentException("ExpMonth");

			if (!values.ContainsKey("ExpYear"))
				throw new ArgumentException("ExpYear");

			var order = Services.ContentService.GetById(orderId);
			var payment = new PaymentInfo();

			payment.FirstName = TryGetDocumentValue<string>(order, "firstName");
			payment.LastName = TryGetDocumentValue<string>(order, "lastName");
			payment.AddressLine1 = TryGetDocumentValue<string>(order, "billingAddressLine1");
			payment.AddressLine2 = TryGetDocumentValue<string>(order, "billingAddressLine2");
			payment.City = TryGetDocumentValue<string>(order, "billingCity");
			payment.State = TryGetDocumentValue<string>(order, "billingState");
			payment.PostCodeOrZip = TryGetDocumentValue<string>(order, "billingPostCodeOrZip");
			payment.Country = TryGetDocumentValue<string>(order, "billingCountry");
			payment.Email = TryGetDocumentValue<string>(order, "Email");


			payment.NameOnCard = TryGetDocumentValue<string>(order, "NameOnCard", true);
			payment.CardNumber = values["CardNumber"];
			payment.ExpirationMonth = int.Parse(values["ExpMonth"]);
			payment.ExpirationYear = int.Parse(values["ExpYear"]);

			payment.ChargeAmount = TryGetDocumentValue<decimal>(order, "OrderTotal", true);
			payment.Currency = TryGetDocumentValue<string>(order, "Currency") ?? "USD";
			payment.OrderRefNumber = orderId.ToString();

			// Attempt to get the processor
			var processor = (IPaymentProcessor)Activator.CreateInstance(GetAssemblyName(), GetTypeName());

			// Validate the data
			var validation = processor.Validate(payment).ToArray();
			if (validation.Any())
				throw new ApplicationException("Payment returned validation errors: " + string.Join("\r\n\t", validation));

			// Process the order! -- Now we see if these data plans are worth the price we paid!
			string errorString;
			string transactionId;
			bool result = processor.Charge(payment, out errorString, out transactionId);

			if (!result)
				throw new InvalidOperationException("Credit Card Charge Failed. Reason: " + errorString);

			TrySetDocumentValue<string>(order, "TransactionId", transactionId, true);
			TrySetDocumentValue<DateTime>(order, "PaidDate", DateTime.Now, false);
			TrySetDocumentValue<decimal>(order, "PaidAmount", payment.ChargeAmount, false);

			Services.ContentService.SaveAndPublishWithStatus(order);

			return transactionId;
		}

		private T TryGetDocumentValue<T>(IContent doc, string propertyName, bool required = false)
		{
			var prop = doc.Properties.SingleOrDefault(n => n.Alias == propertyName);
			if (prop == null)
			{
				if (required)
					throw new InvalidOperationException("Order Document Type does not include a field named: " + propertyName);

				return default(T);
			}

			return (T)Convert.ChangeType(prop.Value, typeof(T));
		}

		private void TrySetDocumentValue<T>(IContent doc, string propertyName, T value, bool required = false)
		{
			var prop = doc.Properties.SingleOrDefault(n => n.Alias == propertyName);
			if (prop == null)
			{
				if (required)
					throw new InvalidOperationException("Order Document Type does not include a field named: " + propertyName);
			} 
			else
				prop.Value = value;
		}

		private string GetAssemblyName()
		{
			string processor = GetProcessor();
			int index = processor.LastIndexOf(',');

			if (index == -1)
				throw new ConfigurationException("Bad format for type name in 'ProcessorType'");

			return processor.Substring(index + 1);
		}

		private string GetTypeName()
		{
			string processor = GetProcessor();
			int index = processor.LastIndexOf(',');

			if (index == -1)
				throw new ConfigurationException("Bad format for type name in 'ProcessorType'");

			return processor.Substring(0, index);
		}

		private string GetProcessor()
		{
			string processorString = ConfigurationManager.AppSettings["ProcessorType"];
			if (string.IsNullOrWhiteSpace(processorString))
				throw new ConfigurationException("Missing Web.config setting for 'ProcessorType'");

			return processorString;
		}
	}
}
