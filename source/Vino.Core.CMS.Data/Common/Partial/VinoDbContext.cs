﻿//----------------------------------------------------------------
// Copyright (C) 2018 vino 版权所有
//
// 文件名：VinoDbContext.cs
// 功能描述：DbContext
//
// 创建者：kulend@qq.com
// 创建时间：2018-02-04 19:05
// 说明：此代码由工具自动生成，对此文件的更改可能会导致不正确的行为，
// 并且如果重新生成代码，这些更改将会丢失。
//
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Vino.Core.CMS.Data.Common
{
    public partial class VinoDbContext : DbContext, IDbContext
    {
        public VinoDbContext(DbContextOptions<VinoDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 异步保存
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<Domain.Entity.System.User> Users { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Domain.Entity.System.Role> Roles { get; set; }

        /// <summary>
        /// 用户操作日志
        /// </summary>
        public DbSet<Domain.Entity.System.UserActionLog> UserActionLogs { get; set; }

        /// <summary>
        /// 功能
        /// </summary>
        public DbSet<Domain.Entity.System.Function> Functions { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<Domain.Entity.System.Menu> Menus { get; set; }

        /// <summary>
        /// 短信
        /// </summary>
        public DbSet<Domain.Entity.System.Sms> Smss { get; set; }

        /// <summary>
        /// 短信队列
        /// </summary>
        public DbSet<Domain.Entity.System.SmsQueue> SmsQueues { get; set; }

        /// <summary>
        /// 会员
        /// </summary>
        public DbSet<Domain.Entity.Membership.Member> Members { get; set; }

        /// <summary>
        /// 会员类型
        /// </summary>
        public DbSet<Domain.Entity.Membership.MemberType> MemberTypes { get; set; }

        /// <summary>
        /// 公众号
        /// </summary>
        public DbSet<Domain.Entity.WeChat.WxAccount> WxAccounts { get; set; }

        /// <summary>
        /// 微信菜单
        /// </summary>
        public DbSet<Domain.Entity.WeChat.WxMenu> WxMenus { get; set; }

        /// <summary>
        /// 微信用户标签
        /// </summary>
        public DbSet<Domain.Entity.WeChat.WxUserTag> WxUserTags { get; set; }

        /// <summary>
        /// 微信用户
        /// </summary>
        public DbSet<Domain.Entity.WeChat.WxUser> WxUsers { get; set; }

        /// <summary>
        /// 微信二维码
        /// </summary>
        public DbSet<Domain.Entity.WeChat.WxQrcode> WxQrcodes { get; set; }

        /// <summary>
        /// 图片素材
        /// </summary>
        public DbSet<Domain.Entity.Material.Picture> Pictures { get; set; }

        /// <summary>
        /// 素材分组
        /// </summary>
        public DbSet<Domain.Entity.Material.MaterialGroup> MaterialGroups { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        public DbSet<Domain.Entity.Content.Article> Articles { get; set; }

        /// <summary>
        /// 配送模板
        /// </summary>
        public DbSet<Domain.Entity.Mall.DeliveryTemplet> DeliveryTemplets { get; set; }

    }
}
