using LibraryMS.DAL.Repositories.Implementation;
using LibraryMS.DAL.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryMS.DAL.Common
{
    public static class DataAccessDependencyInjection
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            //Unite of work
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            //Repository
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
