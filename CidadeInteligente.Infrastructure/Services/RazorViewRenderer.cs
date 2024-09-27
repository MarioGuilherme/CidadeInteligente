using CidadeInteligente.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace CidadeInteligente.Infrastructure.Services;

public class RazorViewRenderer(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider) : IRazorViewRenderer {
    private readonly IRazorViewEngine _viewEngine = viewEngine;
    private readonly ITempDataProvider _tempDataProvider = tempDataProvider;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<string> RenderViewToStringAsync<T>(string viewName, T model, string appUrl) {
        ActionContext actionContext = new(
            new DefaultHttpContext { RequestServices = this._serviceProvider },
            new RouteData(),
            new ActionDescriptor()
        );

        ViewEngineResult viewResult = this._viewEngine.FindView(actionContext, viewName, false);

        if (viewResult.View is null) throw new ArgumentNullException($"{viewName} não encontrado.");

        using StringWriter sw = new();
        ViewContext viewContext = new(
            actionContext,
            viewResult.View,
            new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary()
            ) { Model = model },
            new TempDataDictionary(actionContext.HttpContext, this._tempDataProvider),
            sw,
            new HtmlHelperOptions()
        );

        viewContext.ViewData["AppUrl"] = appUrl;

        await viewResult.View.RenderAsync(viewContext);
        return sw.ToString();
    }
}