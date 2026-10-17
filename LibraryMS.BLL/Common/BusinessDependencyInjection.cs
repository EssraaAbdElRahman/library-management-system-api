 using LibraryMS.BLL.Service.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryMS.BLL.Common
{
     public static class BusinessDependencyInjection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IBorrowService, BorrowService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(BusinessDependencyInjection).Assembly);
            });
            services.AddValidatorsFromAssembly(typeof(BusinessDependencyInjection).Assembly);
            return services;
        }
    }
}
