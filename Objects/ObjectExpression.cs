using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionSearch.Objects {
	public abstract class ObjectExpression<T> : Expression {


		//fields
		public T Value { get; private init; }

		//constructor
		public ObjectExpression(T value) : base(value.ToString(), new Expression[0]) {
			Value = value;
		}



	}
}
