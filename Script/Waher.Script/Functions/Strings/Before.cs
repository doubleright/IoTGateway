﻿using Waher.Script.Abstraction.Elements;
using Waher.Script.Model;
using Waher.Script.Objects;

namespace Waher.Script.Functions.Strings
{
    /// <summary>
    /// Before(s,Delimiter)
    /// </summary>
    public class Before : FunctionTwoScalarVariables
    {
        /// <summary>
        /// Before(s,Delimiter)
        /// </summary>
        /// <param name="String">String.</param>
        /// <param name="Delimiter">Delimiter</param>
        /// <param name="Start">Start position in script expression.</param>
        /// <param name="Length">Length of expression covered by node.</param>
        /// <param name="Expression">Expression containing script.</param>
        public Before(ScriptNode String, ScriptNode Delimiter, int Start, int Length, Expression Expression)
            : base(String, Delimiter, Start, Length, Expression)
        {
        }

        /// <summary>
        /// Name of the function
        /// </summary>
        public override string FunctionName => nameof(Before);

        /// <summary>
        /// Default Argument names
        /// </summary>
		public override string[] DefaultArgumentNames => new string[] { "s", "Delimiter" };

		/// <summary>
		/// Evaluates the function on a scalar argument.
		/// </summary>
		/// <param name="Argument1">String.</param>
        /// <param name="Argument2">Delimiter</param>
		/// <param name="Variables">Variables collection.</param>
		/// <returns>Function result.</returns>
		public override IElement EvaluateScalar(string Argument1, string Argument2, Variables Variables)
		{
            int i = Argument1.IndexOf(Argument2);
            if (i < 0)
                return ObjectValue.Null;
            else
                return new StringValue(Argument1.Substring(0, i));
		}

	}
}
