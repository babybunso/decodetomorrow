using System;

namespace Digify
{
	public class Rainfall
	{
		public double Amount {get; set;}
		public DateTime DateTime {get; set;}

		public Rainfall(DateTime dateTime, double amount)
		{
			this.DateTime = dateTime;
			this.Amount = amount;
		}
	}
}