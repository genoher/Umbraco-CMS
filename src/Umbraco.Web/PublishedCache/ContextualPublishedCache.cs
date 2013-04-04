﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using Umbraco.Core.Models;
using Umbraco.Core.Xml;

namespace Umbraco.Web.PublishedCache
{
    /// <summary>
    /// Provides access to cached contents in a specified context.
    /// </summary>
    internal abstract class ContextualPublishedCache
    {
        protected readonly UmbracoContext UmbracoContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualPublishedCache"/> with a context.
        /// </summary>
        /// <param name="umbracoContext">The context.</param>
        protected ContextualPublishedCache(UmbracoContext umbracoContext)
        {
            UmbracoContext = umbracoContext;
        }

        /// <summary>
        /// Gets a content identified by its unique identifier.
        /// </summary>
        /// <param name="contentId">The content unique identifier.</param>
        /// <returns>The content, or null.</returns>
        public abstract IPublishedContent GetById(int contentId);

        /// <summary>
        /// Gets contents at root.
        /// </summary>
        /// <returns>The contents.</returns>
        public abstract IEnumerable<IPublishedContent> GetAtRoot();

        /// <summary>
        /// Gets a content resulting from an XPath query.
        /// </summary>
        /// <param name="xpath">The XPath query.</param>
        /// <param name="vars">Optional XPath variables.</param>
        /// <returns>The content, or null.</returns>
        /// <remarks>
        /// <para>If <param name="vars" /> is <c>null</c>, or is empty, or contains only one single
        /// value which itself is <c>null</c>, then variables are ignored.</para>
        /// <para>The XPath expression should reference variables as <c>$var</c>.</para>
        /// </remarks>
        public abstract IPublishedContent GetSingleByXPath(string xpath, params XPathVariable[] vars);

        /// <summary>
        /// Gets contents resulting from an XPath query.
        /// </summary>
        /// <param name="xpath">The XPath query.</param>
        /// <param name="vars">Optional XPath variables.</param>
        /// <returns>The contents.</returns>
        /// <remarks>
        /// <para>If <param name="vars" /> is <c>null</c>, or is empty, or contains only one single
        /// value which itself is <c>null</c>, then variables are ignored.</para>
        /// <para>The XPath expression should reference variables as <c>$var</c>.</para>
        /// </remarks>
        public abstract IEnumerable<IPublishedContent> GetByXPath(string xpath, params XPathVariable[] vars);

        /// <summary>
        /// Gets an XPath navigator that can be used to navigate contents.
        /// </summary>
        /// <returns>The XPath navigator.</returns>
        public abstract XPathNavigator GetXPathNavigator();

        /// <summary>
        /// Gets a value indicating whether the underlying non-contextual cache contains published content.
        /// </summary>
        /// <returns>A value indicating whether the underlying non-contextual cache contains published content.</returns>
        public abstract bool HasContent();
    }
}