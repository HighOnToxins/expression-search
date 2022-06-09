using ExpressionSearch.Syntax;
using System.Text.RegularExpressions;

namespace ExpressionSearch.Objects {
	[Expression(2)]
	public class VariableExpression: ObjectExpression<string> {

		//constructor
		public VariableExpression(string s) : base(s) {}

		//parse
		new public static Expression? Parse(string s) {
			string? value = Formatter.ReadRegexValue("([a-zA-Z_]+)([a-zA-Z0-9_]*)", s);
			return value == null ? null : new VariableExpression(value);
		}
	}
}
