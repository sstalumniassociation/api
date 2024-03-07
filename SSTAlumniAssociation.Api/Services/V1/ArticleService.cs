using Article.V1;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using SSTAlumniAssociation.Api.Authorization;

namespace SSTAlumniAssociation.Api.Services.V1;

[AuthorizeMember]
public class ArticleServiceV1 : ArticleService.ArticleServiceBase
{
    public override async Task<ListArticlesResponse> ListArticles(ListArticlesRequest request,
        ServerCallContext context)
    {
        return await base.ListArticles(request, context);
    }
}