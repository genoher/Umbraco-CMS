using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Macros;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Templates;

namespace Umbraco.Web.PropertyEditors
{
	/// <summary>
	/// A value converter for TinyMCE that will ensure any macro content is rendered properly even when 
	/// used dynamically.
	/// </summary>
    // because that version of RTE converter parses {locallink} and executes macros, when going from
    // data to source, its source value has to be cached at the request level, because we have no idea
    // what the macros may depend on actually. An so, object and xpath need to follow... request, too.
    // note: the TinyMceValueConverter is NOT inherited, so the PropertyValueCache attribute here is not
    // actually required (since Request is default) but leave it here to be absolutely explicit.
    [PropertyValueType(typeof(IHtmlString))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Request)]
    internal class RteMacroRenderingValueConverter : TinyMceValueConverter
	{
        // NOT thread-safe over a request because it modifies the
        // global UmbracoContext.Current.InPreviewMode status. So it
        // should never execute in // over the same UmbracoContext with
        // different preview modes.
	    static string RenderRteMacros(string source, bool preview)
        {
            // save and set for macro rendering
            var inPreviewMode = UmbracoContext.Current.InPreviewMode;
	        UmbracoContext.Current.InPreviewMode = preview;

            var sb = new StringBuilder();
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            MacroTagParser.ParseMacros(
                source,
                //callback for when text block is found
                textBlock => sb.Append(textBlock),
                //callback for when macro syntax is found
                (macroAlias, macroAttributes) => sb.Append(umbracoHelper.RenderMacro(
                    macroAlias,
                    //needs to be explicitly casted to Dictionary<string, object>
                    macroAttributes.ConvertTo(x => (string)x, x => x)).ToString()));

            // restore
	        UmbracoContext.Current.InPreviewMode = inPreviewMode;

            return sb.ToString();
        }

	    public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
	    {
	        if (source == null) return null;
	        var sourceString = source.ToString();

            // ensures string is parsed for {localLink} and urls are resolved correctly
            sourceString = TemplateUtilities.ParseInternalLinks(sourceString, preview);
	        sourceString = TemplateUtilities.ResolveUrlsFromTextString(sourceString);
            // ensure string is parsed for macros and macros are executed correctly
            sourceString = RenderRteMacros(sourceString, preview);

	        return sourceString;
	    }
    }
}