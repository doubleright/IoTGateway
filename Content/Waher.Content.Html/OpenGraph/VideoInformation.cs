﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Waher.Content.Html.OpenGraph
{
	/// <summary>
	/// Video information, as defined by the Open Graph protocol.
	/// </summary>
    public class VideoInformation : AudioInformation
    {
		private int? width = null;
		private int? height = null;

		/// <summary>
		/// Video information, as defined by the Open Graph protocol.
		/// </summary>
		public VideoInformation()
		{
		}

		/// <summary>
		/// The number of pixels wide.
		/// If not defined, null is returned.
		/// </summary>
		public int? Width
		{
			get => this.width;
			set => this.width = value;
		}

		/// <summary>
		/// The number of pixels high.
		/// If not defined, null is returned.
		/// </summary>
		public int? Height
		{
			get => this.height;
			set => this.height = value;
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			if (obj is VideoInformation Video)
			{
				return base.Equals(Video) &&
					this.width == Video.width &&
					this.height == Video.height;
			}
			else
				return false;
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			int Result = base.GetHashCode();

			if (this.width.HasValue)
				Result ^= Result << 5 ^ this.width.Value.GetHashCode();

			if (this.height.HasValue)
				Result ^= Result << 5 ^ this.height.Value.GetHashCode();

			return Result;
		}
	}
}
