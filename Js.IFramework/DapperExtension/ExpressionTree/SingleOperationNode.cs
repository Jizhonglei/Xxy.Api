using System.Linq.Expressions;

namespace IFramework.DapperExtension.ExpressionTree
{
    internal class SingleOperationNode : Node
    {
        public ExpressionType Operator { get; set; }
        public Node Child { get; set; }
    }
}
