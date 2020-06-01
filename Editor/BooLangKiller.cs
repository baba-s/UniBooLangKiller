using SyntaxTree.VisualStudio.Unity.Bridge;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEditor;

namespace Kogane
{
	[InitializeOnLoad]
	internal static class BooLangKiller
	{
		static BooLangKiller()
		{
			ProjectFilesGenerator.ProjectFileGeneration += OnGeneration;
		}

		private static string OnGeneration( string name, string content )
		{
			var document = XDocument.Parse( content );

			document.Root?.Descendants()
				.Where( c => c.Name.LocalName == "Reference" )
				.Where( c => c.Attribute( "Include" )?.Value == "Boo.Lang" )
				.Remove()
				;

			var stream = new Utf8StringWriter();
			document.Save( stream );

			return stream.ToString();
		}

		private sealed class Utf8StringWriter : StringWriter
		{
			public override Encoding Encoding => Encoding.UTF8;
		}
	}
}