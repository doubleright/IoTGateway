﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Waher.Script.Abstraction.Elements;
using Waher.Script.Abstraction.Sets;
using Waher.Script.Exceptions;
using Waher.Script.Model;
using Waher.Script.Objects;
using Waher.Script.Objects.VectorSpaces;

namespace Waher.Script.Operators.Vectors
{
    /// <summary>
    /// Creates a vector.
    /// </summary>
    public class VectorDefinition : ElementList
    {
        /// <summary>
        /// Creates a vector.
        /// </summary>
        /// <param name="Elements">Elements.</param>
        /// <param name="Start">Start position in script expression.</param>
        /// <param name="Length">Length of expression covered by node.</param>
		/// <param name="Expression">Expression containing script.</param>
        public VectorDefinition(ScriptNode[] Elements, int Start, int Length, Expression Expression)
            : base(Elements, Start, Length, Expression)
        {
        }

        /// <summary>
        /// Evaluates the node, using the variables provided in the <paramref name="Variables"/> collection.
        /// </summary>
        /// <param name="Variables">Variables collection.</param>
        /// <returns>Result.</returns>
        public override IElement Evaluate(Variables Variables)
        {
            LinkedList<IElement> VectorElements = new LinkedList<IElement>();

            foreach (ScriptNode Node in this.Elements)
			{
				if (Node is null)
					VectorElements.AddLast(ObjectValue.Null);
				else
					VectorElements.AddLast(Node.Evaluate(Variables));
			}

			return Encapsulate(VectorElements, true, this);
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

            LinkedList<IElement> VectorElements = new LinkedList<IElement>();

            foreach (ScriptNode Node in this.Elements)
            {
                if (Node is null)
                    VectorElements.AddLast(ObjectValue.Null);
                else
                    VectorElements.AddLast(await Node.EvaluateAsync(Variables));
            }

            return Encapsulate(VectorElements, true, this);
        }
        /// <summary>
        /// Encapsulates the elements of a vector.
        /// </summary>
        /// <param name="Elements">Vector elements.</param>
        /// <param name="CanEncapsulateAsMatrix">If the method can encapsulate the contents as a matrix.</param>
        /// <param name="Node">Script node from where the encapsulation is done.</param>
        /// <returns>Encapsulated vector.</returns>
        public static IVector Encapsulate(ICollection<IElement> Elements, bool CanEncapsulateAsMatrix, ScriptNode Node)
        {
            IElement SuperSetExample = null;
            IElement Element2;
            ISet CommonSuperSet = null;
            IVectorSpaceElement Vector;
            ISet Set;
            int? Columns = null;
            bool Upgraded = false;
            bool SameDimensions = true;

            foreach (IElement Element in Elements)
            {
                if (CanEncapsulateAsMatrix && SameDimensions)
                {
                    Vector = Element as IVectorSpaceElement;
                    if (Vector is null)
                        SameDimensions = false;
                    else
                    {
                        if (!Columns.HasValue)
                            Columns = Vector.Dimension;
                        else if (Columns.Value != Vector.Dimension)
                            SameDimensions = false;
                    }
                }

                if (CommonSuperSet is null)
                {
                    SuperSetExample = Element;

                    if (Element is null)
                        CommonSuperSet = new ObjectValues();
                    else
                        CommonSuperSet = Element.AssociatedSet;
                }
                else
                {
                    if (Element is null)
                        Set = new ObjectValues();
                    else
                        Set = Element.AssociatedSet;

                    if (!Set.Equals(CommonSuperSet))
                    {
                        Element2 = Element;
                        if (!Expression.UpgradeField(ref Element2, ref Set, ref SuperSetExample, ref CommonSuperSet))
                        {
                            CommonSuperSet = null;
                            break;
                        }
                        else
                            Upgraded = true;
                    }
                }
            }

            if (CanEncapsulateAsMatrix && SameDimensions && Columns.HasValue)
			{
                IMatrix M = Matrices.MatrixDefinition.Encapsulate(Elements, Node);
                if (M is IVector V)
                    return V;
                else
                    throw new ScriptRuntimeException("Unable to convert matrix to vector.", Node);
            }

            if (!(CommonSuperSet is null))
            {
                if (Upgraded)
                {
                    LinkedList<IElement> SuperElements = new LinkedList<IElement>();

                    foreach (IElement Element in Elements)
                    {
                        if (Element is null)
                            Set = new ObjectValues();
                        else
                            Set = Element.AssociatedSet;

                        if (Set.Equals(CommonSuperSet))
                            SuperElements.AddLast(Element);
                        else
                        {
                            Element2 = Element;
                            if (Expression.UpgradeField(ref Element2, ref Set, ref SuperSetExample, ref CommonSuperSet))
                                SuperElements.AddLast(Element2);
                            else
                            {
                                SuperElements = null;
                                CommonSuperSet = null;
                                break;
                            }
                        }
                    }

                    if (!(SuperElements is null))
                        Elements = SuperElements;
                }

                if (!(CommonSuperSet is null))
                {
                    if (CommonSuperSet is DoubleNumbers)
                        return new DoubleVector(Elements);
                    else if (CommonSuperSet is ComplexNumbers)
                        return new ComplexVector(Elements);
                    else if (CommonSuperSet is BooleanValues)
                        return new BooleanVector(Elements);
					else if (CommonSuperSet is DateTimeValues)
						return new DateTimeVector(Elements);
				}
			}

            return new ObjectVector(Elements);
        }

		/// <summary>
		/// Performs a pattern match operation.
		/// </summary>
		/// <param name="CheckAgainst">Value to check against.</param>
		/// <param name="AlreadyFound">Variables already identified.</param>
		/// <returns>Pattern match result</returns>
		public override PatternMatchResult PatternMatch(IElement CheckAgainst, Dictionary<string, IElement> AlreadyFound)
		{
			ScriptNode[] Elements = this.Elements;

            if (!(CheckAgainst is IVector Vector) || Vector.Dimension != Elements.Length)
                return PatternMatchResult.NoMatch;

            PatternMatchResult Result;
			int i = 0;

            foreach (IElement E in Vector.VectorElements)
			{
				Result = Elements[i++].PatternMatch(E, AlreadyFound);
				if (Result != PatternMatchResult.Match)
					return Result;
			}

			return PatternMatchResult.Match;
        }

    }
}
