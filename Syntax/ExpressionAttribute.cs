
namespace ExpressionSearch.Syntax{
	public sealed class ExpressionAttribute : Attribute{
        public float Order { get; }
        
        public ExpressionAttribute(float order) {
			Order = order;
		}
	}
}
