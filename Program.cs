using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutofacTutorial.Interface;
using AutofacTutorial.Services.Impl;
using AutofacTutorial.Models.Reports;
using AutofacTutorial.Services;
using AutofacTutorial.Extentions;
using AutofacTutorial.ViewModels;
using ClientsDb;
using ClientsDb.Entities;
using AutofacTutorial.Mapper;

namespace AutofacTutorial
{
    //Домашняя работа #6 "Знакомство с autofac"
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure App

            builder.Services.AddDbContext<ClientContext>((optionts =>
            {
                optionts.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            }));

            builder.Services.AddScoped<ICrudClient, CrudClient>();
            builder.Services.AddScoped<ICrudOrder, CrudOrder>();
            builder.Services.AddScoped<Catalog>();
            builder.Services.AddScoped<IProdactService, ProductReportWord>();
            builder.Services.AddScoped<IReport,Report>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(typeof(ClientAppMappingProfile), typeof(OrderAppMappingProfile));
            #endregion

            //дальше идет конфигурирования обработки наших запросов
            var app = builder.Build();

            //страница по умолчании при ошибке
            if(app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();//позволяет подгрузить наш статический контент

            app.MapDefaultControllerRoute();

            app.Run();
        }
        
    }
}