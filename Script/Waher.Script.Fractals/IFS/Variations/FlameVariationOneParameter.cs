﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Waher.Script.Abstraction.Elements;
using Waher.Script.Abstraction.Sets;
using Waher.Script.Model;
using Waher.Script.Objects.VectorSpaces;
using Waher.Script.Operators;

namespace Waher.Script.Fractals.IFS.Variations
{
	public abstract class FlameVariationOneParameter : FunctionOneScalarVariable, IFlameVariation
	{
		protected double[] homogeneousTransform = new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 };
		protected double variationWeight = 1;

		public FlameVariationOneParameter(ScriptNode Parameter, int Start, int Length, Expression Expression)
			: base(Parameter, Start, Length, Expression)
		{
		}

		public override string ToString()
		{
			return LambdaDefinition.ToString(this);
		}

		#region IFlameVariation Members

		public void Initialize(double[] HomogeneousTransform, double VariationWeight)
		{
			this.homogeneousTransform = HomogeneousTransform;
			this.variationWeight = VariationWeight;
		}

		public abstract void Operate(ref double x, ref double y);

		#endregion

		#region ILambdaExpression Members

		public IElement Evaluate(IElement[] Arguments, Variables Variables)
		{
			double x = Expression.ToDouble(Arguments[0].AssociatedObjectValue);
			double y = Expression.ToDouble(Arguments[1].AssociatedObjectValue);

			this.Operate(ref x, ref y);

			return new DoubleVector(new double[] { x, y });
		}

		public Task<IElement> EvaluateAsync(IElement[] Arguments, Variables Variables)
		{
			return Task.FromResult<IElement>(this.Evaluate(Arguments, Variables));
		}

		public int NrArguments => 2;
		public string[] ArgumentNames => FlameVariationZeroParameters.parameterNames;
		public ArgumentType[] ArgumentTypes => FlameVariationZeroParameters.parameterTypes;

		#endregion

		#region IElement members

		public IElement Encapsulate(ICollection<IElement> Elements, ScriptNode Node)
		{
			return this;
		}

		public bool TryConvertTo(Type DesiredType, out object Value)
		{
			Value = null;
			return false;
		}

		public ISet AssociatedSet => SetOfVariations.Instance;
		public object AssociatedObjectValue => this;
		public bool IsScalar => false;
		public ICollection<IElement> ChildElements => new IElement[0];

		#endregion
	}
}
