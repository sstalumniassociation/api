using Api.Authorization;
using Article.V1;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace Api.Services.V1;

[AuthorizeMember]
public class ArticleServiceV1 : ArticleService.ArticleServiceBase
{
    public override async Task<ListArticlesResponse> ListArticles(ListArticlesRequest request,
        ServerCallContext context)
    {
        return await base.ListArticles(request, context);
    }
}