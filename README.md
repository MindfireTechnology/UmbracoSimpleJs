# UmbracoSimpleJs
Integrates SimpleJS into Umbraco

The best way to get this package is to wait for the release which will include all document types and dependencies. For manual installation:

1. Install SimpleCart.js onto your website
2. Setup with SendForm checkout:
```JavaScript
simpleCart({
		checkout: {
				type: "SendForm",

				// Replace this with the real URL
				url: "http://www.MindfireTechnology.com",

				// http method for form, "POST" or "GET", default is "POST"
				method: "POST",

				// url to return to on successful checkout, default is null
				//success: "success.html" , 

				// url to return to on cancelled checkout, default is null
				//cancel: "cancel.html" ,

				// an option list of extra name/value pairs that can
				// be sent along with the checkout data
				extra_data: {
						// Note: Make sure you include "OrderNumber" from the success of the beforeCheckout call to create the order in Umbraco
						OrderNumber: "1",
						CardNumber: "4111111111111111",
						CVV: "123",
						ExpMonth: "08",
						ExpYear: "2016"
				}
		}
});
```

3. Attach to the `beforeCheckout` event. This is where we will use the UmbracoDynamicDoc projec to create our Order and LineItems.
```JavaScript

// simple callback example
simpleCart.bind('beforeCheckout', function(data) {
		// This is where we would store our order (and get it's ID) using the umbracoDynamicObject lib

		// Create Order Here
		// Be sure to include creating all LineItem types

		var items = [];
		simpleCart.each(function(item, idx) {

			// Extract the item data -- locked away in functions
			var newObj = {
				name: item.fields().name,
				id: item.id(),
				price: item.price(),
				quantity: item.quantity(),
				total: item.total(),
				
				// Custom Info!
				size: item.options().size,
				custom: item.options().custom
			};
			
				items.push(newObj);
				alert(JSON.stringify(newObj));
		});

		// Visualize the data -- helpful for debugging
		data.items = items;
		alert(JSON.stringify(data));
});

```
4. Make sure you setup the configuration in your web.config file. The default credit card processor is Stripe (included) but other processors could be made.
```
	<appSettings>
		<!--Required for Stripe Processor-->
		<add key="StripeApiKey" value="[your api key here]" />
		<add key="ProcessorType" value="StripeProcessor.StripePaymentProcessor, StripeProcessor"/>
	</appSettings>
```
That's it! This will all make a lot more sense in the larger package.
