﻿using System;
using System.Reflection;
using Waher.Script.Abstraction.Sets;
using Waher.Script.Abstraction.Elements;

namespace Waher.Script.Objects
{
    /// <summary>
    /// Object value.
    /// </summary>
    public sealed class ObjectValue : Element, IDisposable
    {
        private static readonly ObjectValues associatedSet = new ObjectValues();

        private object value;

        /// <summary>
        /// Object value.
        /// </summary>
        /// <param name="Value">Object value.</param>
        public ObjectValue(object Value)
        {
            this.value = Value;
        }

        /// <summary>
        /// Object value.
        /// </summary>
        public object Value
        {
			get => this.value;
			set => this.value = value;
		}

        /// <inheritdoc/>
        public override string ToString()
        {
			if (this.value is null)
				return "null";
			else
				return Expression.ToString(this.value);
        }

        /// <summary>
        /// Associated Set.
        /// </summary>
        public override ISet AssociatedSet
        {
            get { return associatedSet; }
        }

        /// <summary>
        /// Associated object value.
        /// </summary>
        public override object AssociatedObjectValue => this.value;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
			if (obj is ObjectValue E)
			{
				if (this.value is null)
					return E.value is null;
				else if (E.value is null)
					return false;
				else
					return this.value.Equals(E.value);
			}
			else
				return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (this.value is null)
                return 0;
            else
                return this.value.GetHashCode();
        }

        /// <summary>
        /// Null value.
        /// </summary>
        public static readonly ObjectValue Null = new ObjectValue(null);

        /// <summary>
        /// Converts the value to a .NET type.
        /// </summary>
        /// <param name="DesiredType">Desired .NET type.</param>
        /// <param name="Value">Converted value.</param>
        /// <returns>If conversion was possible.</returns>
        public override bool TryConvertTo(Type DesiredType, out object Value)
        {
			if (this.value is null)
			{
				Value = null;
				return true;
			}
			else
			{
				TypeInfo TI = DesiredType.GetTypeInfo();

				if (TI.IsAssignableFrom(this.value.GetType().GetTypeInfo()))
				{
					Value = this.value;
					return true;
				}
				else if (TI.IsAssignableFrom(typeof(ObjectValue).GetTypeInfo()))
				{
					Value = this;
					return true;
				}
				else
					return Expression.TryConvert(this.value, DesiredType, out Value);
			}
		}

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>
        /// </summary>
		public void Dispose()
		{
            if (this.value is IDisposable D)
                D.Dispose();
		}
	}
}
