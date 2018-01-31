﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vino.Core.CMS.Domain.Dto.System;
using Vino.Core.CMS.Domain.Entity.System;
using Vino.Core.CMS.IService.System;
using Vino.Core.Infrastructure.Exceptions;
using Vino.Core.Infrastructure.Helper;
using Vino.Core.Infrastructure.Extensions;
using Vino.Core.Infrastructure.IdGenerator;
using Vino.Core.CMS.Domain;

namespace Vino.Core.CMS.Service.System
{
    public partial class FunctionService : IFunctionService
    {
        #region 自动生成的方法

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="sort">排序</param>
        /// <returns>List<FunctionDto></returns>
        public async Task<List<FunctionDto>> GetListAsync(FunctionSearch where, string sort)
        {
            var data = await _repository.QueryAsync(where.GetExpression(), sort ?? "CreateTime desc");
            return _mapper.Map<List<FunctionDto>>(data);
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">条数</param>
        /// <param name="where">查询条件</param>
        /// <param name="sort">排序</param>
        /// <returns>count：条数；items：分页数据</returns>
        public async Task<(int count, List<FunctionDto> items)> GetListAsync(int page, int size, FunctionSearch where, string sort)
        {
            var data = await _repository.PageQueryAsync(page, size, where.GetExpression(), sort ?? "CreateTime desc");
            return (data.count, _mapper.Map<List<FunctionDto>>(data.items));
        }

        /// <summary>
        /// 根据主键取得数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<FunctionDto> GetByIdAsync(long id)
        {
            return _mapper.Map<FunctionDto>(await this._repository.GetByIdAsync(id));
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public async Task SaveAsync(FunctionDto dto)
        {
            Function model = _mapper.Map<Function>(dto);
            if (model.Id == 0)
            {
                //新增
                //取得父功能              
                if (model.ParentId.HasValue)
                {
                    var pModel = await _repository.GetByIdAsync(model.ParentId.Value);
                    if (pModel == null)
                    {
                        throw new VinoDataNotFoundException("无法取得父模块数据!");
                    }
                    if (!pModel.HasSub)
                    {
                        pModel.HasSub = true;
                        _repository.Update(pModel);
                    }
                    model.Level = pModel.Level + 1;
                }
                else
                {
                    model.ParentId = null;
                    model.Level = 1;
                }

                model.Id = ID.NewID();
                model.CreateTime = DateTime.Now;
                await _repository.InsertAsync(model);

            }
            else
            {
                //更新
                var item = await _repository.GetByIdAsync(model.Id);
                if (item == null)
                {
                    throw new VinoDataNotFoundException("无法取得功能数据！");
                }

                //TODO:这里进行赋值

                item.Name = model.Name;
                item.AuthCode = model.AuthCode;
                item.IsEnable = model.IsEnable;
                _repository.Update(item);
            }
            await _repository.SaveAsync();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();
        }

        #endregion

        #region 其他方法
		
        public Task<List<FunctionDto>> GetParentsAsync(long parentId)
        {
            List<FunctionDto> Gets()
            {
                return GetParents(parentId);
            }
            return Task.FromResult(Gets());
        }
		
        private List<FunctionDto> GetParents(long parentId)
        {
            var list = new List<Function>();
            void GetModel(long pid)
            {
                var model = _repository.FirstOrDefault(x => x.Id == pid);
                if (model != null)
                {
                    if (model.ParentId.HasValue)
                    {
                        GetModel(model.ParentId.Value);
                    }
                    list.Add(model);
                }
            }
            GetModel(parentId);
            return _mapper.Map<List<FunctionDto>>(list);
        }
		
        public async Task<List<FunctionDto>> GetSubsAsync(long? parentId)
        {
            var queryable = _repository.GetQueryable();
            if (parentId.HasValue)
            {
                queryable = queryable.Where(u => u.ParentId == parentId);
            }
            else
            {
                queryable = queryable.Where(u => u.ParentId == null);
            }

            var query = queryable.OrderBy(x => x.CreateTime);
            return _mapper.Map<List<FunctionDto>>(await query.ToListAsync());
        }
		
        #endregion
		
        #region 权限认证

        public async Task<bool> CheckUserAuth(long userId, string authCode)
        {
            if (authCode.IsNullOrEmpty())
            {
                return true;
            }
            var key = string.Format(CacheKeyDefinition.UserAuthCode, userId);
            var authcodes  = _cache.Get<List<string>>(key);
            if (authcodes == null)
            {
                //取得用户所有权限码
                List<string> codes = new List<string>();
                //取得所有角色
                var roles = await context.UserRoles.Include(t=>t.Role).Where(x => x.UserId == userId && x.Role.IsEnable).ToListAsync();
                if (roles.Any(x=>x.Role.NameEn.Equals("vino.developer")))
                {
                    authcodes = new List<string> {"vino.develop"};
                }
                else
                {
                    foreach (var role in roles)
                    {
                        var cds = await GetRoleAuthCodes(role.RoleId);
                        codes.AddRange(cds);
                    }
                    //去重
                    authcodes = codes.Distinct().ToList();
                }

                //缓存
                _cache.Add(key, authcodes);
            }
            if (authcodes.Contains("vino.develop"))
            {
                return true;
            }
            //验证
            //去除空格
            string tagnobk = authCode.Replace(" ", "");
            //去除括号和操作符
            string tagnofh = tagnobk.Replace("(", ",").Replace(")", ",").Replace("&&", ",").Replace("||", ",");
            //取得所有code
            string[] codeSplits = tagnofh.Split(',');
            if (codeSplits.Length == 1)
            {
                return authcodes.Contains(codeSplits[0]);
            }
            //TODO:以后扩展
            //foreach (var cd in codeSplits)
            //{
            //    if (cd.IsNullOrEmpty())
            //    {
            //        continue;
            //    }

            //    bool auth = authcodes.Contains(cd);
            //    if (auth)
            //    {
            //        Regex reg = new Regex(cd);
            //        tagnobk = reg.Replace(tagnobk, "true", 1);
            //    }
            //    else
            //    {
            //        Regex reg = new Regex(cd);
            //        tagnobk = reg.Replace(tagnobk, "false", 1);
            //    }
            //}

            return authcodes.Contains(authCode);
        }

        private async Task<List<string>> GetRoleAuthCodes(long roleId)
        {
            var functions = await context.RoleFunctions.Include(t => t.Function)
                .Where(x => x.RoleId == roleId && x.Function.IsEnable).ToListAsync();
            return functions.Select(x => x.Function.AuthCode).ToList();
        }

        public async Task<List<string>> GetUserAuthCodesAsync(long userId, bool encrypt = false)
        {
            var key = string.Format(encrypt?CacheKeyDefinition.UserAuthCodeEncrypt:CacheKeyDefinition.UserAuthCode, userId);
            var authcodes = _cache.Get<List<string>>(key);
            if (authcodes == null)
            {
                //取得用户所有权限码
                List<string> codes = new List<string>();
                //取得所有角色
                var roles = await context.UserRoles.Include(t => t.Role).Where(x => x.UserId == userId && x.Role.IsEnable).ToListAsync();
                if (roles.Any(x => x.Role.NameEn.Equals("vino.developer")))
                {
                    authcodes = new List<string> { "vino.develop" };
                }
                else
                {
                    foreach (var role in roles)
                    {
                        var cds = await GetRoleAuthCodes(role.RoleId);
                        codes.AddRange(cds);
                    }
                    //去重
                    authcodes = codes.Distinct().ToList();
                }
                if (encrypt)
                {
                    authcodes = authcodes.Select(CryptHelper.EncryptMD5).ToList();
                }
                //缓存
                _cache.Add(key, authcodes);
            }
            return authcodes;
        }

        #endregion
    }
}
