
using ExpressionSearch.Objects;
using ExpressionSearch.Objects.Number;
using ExpressionSearch.Operators;
using ExpressionSearch.Syntax;
using System.Reflection;

namespace ExpressionSearch {
	public abstract class Expression {

		/*
			FANCY MATH WORDS:
			- Calculate. Simple arithmetic operations.
			- Compute. Performing complicated tasks.
			- Simplify (Math). Make simple by reducing in complexity.
			- Reduce (Math). Rewriting of an expression into a simpler form.
			- Determine. To fix or define the position, form, or configuration of. (Getting exact value.)
		 */

		//fields
		protected Expression[] Preimages { get; init; }

		//properties
		public string Format { get; init; }

		//constructor
		public Expression(string format, Expression[] preimages) {
			Format = format;
			Preimages = preimages;
		}



		/// <summary> Returns the simplest form of this expression. </summary>
		public Expression Reduce(Expression? parent = null) {
			Expression[] simplifiedPreimages = new Expression[Preimages.Length];

			//push expressions of higher order up
			try {

				BinaryOperatorExpression thisExp = (BinaryOperatorExpression)this;
				bool changedOrder = thisExp.OrderPreimages();
				if(changedOrder && parent != null) return null;

				bool changedStructure = thisExp.Restructure(parent);
				if(changedStructure && parent != null) return null;

			} catch(Exception) { }

			for(int i = 0; i < Preimages.Length; i++) {
				simplifiedPreimages[i] = Preimages[i].Reduce(this);
				if(simplifiedPreimages[i] == null) return Reduce(parent);
			}

			return Compute(simplifiedPreimages);
		}

		/// <summary> Compute the given expression based on given simplified expresions. </summary>
		protected virtual Expression Compute(Expression[] simplifiedPreimages) {
			return this;
		}



		/// <summary> Converts this expression to string given the format. </summary>
		public override string ToString() {
			string[] preimageStrings = new string[Preimages.Length];

			for(int i = 0; i < Preimages.Length; i++) {
				preimageStrings[i] = Preimages[i].ToString();
			}

			return String.Format(Util.AddFormat(Format), preimageStrings);
		}

		/// <summary> Converts this expression to string given the format. </summary>
		public string GetStructure(int layer = 0) {
			string treeStructure = "";

			for(int i = 0; i < layer; i++) treeStructure += "\t";

			treeStructure += "\"" + Format + "\" " + (GetType().ToString()) + "\n";

			for(int i = 0; i < Preimages.Length; i++) {
				treeStructure += Preimages[i].GetStructure(layer + 1);
			}

			return treeStructure;
		}



		/// <summary> Parses the given string as an expression. </summary>
		public static Expression? Parse(string s) {return ExpressionParser.Parse(s);}

		/// <summary> Returns the precedence of the given expression. </summary>
		public static int GetPrecedence(Expression e) {
			for(int i = 0; i < ExpressionParser.precedence.Length; i++) {
				if(e.GetType() == ExpressionParser.precedence[i]) return i;
			}
			return -1;
		}


	}
}