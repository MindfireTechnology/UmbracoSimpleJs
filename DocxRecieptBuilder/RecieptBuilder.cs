using DocxRecieptBuilder.Models;
using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DocxRecieptBuilder
{
    public class RecieptBuilder
    {
		//public void GetReciept(int orderID, string invoiceNumber)
		//{

		//}

		public Stream BuildReciept(Reciept data)
		{
			Stream docStream = new MemoryStream();

			using (DocX document = DocX.Load("/SourceDocuments/Aurora Awards Entry Receipt.docx"))
			{
				document.ReplaceText("{{DATE}}", data.Date);
				document.ReplaceText("{{INVOICENUMBER}}", data.InvoiceNumber);
				document.ReplaceText("{{AMOUNT}}", data.Amount.ToString());
				document.ReplaceText("{{ENTRANTID}}", data.EntrantID);
				document.ReplaceText("{{BILLEDNAME}}", data.BilledName);
				document.ReplaceText("{{COMPANYNAME}}", data.CompanyName);
				document.ReplaceText("{{PAYMENTMETHOD}}", data.PaymentMethod);
				document.ReplaceText("{{LASTFOUROFCARD}}", data.LastFourOfCard);
				document.ReplaceText("{{TRANSACTIONNUMBER}}", data.TransactionNumber);
				document.ReplaceText("{{ENTRANTNAME}}", data.EntrantName);
				document.ReplaceText("{{ADDRESS}}", data.Address);
				document.ReplaceText("{{STATEPROVINCE}}", data.StateProvince);
				document.ReplaceText("{{POSTALCODE}}", data.PostalCode);
				document.ReplaceText("{{COUNTRY}}", data.Country);
				document.ReplaceText("{{COMPETITIONNAME}}", data.CompetitionName);
				document.ReplaceText("{{QUANTITY}}", data.Quantity);
				document.ReplaceText("{{ENTRYNAME}}", data.EntryName);
				document.ReplaceText("{{C1}}", data.Cat1.CategoryCode);
				document.ReplaceText("{{C2}}", data.Cat2.CategoryCode);
				document.ReplaceText("{{C3}}", data.Cat3.CategoryCode);
				document.ReplaceText("{{C4}}", data.Cat4.CategoryCode);
				document.ReplaceText("{{C5}}", data.Cat5.CategoryCode);
				document.ReplaceText("{{CATEGORYNAMESUBCATNAME1}}", data.Cat1.CategoryName);
				document.ReplaceText("{{CATEGORYNAMESUBCATNAME2}}", data.Cat2.CategoryName);
				document.ReplaceText("{{CATEGORYNAMESUBCATNAME3}}", data.Cat3.CategoryName);
				document.ReplaceText("{{CATEGORYNAMESUBCATNAME4}}", data.Cat4.CategoryName);
				document.ReplaceText("{{CATEGORYNAMESUBCATNAME5}}", data.Cat5.CategoryName);

				document.SaveAs(docStream);
			}
			return docStream;
		}
    }
}
