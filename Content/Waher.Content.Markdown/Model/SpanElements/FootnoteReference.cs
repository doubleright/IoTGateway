﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Waher.Content.Markdown.Model.BlockElements;

namespace Waher.Content.Markdown.Model.SpanElements
{
	/// <summary>
	/// Footnote reference
	/// </summary>
	public class FootnoteReference : MarkdownElement
	{
		private readonly string key;

		/// <summary>
		/// Footnote reference
		/// </summary>
		/// <param name="Document">Markdown document.</param>
		/// <param name="Key">Meta-data key.</param>
		public FootnoteReference(MarkdownDocument Document, string Key)
			: base(Document)
		{
			this.key = Key;
		}

		/// <summary>
		/// Footnote key
		/// </summary>
		public string Key => this.key;

		/// <summary>
		/// Generates Markdown for the markdown element.
		/// </summary>
		/// <param name="Output">Markdown will be output here.</param>
		public override async Task GenerateMarkdown(StringBuilder Output)
		{
			Output.Append("[^");

			if (Guid.TryParse(this.key, out _) &&
				this.Document.TryGetFootnote(this.key, out Footnote Footnote))
			{
				StringBuilder sb = new StringBuilder();
				await Footnote.GenerateMarkdown(sb);
				Output.Append(sb.ToString().TrimEnd());
			}
			else
				Output.Append(this.key);

			Output.Append(']');
		}

		/// <summary>
		/// Generates HTML for the markdown element.
		/// </summary>
		/// <param name="Output">HTML will be output here.</param>
		public override Task GenerateHTML(StringBuilder Output)
		{
			string s;

			if (this.Document.TryGetFootnoteNumber(this.key, out int Nr))
			{
				s = Nr.ToString();

				Output.Append("<sup id=\"fnref-");
				Output.Append(s);
				Output.Append("\"><a href=\"#fn-");
				Output.Append(s);
				Output.Append("\" class=\"footnote-ref\">");
				Output.Append(s);
				Output.Append("</a></sup>");
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Generates plain text for the markdown element.
		/// </summary>
		/// <param name="Output">Plain text will be output here.</param>
		public override Task GeneratePlainText(StringBuilder Output)
		{
			if (this.Document.TryGetFootnoteNumber(this.key, out int Nr))
			{
				Output.Append(" [");
				Output.Append(Nr.ToString());
				Output.Append("]");
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return this.key;
		}

		/// <summary>
		/// Generates WPF XAML for the markdown element.
		/// </summary>
		/// <param name="Output">XAML will be output here.</param>
		/// <param name="TextAlignment">Alignment of text in element.</param>
		public override Task GenerateXAML(XmlWriter Output, TextAlignment TextAlignment)
		{
			if (this.Document.TryGetFootnoteNumber(this.key, out int Nr))
			{
				XamlSettings Settings = this.Document.Settings.XamlSettings;
				string s;

				Output.WriteStartElement("TextBlock");
				Output.WriteAttributeString("Text", Nr.ToString());

				Output.WriteStartElement("TextBlock.LayoutTransform");
				Output.WriteStartElement("TransformGroup");

				Output.WriteStartElement("ScaleTransform");
				Output.WriteAttributeString("ScaleX", s = CommonTypes.Encode(Settings.SuperscriptScale));
				Output.WriteAttributeString("ScaleY", s);
				Output.WriteEndElement();

				Output.WriteStartElement("TranslateTransform");
				Output.WriteAttributeString("Y", Settings.SuperscriptOffset.ToString());
				Output.WriteEndElement();

				Output.WriteEndElement();
				Output.WriteEndElement();
				Output.WriteEndElement();
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Generates Xamarin.Forms XAML for the markdown element.
		/// </summary>
		/// <param name="Output">XAML will be output here.</param>
		/// <param name="State">Xamarin Forms XAML Rendering State.</param>
		public override Task GenerateXamarinForms(XmlWriter Output, XamarinRenderingState State)
		{
			if (this.Document.TryGetFootnoteNumber(this.key, out int Nr))
			{
				bool Bak = State.Superscript;
				State.Superscript = true;

				Paragraph.GenerateXamarinFormsSpan(Output, Nr.ToString(), State);

				State.Superscript = Bak;
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Generates Human-Readable XML for Smart Contracts from the markdown text.
		/// Ref: https://gitlab.com/IEEE-SA/XMPPI/IoT/-/blob/master/SmartContracts.md#human-readable-text
		/// </summary>
		/// <param name="Output">Smart Contract XML will be output here.</param>
		/// <param name="State">Current rendering state.</param>
		public override Task GenerateSmartContractXml(XmlWriter Output, SmartContractRenderState State)
		{
			if (this.Document.TryGetFootnoteNumber(this.key, out int Nr))
			{
				Output.WriteStartElement("super");
				Output.WriteElementString("text", Nr.ToString());
				Output.WriteEndElement();
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Generates LaTeX for the markdown element.
		/// </summary>
		/// <param name="Output">LaTeX will be output here.</param>
		public override async Task GenerateLaTeX(StringBuilder Output)
		{
			if (this.Document.TryGetFootnote(this.key, out Footnote Footnote))
			{
				Output.Append("\\footnote");

				if (this.Document.TryGetFootnoteNumber(this.key, out int Nr))
				{
					Output.Append('[');
					Output.Append(Nr.ToString());
					Output.Append(']');
				}

				Output.Append('{');
				await Footnote.GenerateLaTeX(Output);
				Output.Append('}');
			}
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
			Output.WriteStartElement("FootnoteReference");

			if (this.Document.TryGetFootnoteNumber(this.key, out int Nr))
				Output.WriteAttributeString("nr", Nr.ToString());
			else
				Output.WriteAttributeString("key", this.key);

			Output.WriteEndElement();
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			return obj is FootnoteReference x &&
				this.key == x.key &&
				base.Equals(obj);
		}

		/// <summary>
		/// Serves as the default hash function.
		/// </summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode()
		{
			int h1 = base.GetHashCode();
			int h2 = this.key?.GetHashCode() ?? 0;

			h1 = ((h1 << 5) + h1) ^ h2;

			return h1;
		}

		/// <summary>
		/// Increments the property or properties in <paramref name="Statistics"/> corresponding to the element.
		/// </summary>
		/// <param name="Statistics">Contains statistics about the Markdown document.</param>
		public override void IncrementStatistics(MarkdownStatistics Statistics)
		{
			Statistics.NrFootnoteReference++;
		}

	}
}
