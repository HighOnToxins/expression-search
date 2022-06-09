using System.Reflection;

namespace ExpressionSearch.Syntax {

	public static class ExpressionParser {

		private delegate Expression? ParseDelegate(string s);

		//A list of each parse function 
		private static readonly ParseDelegate[] parseDelegates;

		//A list of each type of expression 
		public static readonly Type[] precedence;

		/// <summary> Creates a list of delegates. </summary>
		static ExpressionParser() {

			//gets data
			List<(float order, Type type, ParseDelegate parser)> precedenceList = new();

			IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => t.GetCustomAttribute<ExpressionAttribute>() != null);

			//gathers information
			foreach(Type type in types) {

				ExpressionAttribute expAttr = type.GetCustomAttribute<ExpressionAttribute>()!;

				try {
					ParseDelegate parseDelegate = (type.GetMethod("Parse")?.CreateDelegate(typeof(ParseDelegate)) as ParseDelegate)!;
					precedenceList.Add((expAttr.Order, type, parseDelegate));
				} catch {
					Console.Error.WriteLine($"Expression parser {type.DeclaringType}.{type.Name} doesn't have correct signature. It needs to be of the signature 'public static bool TryParse(string str, [NotNullWhen(true)] out IExpression? expression)'.");
				}
			}

			//Converts to lists
			IOrderedEnumerable <(float order, Type type, ParseDelegate parser)> orderedPrecedence = precedenceList.OrderBy(p => p.order);
			parseDelegates = orderedPrecedence.Select(p => p.parser).ToArray();
			precedence = orderedPrecedence.Select(p => p.type).ToArray();

		}

		/// <summary> Parse the given string ExpressionBase as an ExpressionBase. </summary>
		public static Expression? Parse(string s) {
			if(parseDelegates == null) throw new Exception("Undefined precedence.");
			
			Expression? expression = null;

			for(int i = 0; i < parseDelegates.Length && expression == null; i++) {
				expression = parseDelegates[i](s);
			}

			return expression;
		}

		/// <summary> Reads the given expression using formatting. </summary>
		public static Expression[]? ReadExpression(string expression, string format, bool leftAssociative) {

			string[]? preimageStrings = !leftAssociative ? Formatter.ReadFormat(format, expression) : Formatter.LastReadFormat(format, expression);

			if(preimageStrings == null) return null;

			Expression[] preimages = new Expression[preimageStrings.Length];
			for(int i = 0; i < preimages.Length; i++) {
				Expression? preimage = Expression.Parse(preimageStrings[i]);
				if(preimage == null) return null;
				preimages[i] = preimage;
			}

			return preimages;
		}

	}
}
