using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbracoSimpleJsProcessor
{
	public interface IPaymentProcessor
	{
		IEnumerable<string> Validate(PaymentInfo value);
		bool Charge(PaymentInfo payment, out string errorString, out string transactionId);
	}
}
