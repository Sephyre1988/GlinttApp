using System;
using AcademiadecodigoWarehouseApi.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiadecodigoWarehouseApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<WarehouseContext>(builder =>
            {
                builder.UseInMemoryDatabase("warehouse"); //Passa o nome da base de dados
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvcWithDefaultRoute();
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                SeedDummyData(scope.ServiceProvider.GetRequiredService<WarehouseContext>());
            }
        }

        private void SeedDummyData(WarehouseContext ctx)
        {
            var dummyData = new[]
            {
                new ProductEntity
                {
                    Code = "111",
                    Name = "Bola",
                    Description = "fantastico",
                    Price = 10.5m,
                    StockMoviments =
                    {
                        new StockMovimentEntity
                        {
                            Price = 10.5m,
                            Quantity = 2,
                            CreatedOn = DateTimeOffset.Now.AddDays(-2),
                            CreatedBy = "ruben.ribeiro"
                        }
                    },
                    CreatedOn = DateTimeOffset.Now.AddDays(-30),
                    CreatedBy = "ruben.ribeiro",
                    UpdatedOn = DateTimeOffset.Now.AddMinutes(-32),
                    UpdatedBy = "ruben.ribeiro"
                },

                new ProductEntity
                {
                    Code = "222",
                    Name = "Bola de futebol",
                    Description = "not so Awesome",
                    Price = 50.5m,
                    StockMoviments =
                    {
                        new StockMovimentEntity
                        {
                            Price = 50.5m,
                            Quantity = 2,
                            CreatedOn = DateTimeOffset.Now.AddDays(-20),
                            CreatedBy = "ruben.ribeiro"
                        }
                    },
                    CreatedOn = DateTimeOffset.Now.AddDays(-30),
                    CreatedBy = "ruben.ribeiro",
                    UpdatedOn = DateTimeOffset.Now.AddHours(-50),
                    UpdatedBy = "Marcolino"
                }
            };

            foreach (var p in dummyData) ctx.Add(p);

            ctx.SaveChanges();
        }
    }
}