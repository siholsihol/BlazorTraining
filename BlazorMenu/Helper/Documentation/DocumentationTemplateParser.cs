namespace BlazorMenu.Helper.Documentation
{
    internal sealed class DocumentationTemplateParser
    {
        public static string ParseTemplate(string programId, string version)
        {
            var fileExtension = ".htm";
            var module = programId.Substring(0, 2);
            var programType = programId.Substring(2, 1);
            var program = programId + fileExtension;

            return string.Join("/", "doc", version, module, programType, program);
        }
    }
}
