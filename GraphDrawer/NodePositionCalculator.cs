namespace GraphDrawer;

public class NodePositionCalculator
{
    private readonly TreeDescription tree;
    private readonly Dictionary<RealNode, PointF> positions;
    private readonly SizeF nodeSize;

    public NodePositionCalculator(TreeDescription tree)
    {
        this.tree = tree;
        nodeSize = tree.NodeSize;
        positions = CalculatePositions(tree.Root);
    }
    private Dictionary<RealNode, PointF> CalculatePositions(RealNode root)
    {
        /*
             * 1. родитель всегда находится по горизонтали ровно по центру между детьми
             * 2. По вертикали позиция полностью определяется уровнем в дереве
             * 3. По горизонтали позиции вычисляются у детей слева на право.
             * 4. В каждый момент времени для каждого уровня храним минимальную горизонтальную позицию, в которую можно поместить узел.
             * 5. Поддерево помещаем как можно левее, учитывая минимальную позицию для уровня.
             * 6. Ширина каждого узла задается настройками и константна.
             */
        var res = new Dictionary<RealNode, PointF>();
        var minXByLevel = new float[10];
        SetPosition(root, 0, minXByLevel, res);
        var shiftToRight = tree.NodeSpacing.Width + nodeSize.Width / 2;
        ShiftRight(root, res, shiftToRight);
        return res;
    }

    public SizeF GetImageSize()
    {
        var rect = GetRect(tree.Root, positions);
        return new SizeF(rect.Width + 2*tree.NodeSpacing.Width, rect.Height + 2*tree.NodeSpacing.Height);
    }

    public PointF this[RealNode node] => positions[node];

    private RectangleF GetRect(RealNode node, Dictionary<RealNode, PointF> pos)
    {
        var res = new RectangleF(pos[node], nodeSize);
        foreach (RealNode child in node.RealChildren)
        {
            res = RectangleF.Union(res, GetRect(child, pos));
        }

        return res;
    }

    private void SetPosition(RealNode node, int level, float[] minXByLevel, Dictionary<RealNode, PointF> res)
    {
        if (node.RealChildren.Any())
        {
            foreach (var child in node.RealChildren)
            {
                SetPosition(child, level + 1, minXByLevel, res);
            }

            var minChildX = node.RealChildren.Min(c => res[c].X);
            var maxChildX = node.RealChildren.Max(c => res[c].X);
            var desiredPos = (minChildX + maxChildX) / 2;
            if (minXByLevel[level] > desiredPos)
            {
                var shiftToRight = minXByLevel[level] - desiredPos;
                foreach (var child in node.RealChildren)
                    ShiftRight(child, res, shiftToRight);
                for (int i = level + 1; i < minXByLevel.Length; i++)
                {
                    minXByLevel[i] += shiftToRight;

                }
            }
            var x = Math.Max(minXByLevel[level], desiredPos);
            minXByLevel[level] = x + nodeSize.Width + tree.NodeSpacing.Width;
            res.Add(node, new PointF(x, GetNodeYPos(level)));
        }
        else
        {
            var x = minXByLevel[level];
            minXByLevel[level] = x + nodeSize.Width + tree.NodeSpacing.Width;
            res.Add(node, new PointF(x, GetNodeYPos(level)));
        }
    }

    private float GetNodeYPos(int level)
    {
        var margin = tree.NodeSpacing.Height + nodeSize.Height / 2;
        return margin + level * (nodeSize.Height + tree.NodeSpacing.Height);
    }

    private void ShiftRight(RealNode node, Dictionary<RealNode, PointF> res, float shiftToRight)
    {
        res[node] += new PointF(shiftToRight, 0);

        foreach (var child in node.RealChildren)
            ShiftRight(child, res, shiftToRight);
    }

}