using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace moonstone.ui.web.extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString MSValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.ValidationMessageFor(expression, string.Empty, new { @class = "ui red basic pointing label" }, "div");
        }
    }
}