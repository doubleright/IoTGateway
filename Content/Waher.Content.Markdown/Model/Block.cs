﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Waher.Content.Markdown.Model
{
	internal class Block
	{
		private readonly string[] rows;
		private readonly int[] positions;
		private int indent;
		private readonly int start;
		private readonly int end;

		public Block(string[] Rows, int[] Positions, int Indent)
			: this(Rows, Positions, Indent, 0, Rows.Length - 1)
		{
		}

		public Block(string[] Rows, int[] Positions, int Indent, int Start, int End)
		{
			this.rows = Rows;
			this.positions = Positions;
			this.indent = Indent;
			this.start = Start;
			this.end = End;
		}

		public int Indent
		{
			get => this.indent;
			set => this.indent = value;
		}

		public string[] Rows => this.rows;
		public int[] Positions => this.positions;
		public int Start => this.start;
		public int End => this.end;

		public bool IsPrefixedBy(string Prefix, bool MustHaveWhiteSpaceAfter)
		{
			return MarkdownDocument.IsPrefixedBy(this.rows[this.start], Prefix, MustHaveWhiteSpaceAfter);
		}

		public bool IsPrefixedByNumber(out int Numeral)
		{
			return MarkdownDocument.IsPrefixedByNumber(this.rows[this.start], out Numeral);
		}

		public List<Block> RemovePrefix(string Prefix, int NrCharacters)
		{
			List<Block> Result = new List<Block>();
			List<string> Rows = new List<string>();
			List<int> Positions = new List<int>();
			string s;
			int Indent = 0;
			int i, j, k;
			int d = Prefix.Length;
			int Pos;
			bool FirstRow = true;

			for (i = this.start; i <= this.end; i++)
			{
				s = this.rows[i];
				Pos = this.positions[i];

				if (s.StartsWith(Prefix))
				{
					s = s.Substring(d);
					Pos += d;
					j = d;
				}
				else
					j = 0;

				k = 0;
				foreach (char ch in s)
				{
					if (ch > ' ' && ch != 160)
						break;

					k++;

					if (ch == ' ' || ch == 160)
						j++;
					else if (ch == '\t')
						j += 4;

					if (!FirstRow && j >= NrCharacters)
						break;
				}

				if (k > 0)
				{
					s = s.Substring(k);
					Pos += k;
				}

				if (string.IsNullOrEmpty(s))
				{
					if (!FirstRow)
					{
						Result.Add(new Block(Rows.ToArray(), Positions.ToArray(), Indent));
						Rows.Clear();
						Positions.Clear();
						Indent = 0;
						FirstRow = true;
					}
				}
				else
				{
					if (FirstRow)
					{
						FirstRow = false;
						Indent = (j - NrCharacters) / 4;
					}

					Rows.Add(s);
					Positions.Add(Pos);
				}
			}

			if (!FirstRow)
				Result.Add(new Block(Rows.ToArray(), Positions.ToArray(), Indent));

			return Result;
		}

		private static readonly Regex caption = new Regex(@"^\s*([\[](?'Caption'[^\]]*)[\]])?([\[](?'Id'[^\]]*)[\]])\s*$", RegexOptions.Compiled);

		public bool IsTable(out TableInformation TableInformation)
		{
			string[] Rows = (string[])this.rows.Clone();
			int[] Positions = (int[])this.positions.Clone();
			string Caption = string.Empty;
			string Id = string.Empty;
			string s;
			int Pos;
			int Columns = 0;
			int UnderlineRow = -1;
			int i, j;
			int End = this.end;
			bool IsUnderline;
			Match M;

			TableInformation = null;

			for (i = this.start; i <= End; i++)
			{
				j = 0;
				IsUnderline = true;

				s = Rows[i];
				Pos = Positions[i];

				if (i == End && (M = caption.Match(s)).Success)
				{
					End--;
					Caption = M.Groups["Caption"].Value;
					Id = M.Groups["Id"].Value;
				}
				else
				{
					s = s.TrimEnd();

					if (s.StartsWith("|"))
					{
						if (s.EndsWith("|") && s != "|")
							s = s.Substring(1, s.Length - 2);
						else
							s = s.Substring(1);
					
						Pos++;
					}
					else if (s.EndsWith("|"))
						s = s.Substring(0, s.Length - 1);

					Rows[i] = s;
					Positions[i] = Pos;

					foreach (char ch in s)
					{
						if (ch == '|')
							j++;
						else if (ch != '-' && ch != ':' && ch > ' ' && ch != 160)
							IsUnderline = false;
					}

					if (IsUnderline && UnderlineRow < 0)
						UnderlineRow = i;

					if (j == 0)
						return false;
					else if (i == this.start)
						Columns = j + 1;
					else if (Columns != j + 1)
						return false;
				}
			}

			if (UnderlineRow < 0)
				return false;

			s = Rows[UnderlineRow];
			Pos = Positions[UnderlineRow];

			string[] Parts = s.Split('|');
			int[] PartPositions = new int[Columns];

			TextAlignment[] Alignments = new TextAlignment[Columns];
			string[] AlignmentDefinitions = new string[Columns];
			bool Left;
			bool Right;
			int Diff;

			for (j = 0; j < Columns; j++)
			{
				s = Parts[j];
				PartPositions[j] = Pos;

				Pos += s.Length + 1;

				s = s.TrimEnd();
				Diff = s.Length;
				s = s.TrimStart();
				Diff -= s.Length;
				PartPositions[j] += Diff;

				Left = s.StartsWith(":");
				Right = s.EndsWith(":");
				AlignmentDefinitions[j] = s;

				if (Left && Right)
					Alignments[j] = TextAlignment.Center;
				else if (Right)
					Alignments[j] = TextAlignment.Right;
				else
					Alignments[j] = TextAlignment.Left;

				if (Left)
					s = s.Substring(1);

				if (Right && !string.IsNullOrEmpty(s))
					s = s.Substring(0, s.Length - 1);

				foreach (char ch in s)
				{
					if (ch != '-')
						return false;
				}
			}

			TableInformation = new TableInformation
			{
				Alignments = Alignments,
				AlignmentDefinitions = AlignmentDefinitions,
				Caption = Caption,
				Columns = Columns,
				Id = Id,
				NrHeaderRows = (UnderlineRow - this.start),
				NrDataRows = End - UnderlineRow
			};

			TableInformation.Headers = new string[TableInformation.NrHeaderRows][];
			TableInformation.HeaderPositions = new int[TableInformation.NrHeaderRows][];
			TableInformation.Rows = new string[TableInformation.NrDataRows][];
			TableInformation.RowPositions = new int[TableInformation.NrDataRows][];

			for (i = 0; i < TableInformation.NrHeaderRows; i++)
			{
				TableInformation.Headers[i] = Rows[this.start + i].Split('|');
				TableInformation.HeaderPositions[i] = new int[Columns];

				Pos = Positions[this.start + i];
				for (j = 0; j < Columns; j++)
				{
					s = TableInformation.Headers[i][j];
					TableInformation.HeaderPositions[i][j] = Pos;
					Pos += s.Length + 1;

					if (string.IsNullOrEmpty(s))
						s = null;
					else
					{
						s = s.TrimEnd();
						Diff = s.Length;
						s = s.TrimStart();
						Diff -= s.Length;
						TableInformation.HeaderPositions[i][j] += Diff;
					}

					TableInformation.Headers[i][j] = s;
				}
			}

			for (i = 0; i < TableInformation.NrDataRows; i++)
			{
				TableInformation.Rows[i] = Rows[UnderlineRow + i + 1].Split('|');
				TableInformation.RowPositions[i] = new int[Columns];

				Pos = Positions[UnderlineRow + i + 1];
				for (j = 0; j < Columns; j++)
				{
					s = TableInformation.Rows[i][j];
					TableInformation.RowPositions[i][j] = Pos;
					Pos += s.Length + 1;

					if (string.IsNullOrEmpty(s))
						s = null;
					else
					{
						s = s.TrimEnd();
						Diff = s.Length;
						s = s.TrimStart();
						Diff -= s.Length;
						TableInformation.RowPositions[i][j] += Diff;
					}

					TableInformation.Rows[i][j] = s;
				}
			}

			return true;
		}

		public bool IsFootnote(out string Label)
		{
			string s;
			int i, j, c;
			char ch;

			Label = null;
			s = this.rows[this.start];

			if (!s.StartsWith("[^"))
				return false;

			i = s.IndexOf("]:");
			if (i < 0)
				return false;

			Label = s.Substring(2, i - 2);

			i += 2;
			j = 0;
			c = s.Length;

			while (i < c && j < 3 && ((ch = s[i]) <= ' ' || ch == 160))
			{
				i++;

				if (ch == ' ' || ch == 160)
					j++;
				else if (ch == '\t')
					j += 4;
			}

			this.rows[this.start] = s.Substring(i);

			return true;
		}

	}
}
