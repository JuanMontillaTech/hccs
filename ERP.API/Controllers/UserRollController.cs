using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Model.Dtos;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRollController : ControllerBase
    {
        private readonly IGenericRepository<UserRoll> _repUserRoll;
        private readonly ICurrentUser _getCurrentUser;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRollController(IGenericRepository<UserRoll> repUserRoll, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, ICurrentUser getCurrentUser)
        {
            _repUserRoll = repUserRoll;
            _httpContextAccessor = httpContextAccessor;
            _getCurrentUser = getCurrentUser;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserRollDto data)
        {
            var mapper = _mapper.Map<UserRoll>(data);


            var result = await _repUserRoll.InsertAsync(mapper);
            try
            {
                var dataSave = await _repUserRoll.SaveChangesAsync();
                if (dataSave != 1)
                    return Ok(Result<UserRollDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<UserRollDto>(result);
                return Ok(Result<UserRollDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<UserRollDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repUserRoll.Find(x => x.IsActive).AsQueryable()
                .Include(x => x.Rolles).ToListAsync();

            var mapperOut = _mapper.Map<UserRollDetallisDto[]>(dataSave);

            return Ok(Result<UserRollDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetAllByUser")]
        public async Task<IActionResult> GetAllByUser()
        {
            var email = _getCurrentUser.UserEmail();
            var dataSave = await _repUserRoll.Find(x => x.IsActive && x.Email == email).AsQueryable()
                .Include(x => x.Rolles).ToListAsync();


            var mapperOut = _mapper.Map<UserRollDetallisDto[]>(dataSave);

            return Ok(Result<UserRollDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<UserRollDetallisDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            var userRolls = _repUserRoll.Find(x => x.IsActive == true
                                               && (x.Email.ToLower().Contains(filter.Search.Trim().ToLower()))
                                               && (x.Rolles.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).Include(x => x.Rolles).ToList();

            int totalRecords = userRolls.Count();
            var dataMaperOut = _mapper.Map<List<UserRollDetallisDto>>(userRolls);

            var list = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<UserRollDetallisDto>>.Success(list);
            return Ok(result);
        }
        
        [HttpGet("GetFilterUser")]
        [ProducesResponseType(typeof(Result<ICollection<UserRollDetallisDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilterUser([FromQuery] PaginationFilter filter)
        {
            var email = _getCurrentUser.UserEmail();
            var userRolls = _repUserRoll.Find(x => x.IsActive == true
                                               && x.Email ==email
                                               || (x.Rolles.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).Include(x => x.Rolles).ToList();

            int totalRecords = userRolls.Count();
            var dataMaperOut = _mapper.Map<List<UserRollDetallisDto>>(userRolls);

            var list = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<UserRollDetallisDto>>.Success(list);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repUserRoll.GetById(id);

            var mapperOut = _mapper.Map<UserRollDto>(dataSave);

            return Ok(Result<UserRollDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repUserRoll.GetById(id);

            data.IsActive = false;

            await _repUserRoll.Update(data);

            var save = await _repUserRoll.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<UserRollDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<UserRollDto>(data);

            return Ok(Result<UserRollDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserRollDto updateDto)
        {
            var mapper = _mapper.Map<UserRoll>(updateDto);
            mapper.IsActive = true;
            var result = await _repUserRoll.Update(mapper);

            var dataSave = await _repUserRoll.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<UserRollDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<UserRollDto>(result);

            return Ok(Result<UserRollDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}