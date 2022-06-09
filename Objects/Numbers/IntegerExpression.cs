using ExpressionSearch.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionSearch.Objects.Number {
	[Expression(1)]
	public class IntegerExpression : ObjectExpression<int> {

		//constructor
		public IntegerExpression(int value) : base(value) {}

		//parse
		new public static Expression? Parse(string s) {
			try {
				return new IntegerExpression(int.Parse(s));
			} catch(Exception) {
				return null;
			}
		}
	}
}
