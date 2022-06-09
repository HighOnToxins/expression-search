using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionSearch.Objects.Number;

namespace ExpressionSearch.Operators {
	public abstract class BinaryOperatorExpression: Expression{

		//constructor
		public BinaryOperatorExpression(string format, Expression preimage1, Expression preimage2) : 
			base(format, new Expression[] { preimage1, preimage2 }) {}

		//compute
		protected override Expression Compute(Expression[] simplifiedPreimages) {
			try {
				IntegerExpression preimage0 = (IntegerExpression)simplifiedPreimages[0];
				IntegerExpression preimage1 = (IntegerExpression)simplifiedPreimages[1];

				return new IntegerExpression(ComputeInt(preimage0.Value, preimage1.Value));

			} catch(Exception) {
				return DefaultCompute(simplifiedPreimages[0], simplifiedPreimages[1]);
			}
		}

		//check if the preimages should be swaped
		public bool OrderPreimages() {
			int leftOrder = Expression.GetPrecedence(Preimages[0]);
			int rightOrder = Expression.GetPrecedence(Preimages[1]);

			if(leftOrder > rightOrder) {
				SwapPreimages();
				return true;
			}

			return false;
		}

		public bool Restructure(Expression? parent) {
			try {
				BinaryOperatorExpression leftPre = (BinaryOperatorExpression)Preimages[0];

				int rightOrder1 = Expression.GetPrecedence(Preimages[1]);
				int rightOrder2 = Expression.GetPrecedence(leftPre.Preimages[1]);
				int order1 = Expression.GetPrecedence(this);
				int order2 = Expression.GetPrecedence(Preimages[0]);

				//restructure tree
				if(order1 == order2 && rightOrder2 > rightOrder1) {
					BinaryOperatorExpression temp = leftPre;
					Preimages[0] = leftPre.Preimages[0];
					temp.Preimages[0] = this;

					BinaryOperatorExpression? biParent = parent as BinaryOperatorExpression;
					if(biParent != null) biParent.Preimages[0] = temp;

					return true;
				}

			} catch(Exception) { }
			return false;
		}

		//swaps preimages
		protected abstract void SwapPreimages();

		//default compute
		protected abstract Expression DefaultCompute(Expression preimage0, Expression preimage1);

		/// <summary> Computes the result given an integer. This function is only called if the inputs are integers. </summary>
		public abstract int ComputeInt(int preimage0, int preimage1);


	}
}
