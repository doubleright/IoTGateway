﻿using System;
using System.Threading.Tasks;
using Waher.Script.Abstraction.Elements;
using Waher.Script.Model;
using Waher.Script.Operators.Binary;

namespace Waher.Script.Operators.Assignments.WithSelf
{
	/// <summary>
	/// Binary And with self operator.
	/// </summary>
	public class BinaryAndWithSelf : Assignment 
	{
        private readonly And and;

        /// <summary>
        /// Binary And with self operator.
        /// </summary>
        /// <param name="VariableName">Variable name..</param>
        /// <param name="Operand">Operand.</param>
        /// <param name="Start">Start position in script expression.</param>
        /// <param name="Length">Length of expression covered by node.</param>
		/// <param name="Expression">Expression containing script.</param>
        public BinaryAndWithSelf(string VariableName, ScriptNode Operand, int Start, int Length, Expression Expression)
			: base(VariableName, Operand, Start, Length, Expression)
		{
            this.and = new And(new VariableReference(VariableName, true, Start, Length, Expression), Operand, Start, Length, Expression);
        }

        /// <summary>
        /// Evaluates the node, using the variables provided in the <paramref name="Variables"/> collection.
        /// </summary>
        /// <param name="Variables">Variables collection.</param>
        /// <returns>Result.</returns>
        public override IElement Evaluate(Variables Variables)
		{
            IElement Result = this.and.Evaluate(Variables);
            Variables[this.VariableName] = Result;
            return Result;
        }

        /// <summary>
        /// Evaluates the node, using the variables provided in the <paramref name="Variables"/> collection.
        /// </summary>
        /// <param name="Variables">Variables collection.</param>
        /// <returns>Result.</returns>
        public override async Task<IElement> EvaluateAsync(Variables Variables)
        {
            IElement Result = await this.and.EvaluateAsync(Variables);
            Variables[this.VariableName] = Result;
            return Result;
        }
    }
}
