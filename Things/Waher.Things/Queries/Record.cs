﻿using System;

namespace Waher.Things.Queries
{
	/// <summary>
	/// Defines a record in a table.
	/// </summary>
	public class Record
	{
		private readonly object[] elements;

		/// <summary>
		/// Defines a record in a table.
		/// </summary>
		public Record(params object[] Elements)
		{
			this.elements = Elements;
		}

		/// <summary>
		/// Record elements.
		/// </summary>
		public object[] Elements => this.elements;
	}
}
