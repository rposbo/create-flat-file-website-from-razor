using System.Text.RegularExpressions;
using System.Web.Helpers;
using RazorEngine.Templating;

namespace CreateFlatFileWebsiteFromRazor.Logic
{
    public class RenderHtmlPage
    {
        private readonly IContentRepository _contentRepository;
        private readonly IDataRepository _dataRepository;

        public RenderHtmlPage(IContentRepository contentRepository, IDataRepository dataRepository)
        {
            _contentRepository = contentRepository;
            _dataRepository = dataRepository;
        }

        public string BuildContentResult(string page, string id)
        {
            using (var service = new TemplateService())
            {
                // get the top level razor template, e.g. "product"
                // equivalent of "product.cshtml"
                var content = GetContent(page);
                var data = GetData(id);

                ProcessContent(content, service, data);
                var result = service.Parse(content, data, null, page);

                return result;
            }
        }

        private void ProcessContent(string content, TemplateService service, dynamic model)
        {
            // does the string passed in reference a Layout at the start?
            const string layoutPattern = @"@\{Layout = ""([_a-zA-Z]*)"";\}";

            // does the string passed in reference an Include anywhere?
            const string includePattern = @"@Include\(""([_a-zA-Z]*)""\)";

            // recursively process the Layout
            foreach (Match match in Regex.Matches(content, layoutPattern, RegexOptions.IgnoreCase))
            {
                ProcessSubContent(service, match, model);
            }

            // recursively process the @Includes
            foreach (Match match in Regex.Matches(content, includePattern, RegexOptions.IgnoreCase))
            {
                ProcessSubContent(service, match, model);
            }
        }

        private void ProcessSubContent(TemplateService service, Match match, dynamic model)
        {
            var subName = match.Groups[1].Value; // got an include/layout match?
            var subContent = GetContent(subName); // go get that template then
            ProcessContent(subContent, service, model); // recursively process it

            service.GetTemplate(subContent, model, subName); // then add it to the service
        }

        private string GetContent(string templateToLoad)
        {
            // hit the filesystem, db, API, etc to retrieve the razor template
            return _contentRepository.GetContent(templateToLoad);
        }

        private dynamic GetData(string dataToLoad)
        {
            // hit the filesystem, db, API, etc to return some Json data as the model
            return Json.Decode(_dataRepository.GetData(dataToLoad));
        }
    }
}
