using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Caracan.Templates.Loader
{
    public class TemplateLoader : ITemplateLoader
    {
        private const string DefaultTemplate = "https://raw.githubusercontent.com/caracan-team/Caracan.Pdf/develop/Caracan.Templates/Templates/template.liquid";
        
        
        private string LoadFromFile(string fullPath)
        {
            return File.ReadAllText(fullPath);
        }
        
        private string GetFullPath(string fileName)
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Remove(0, 6);
            return Path.Combine(exePath, DefaultTemplate, fileName);
        }
        
        public async Task<string> GetTemplateAsync(string fileName)
        {
            var path = GetFullPath(fileName);
            var template = LoadFromFile(path);
            return template;
        }
    }
}