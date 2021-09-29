using GangSungu.Span;

namespace GangSungu.AST;

public ref struct Module
{
    public Source Source;
    public List<TopLevel> TopLevels;
}
