//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//
// <copyright file="IUserService.cs">
//        最后生成时间：2018-01-30 17:47
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Vino.Core.Cache;
using Vino.Core.CMS.Data.Common;
using Vino.Core.CMS.Data.Repository.System;
using Vino.Core.CMS.IService.System;

namespace Vino.Core.CMS.Service.System
{
    public partial class UserService : BaseService, IUserService
    {
        protected readonly IUserRepository _repository;

        public UserService(VinoDbContext context, ICacheService cache, IMapper mapper, IUserRepository repository)
            : base(context, cache, mapper)
        {
            this._repository = repository;
        }
    }
}
