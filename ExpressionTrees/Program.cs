using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees
{
  class Program
  {
    static void Main(string[] args)
    {
      GetExpression().Compile()(Assembly.GetExecutingAssembly());

      Console.ReadKey();
    }

    static Expression<Action<Assembly>> GetExpression()
    {
      Expression<Action<Type>> printTypeName = (t) => Console.WriteLine($"\n{t.FullName}");
      Expression<Action<Type>> printPropertiesInfo = (t) => t
        .GetProperties()
        .ToList()
        .ForEach(p => Console.WriteLine($"{p.PropertyType.ToString()} {p.Name}"));

      return (arg) => arg
        .GetTypes()
        .ToList()
        .ForEach(t => Expression
          .Block(printTypeName, printPropertiesInfo)
          .Expressions
          .ToList()
          .ForEach(e => ((Expression<Action<Type>>)e).Compile()(t))
        );
    }
  }

  class CustomClass
  {
    public string Name { get; set; }
    public string Type { get; set; }
  }

  class AnotherClass
  {
    public int Id { get; set; }
    public DateTime Created { get; set; }
  }
}
