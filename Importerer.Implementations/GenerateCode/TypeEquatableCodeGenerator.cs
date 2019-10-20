using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Importerer.Interfaces;

namespace Importerer.Implementations
{
    class TypeEquatableCodeGenerator : ITypeCodeGenerator
    {
        public string GenerateTypeCode(List<string> Properties, string TypeName)
        {
            StringBuilder ClassBuilder = new StringBuilder();

            //Add all the using statements
            ClassBuilder.Append("using System;");
            ClassBuilder.Append("using System.Collections.Generic;");
            ClassBuilder.Append("using System.Diagnostics.CodeAnalysis;");
            ClassBuilder.Append("using System.Linq;");
            ClassBuilder.Append("using System.Reflection;");

            ClassBuilder.Append("namespace ImporterType {");
            //Declare the class and make sure it inherits from IEquatable
            ClassBuilder.AppendFormat("public class {0} : IEquatable<{0}>", TypeName);
            ClassBuilder.Append(" {");

            //properties
            foreach (var p in Properties)
            {
                ClassBuilder.Append("	public string " + p + " { get; set; }");
            }
            ClassBuilder.Append(" ");

            //Equatable Method
            ClassBuilder.AppendFormat("	public bool Equals({0} other)", TypeName);
            ClassBuilder.Append("	{");
            ClassBuilder.Append("		if (ReferenceEquals(other, null))");
            ClassBuilder.Append("			return false;");
            ClassBuilder.Append("		if (ReferenceEquals(other, null))");
            ClassBuilder.Append("			return true;");
            foreach (var p in Properties)
            {
                //first line has the return statemen
                if (Properties.IndexOf(p) == 0)
                {
                    ClassBuilder.AppendFormat("			return {0}.Equals(other.{0})", p);
                }
                //last line needds to have a semicolon
                else if (Properties.IndexOf(p) == Properties.Count - 1)
                {
                    ClassBuilder.AppendFormat("				&& {0}.Equals(other.{0});", p);
                }
                else
                {
                    ClassBuilder.AppendFormat("				&& {0}.Equals(other.{0})", p);
                }

            }
            ClassBuilder.Append("	}");
            ClassBuilder.Append("}");
            ClassBuilder.Append("}");

            return ClassBuilder.ToString();
        }
    }
}
