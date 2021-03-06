//----------------------------------------------------------------
// Copyright (C) 2018 kulend 版权所有
//
// 文件名：TimedTaskService.cs
// 功能描述：定时任务 业务逻辑处理类
//
// 创建者：kulend@qq.com
// 创建时间：2018-07-22 08:15
//
//----------------------------------------------------------------

using AutoMapper;
using Ku.Core.CMS.Domain.Dto.System;
using Ku.Core.CMS.Domain.Entity.System;
using Ku.Core.CMS.IService.System;
using Dnc.Extensions.Dapper;
using Ku.Core.Infrastructure.IdGenerator;
using System;
using System.Threading.Tasks;

namespace Ku.Core.CMS.Service.System
{
    public partial class TimedTaskService : BaseService<TimedTask, TimedTaskDto, TimedTaskSearch>, ITimedTaskService
    {
        #region 构造函数

        public TimedTaskService()
        {
        }

        #endregion

        /// <summary>
        /// 保存数据
        /// </summary>
        public async Task SaveAsync(TimedTaskDto dto)
        {
            TimedTask model = Mapper.Map<TimedTask>(dto);
            if (model.Id == 0)
            {
                //新增
                model.Id = ID.NewID();
                model.CreateTime = DateTime.Now;
                model.IsDeleted = false;
                model.Status = Domain.Enum.System.EmTimedTaskStatus.Disable;
                using (var dapper = DapperFactory.Create())
                {
                    await dapper.InsertAsync(model);
                }
            }
            else
            {
                //更新
                using (var dapper = DapperFactory.Create())
                {
                    var item = new {
                        //这里进行赋值
                        model.Group,
                        model.Name,
                        model.AssemblyName,
                        model.TypeName,
                        model.Cron,
                        model.ValidStartTime,
                        model.ValidEndTime
                    };
                    await dapper.UpdateAsync<TimedTask>(item, new { model.Id });
                }
            }
        }

        public async Task UpdateTaskStatusAsync(long id, dynamic data)
        {
            using (var dapper = DapperFactory.Create())
            {
                await dapper.UpdateAsync<TimedTask>(data, new { Id = id });
            }
        }

        public async Task IncreaseRunTimesAsync(long id)
        {
            using (var dapper = DapperFactory.Create())
            {
                //await dapper.ExecuteAsync($"UPDATE {dapper.Dialect.FormatTableName<TimedTask>()} SET {nameof(TimedTask.RunTimes)}={nameof(TimedTask.RunTimes)}+1 WHERE Id=@Id", new { Id = id });
            }
        }
    }
}
