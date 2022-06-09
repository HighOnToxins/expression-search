using ExpressionSearch.Objects;
using ExpressionSearch.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ExpressionSearch.Operators {
	[Expression(0)]
	internal class AddExpression : BinaryOperatorExpression {

		//format
		public static new string Format = "%s+%s";

		//constructor
		public AddExpression(Expression preimage0, Expression preimage1) : 
			base(Format, preimage0, preimage1) {}

		//compute
		public override int ComputeInt(int preimage0, int preimage1) { return preimage0 + preimage1; }

		//swap
		protected override void SwapPreimages() {
			Expression temop = Preimages[1];
			Preimages[1] = Preimages[0];
			Preimages[0] = temop;
		}

		//Default compute
		protected override Expression DefaultCompute(Expression preimage0, Expression preimage1) { return new AddExpression(preimage0, preimage1); }

		/// <summary> Parses the given string as an expression. </summary>
		new public static Expression? Parse(string s) {
			Expression[]? preimages = ExpressionParser.ReadExpression(s, Format, true);
			if(preimages == null || preimages.Length != 2) return null;
			return new AddExpression(preimages[0], preimages[1]);
		}

	}
}
