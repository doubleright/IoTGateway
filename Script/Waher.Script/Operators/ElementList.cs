﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Waher.Script.Abstraction.Elements;
using Waher.Script.Model;
using Waher.Script.Objects;
using Waher.Script.Operators.Vectors;

namespace Waher.Script.Operators
{
	/// <summary>
	/// Represents a list of elements.
	/// </summary>
	public class ElementList : ScriptNode
	{
		private readonly ScriptNode[] elements;
		private readonly int nrElements;
		private bool isAsync;

		/// <summary>
		/// Represents a list of elements.
		/// </summary>
		/// <param name="Elements">Elements.</param>
		/// <param name="Start">Start position in script expression.</param>
		/// <param name="Length">Length of expression covered by node.</param>
		/// <param name="Expression">Expression containing script.</param>
		public ElementList(ScriptNode[] Elements, int Start, int Length, Expression Expression)
			: base(Start, Length, Expression)
		{
			this.elements = Elements;
			this.nrElements = Elements.Length;

			this.CalcIsAsync();
		}

		private void CalcIsAsync()
		{
			this.isAsync = false;

			for (int i = 0; i < this.nrElements; i++)
			{
				if (this.elements[i]?.IsAsynchronous ?? false)
				{
					this.isAsync = true;
					break;
				}
			}
		}

		/// <summary>
		/// Elements.
		/// </summary>
		public ScriptNode[] Elements => this.elements;

		/// <summary>
		/// If the node (or its decendants) include asynchronous evaluation. Asynchronous nodes should be evaluated using
		/// <see cref="ScriptNode.EvaluateAsync(Variables)"/>.
		/// </summary>
		public override bool IsAsynchronous => this.isAsync;

		/// <summary>
		/// Evaluates the node, using the variables provided in the <paramref name="Variables"/> collection.
		/// </summary>
		/// <param name="Variables">Variables collection.</param>
		/// <returns>Result.</returns>
		public override IElement Evaluate(Variables Variables)
		{
			LinkedList<IElement> List = new LinkedList<IElement>();
			int c = 0;

			foreach (ScriptNode E in this.elements)
			{
				if (E is null)
					List.AddLast((IElement)null);
				else
					List.AddLast(E.Evaluate(Variables));

				c++;
			}

			switch (c)
			{
				case 0:
					return ObjectValue.Null;

				case 1:
					return List.First.Value;

				case 2:
					if (List.First.Value is DoubleNumber Re && List.First.Next.Value is DoubleNumber Im)
						return new ComplexNumber(new Complex(Re.Value, Im.Value));
					break;
			}

			return VectorDefinition.Encapsulate(List, true, this);
		}

		/// <summary>
		/// Evaluates the node, using the variables provided in the <paramref name="Variables"/> collection.
		/// </summary>
		/// <param name="Variables">Variables collection.</param>
		/// <returns>Result.</returns>
		public override async Task<IElement> EvaluateAsync(Variables Variables)
		{
			if (!this.isAsync)
				return this.Evaluate(Variables);

			LinkedList<IElement> List = new LinkedList<IElement>();
			int c = 0;

			foreach (ScriptNode E in this.elements)
			{
				if (E is null)
					List.AddLast((IElement)null);
				else
					List.AddLast(await E.EvaluateAsync(Variables));

				c++;
			}

			switch (c)
			{
				case 0:
					return ObjectValue.Null;

				case 1:
					return List.First.Value;

				case 2:
					if (List.First.Value is DoubleNumber Re && List.First.Next.Value is DoubleNumber Im)
						return new ComplexNumber(new Complex(Re.Value, Im.Value));
					break;
			}

			return VectorDefinition.Encapsulate(List, true, this);
		}

		/// <summary>
		/// Calls the callback method for all child nodes.
		/// </summary>
		/// <param name="Callback">Callback method to call.</param>
		/// <param name="State">State object to pass on to the callback method.</param>
		/// <param name="DepthFirst">If calls are made depth first (true) or on each node and then its leaves (false).</param>
		/// <returns>If the process was completed.</returns>
		public override bool ForAllChildNodes(ScriptNodeEventHandler Callback, object State, bool DepthFirst)
		{
			if (DepthFirst)
			{
				if (!ForAllChildNodes(Callback, this.elements, State, DepthFirst))
					return false;
			}

			ScriptNode Node;
			bool RecalcIsAsync = false;
			int i;

			for (i = 0; i < this.nrElements; i++)
			{
				Node = this.elements[i];
				if (!(Node is null))
				{
					bool b = !Callback(Node, out ScriptNode NewNode, State);
					if (!(NewNode is null))
					{
						this.elements[i] = NewNode;
						RecalcIsAsync = true;
					}

					if (b)
					{
						if (RecalcIsAsync)
							this.CalcIsAsync();

						return false;
					}
				}
			}

			if (RecalcIsAsync)
				this.CalcIsAsync();

			if (!DepthFirst)
			{
				for (i = 0; i < this.nrElements; i++)
				{
					if (!ForAllChildNodes(Callback, this.elements, State, DepthFirst))
						return false;
				}
			}

			return true;
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			return obj is ElementList O &&
				AreEqual(this.elements, O.elements) &&
				base.Equals(obj);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			int Result = base.GetHashCode();
			Result ^= Result << 5 ^ GetHashCode(this.elements);
			return Result;
		}

	}
}
