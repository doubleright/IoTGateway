﻿using System;
using Waher.Script.Abstraction.Elements;
using Waher.Script.Model;
using Waher.Script.Objects;

namespace Waher.Script.Operators.Arithmetics
{
	/// <summary>
	/// Element-wise Left-Division operator.
	/// </summary>
	public class LeftDivideElementWise : BinaryElementWiseOperator
	{
		/// <summary>
		/// Element-wise Left-Division operator.
		/// </summary>
		/// <param name="Left">Left operand.</param>
		/// <param name="Right">Right operand.</param>
		/// <param name="Start">Start position in script expression.</param>
		/// <param name="Length">Length of expression covered by node.</param>
		/// <param name="Expression">Expression containing script.</param>
		public LeftDivideElementWise(ScriptNode Left, ScriptNode Right, int Start, int Length, Expression Expression)
			: base(Left, Right, Start, Length, Expression)
		{
		}

        /// <summary>
        /// Evaluates the operator on scalar operands.
        /// </summary>
        /// <param name="Left">Left value.</param>
        /// <param name="Right">Right value.</param>
        /// <param name="Variables">Variables collection.</param>
        /// <returns>Result</returns>
        public override IElement EvaluateScalar(IElement Left, IElement Right, Variables Variables)
		{
			if (Left is DoubleNumber DL && Right is DoubleNumber DR)
				return new DoubleNumber(DR.Value / DL.Value);
			else
				return LeftDivide.EvaluateDivision(Left, Right, this);
		}

	}
}
