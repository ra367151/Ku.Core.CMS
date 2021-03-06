//----------------------------------------------------------------
// Copyright (C) 2018 kulend 版权所有
//
// 文件名：IWxQrcodeService.cs
// 功能描述：微信二维码 业务逻辑接口
//
// 创建者：kulend@qq.com
// 创建时间：2018-02-04 19:13
//
//----------------------------------------------------------------

using Ku.Core.CMS.Domain.Dto.WeChat;
using Ku.Core.CMS.Domain.Entity.WeChat;
using System.Threading.Tasks;

namespace Ku.Core.CMS.IService.WeChat
{
    public partial interface IWxQrcodeService : IBaseService<WxQrcode, WxQrcodeDto, WxQrcodeSearch>
    {
        /// <summary>
        /// 保存数据
        /// </summary>
        Task SaveAsync(WxQrcodeDto dto);
    }
}
