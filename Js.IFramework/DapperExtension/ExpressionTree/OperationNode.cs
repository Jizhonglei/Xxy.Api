using System.Linq.Expressions;

namespace IFramework.DapperExtension.ExpressionTree
{
    internal class OperationNode : Node
    {
        public ExpressionType Operator { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
    }
}
