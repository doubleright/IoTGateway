﻿using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Waher.Content.Markdown.Model.BlockElements;

namespace Waher.Content.Markdown.Model.SpanElements
{
	/// <summary>
	/// Automatic Link (e-Mail)
	/// </summary>
	public class AutomaticLinkMail : MarkdownElement
	{
		private readonly string eMail;

		/// <summary>
		/// Inline HTML.
		/// </summary>
		/// <param name="Document">Markdown document.</param>
		/// <param name="EMail">Automatic e-Mail link.</param>
		public AutomaticLinkMail(MarkdownDocument Document, string EMail)
			: base(Document)
		{
			this.eMail = EMail;
		}

		/// <summary>
		/// e-Mail
		/// </summary>
		public string EMail => this.eMail;

		/// <summary>
		/// Generates Markdown for the markdown element.
		/// </summary>
		/// <param name="Output">Markdown will be output here.</param>
		public override Task GenerateMarkdown(StringBuilder Output)
		{
			Output.Append('<');
			Output.Append(this.eMail);
			Output.Append('>');
		
			return Task.CompletedTask;
		}

		/// <summary>
		/// Generates HTML for the markdown element.
		/// </summary>
		/// <param name="Output">HTML will be output here.</param>
		public override Task GenerateHTML(StringBuilder Output)
		{
			string s = this.eMail;
			byte[] Data = System.Text.Encoding.ASCII.GetBytes(s);
			StringBuilder sb = new StringBuilder();

			foreach (byte b in Data)
			{
				sb.Append("&#x");
				sb.Append(b.ToString("X2"));
				sb.Append(';');
			}

			s = sb.ToString();

			sb.Clear();
			Data = Encoding.ASCII.GetBytes("mailto:");
			foreach (byte b in Data)
			{
				sb.Append("&#x");
				sb.Append(b.ToString("X2"));
				sb.Append(';');
			}

			Output.Append("<a href=\"");
			Output.Append(sb.ToString());
			Output.Append(s);
			Output.Append("\">");
			Output.Append(s);
			Output.Append("</a>");
		
			return Task.CompletedTask;
		}

		/// <summary>
		/// Generates plain text for the markdown element.
		/// </summary>
		/// <param name="Output">Plain text will be output here.</param>
		public override Task GeneratePlainText(StringBuilder Output)
		{
			Output.Append(this.eMail);
		
			return Task.CompletedTask;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return this.eMail;
		}

		/// <summary>
		/// Generates WPF XAML for the markdown element.
		/// </summary>
		/// <param name="Output">XAML will be output here.</param>
		/// <param name="TextAlignment">Alignment of text in element.</param>
		public override Task GenerateXAML(XmlWriter Output, TextAlignment TextAlignment)
		{
			Output.WriteStartElement("Hyperlink");
			Output.WriteAttributeString("NavigateUri", "mailto:" + this.eMail);
			Output.WriteValue(this.eMail);
			Output.WriteEndElement();
		
			return Task.CompletedTask;
		}

		/// <summary>
		/// Generates Xamarin.Forms XAML for the markdown element.
		/// </summary>
		/// <param name="Output">XAML will be output here.</param>
		/// <param name="State">Xamarin Forms XAML Rendering State.</param>
		public override Task GenerateXamarinForms(XmlWriter Output, XamarinRenderingState State)
		{
			string Bak = State.Hyperlink;
			State.Hyperlink = "mailto:" + this.eMail;
			Paragraph.GenerateXamarinFormsSpan(Output, State.Hyperlink, State);
			State.Hyperlink = Bak;

			return Task.CompletedTask;
		}

		/// <summary>
		/// Generates LaTeX for the markdown element.
		/// </summary>
		/// <param name="Output">LaTeX will be output here.</param>
		public override Task GenerateLaTeX(StringBuilder Output)
		{
			Output.AppendLine(InlineText.EscapeLaTeX(this.eMail));

			return Task.CompletedTask;
		}

		/// <summary>
		/// If the element is an inline span element.
		/// </summary>
		internal override bool InlineSpanElement => true;

		/// <summary>
		/// Exports the element to XML.
		/// </summary>
		/// <param name="Output">XML Output.</param>
		public override void Export(XmlWriter Output)
		{
			Output.WriteStartElement("AutomaticLinkMail");
			Output.WriteAttributeString("eMail", this.eMail);
			Output.WriteEndElement();
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			return obj is AutomaticLinkMail x &&
				this.eMail == x.eMail &&
				base.Equals(obj);
		}

		/// <summary>
		/// Serves as the default hash function.
		/// </summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode()
		{
			int h1 = base.GetHashCode();
			int h2 = this.eMail?.GetHashCode() ?? 0;

			h1 = ((h1 << 5) + h1) ^ h2;

			return h1;
		}

		/// <summary>
		/// Generates Human-Readable XML for Smart Contracts from the markdown text.
		/// Ref: https://gitlab.com/IEEE-SA/XMPPI/IoT/-/blob/master/SmartContracts.md#human-readable-text
		/// </summary>
		/// <param name="Output">Smart Contract XML will be output here.</param>
		/// <param name="State">Current rendering state.</param>
		public override Task GenerateSmartContractXml(XmlWriter Output, SmartContractRenderState State)
		{
			Output.WriteElementString("text", this.eMail);
		
			return Task.CompletedTask;
		}

		/// <summary>
		/// Increments the property or properties in <paramref name="Statistics"/> corresponding to the element.
		/// </summary>
		/// <param name="Statistics">Contains statistics about the Markdown document.</param>
		public override void IncrementStatistics(MarkdownStatistics Statistics)
		{
			if (Statistics.IntMailHyperlinks is null)
				Statistics.IntMailHyperlinks = new List<string>();

			if (!Statistics.IntMailHyperlinks.Contains(this.eMail))
				Statistics.IntMailHyperlinks.Add(this.eMail);

			Statistics.NrMailHyperLinks++;
			Statistics.NrHyperLinks++;
		}

	}
}
