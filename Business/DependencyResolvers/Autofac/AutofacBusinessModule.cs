﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.EntityFramework.Contexts;
using Business.BusinessAspects.Pagination;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EGonulluContext>().As<EGonulluContext>().SingleInstance();

            builder.RegisterType<VolunteerManager>().As<IVolunteerService>();
            builder.RegisterType<EfVolunteerDal>().As<IVolunteerDal>();

            builder.RegisterType<AdvertisementManager>().As<IAdvertisementService>();
            builder.RegisterType<EfAdvertisementDal>().As<IAdvertisementDal>();
           
            builder.RegisterType<EfAdvertisementCategoryDal>().As<IAdvertisementCategoryDal>();
            builder.RegisterType<EfAdvertisementPurposeDal>().As<IAdvertisementPurposeDal>();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>();

            builder.RegisterType<EfAdvertisementVolunteerDal>().As<IAdvertisementVolunteerDal>();

            builder.RegisterType<PaginationUriManager>().As<IPaginationUriService>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
