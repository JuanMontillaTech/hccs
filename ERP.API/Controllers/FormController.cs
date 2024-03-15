using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Infrastructure.Repositories;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using System.Net;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly ISysRepository<Form> RepForms;
        private readonly ISysRepository<Module> RepModule;
        private readonly IGenericRepository<UserRoll> RepUserRoll;
        private readonly IGenericRepository<RollForm> RepRollForm;
        private readonly IDirectSql RepDynamic;
        private readonly IMapper _mapper;
        private readonly ICurrentUser currentUser;

        public FormController(ISysRepository<Form> repForms, 
            ISysRepository<Module> repModule, IGenericRepository<UserRoll> repUserRoll,
            IGenericRepository<RollForm> repRollForml, 
            IMapper mapper, IDirectSql _RepDynamic , ICurrentUser currentUser)
        {
            RepForms = repForms;
            RepModule = repModule;
            RepUserRoll = repUserRoll;
            RepRollForm = repRollForml;
            RepDynamic = _RepDynamic;
            _mapper = mapper;
            this.currentUser = currentUser;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] FormDto data)
        {
            var mapper = _mapper.Map<Form>(data);

            var result = await RepForms.InsertAsync(mapper);

            var dataSave = await RepForms.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<FormDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<FormDto>(result);

            return Ok(Result<FormDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await RepForms.GetAll();

            var filter = dataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<List<FormDto>>(filter);

            return Ok(Result<List<FormDto>>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<FormDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepForms.Find(x => x.IsActive == true
            && (x.Title.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.FormCode.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Label.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = Filter.Count();
            var DataMaperOut = _mapper.Map<List<FormDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<FormDto>>.Success(List);
            return Ok(Result);
        }


        [HttpGet("GetMenu")]
        public async Task<IActionResult> GetMenu()
        {
            try
            {
                var _UserRoll = currentUser.Roll();

                if (_UserRoll == Guid.Empty)
                    return Ok(Result<List<MenuDto>>.Fail(null, "Usuario no tiene Roll"));

                var DataModule = await RepModule.Find(x => x.IsActive == true).Include(x => x.Froms)
                    .OrderBy(x => x.Index).ToListAsync();

                List<MenuDto> Menu = new();
                foreach (var MenuRow in DataModule)
                {
                    MenuDto addNewMenu = new();
                    addNewMenu.Id = MenuRow.Id.ToString();
                    addNewMenu.Icon = MenuRow.Icon;
                    addNewMenu.IsTitle = MenuRow.IsTitle;
                    addNewMenu.Label = MenuRow.Label;
                    addNewMenu.Link = MenuRow.Link;
                    List<SubItem> listSub = new();
                    var menuOptionRows = MenuRow.Froms.Where(x => x.IsActive == true).OrderBy(x => x.Index).ToList();
                    foreach (var MenuOptionRow in menuOptionRows)
                    {
                        var fromValidate = await RepRollForm.Find(x =>
                                x.RollId == _UserRoll && x.FormId == MenuOptionRow.Id && x.IsActive == true)
                            .FirstOrDefaultAsync();
                        if (fromValidate != null)
                        {
                            SubItem menuOptionDto = new();
                            menuOptionDto.Label = MenuOptionRow.Label;
                            menuOptionDto.Id = MenuOptionRow.Id.ToString();
                            switch (MenuOptionRow.FormCode)
                            {
                                case "DM":
                                    menuOptionDto.Link = "/Forms/Index?Form=" + menuOptionDto.Id;
                                    break;
                                case "EX":
                                    menuOptionDto.Link = "/ExpressForm/Index?Form=" + menuOptionDto.Id;
                                    break;
                                case "FEX":
                                    menuOptionDto.Link = "/ExpressForm/Index?Form=" + menuOptionDto.Id;
                                    break;
                                case "CPX":
                                    menuOptionDto.Link = "/ExpressForm/Index?Form=" + menuOptionDto.Id;
                                    break;
                                case "RPT":
                                    menuOptionDto.Link = "/ExpressForm/Report?Form=" + menuOptionDto.Id;
                                    break;
                                case "REC":
                                    menuOptionDto.Link = "/ExpressForm/Index?Form=" + menuOptionDto.Id;
                                    break;
                                default:
                                    menuOptionDto.Link = MenuOptionRow.Path;
                                    break;
                            }

                            menuOptionDto.ParentId = MenuOptionRow.ModuleId.ToString();

                            listSub.Add(menuOptionDto);
                        }



                    }

                    addNewMenu.SubItems = listSub;
                    if (addNewMenu.SubItems.Count > 0)
                    {

                        Menu.Add(addNewMenu);
                    }
                }


                return Ok(Result<List<MenuDto>>.Success(Menu, MessageCodes.AllSuccessfully()));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok(Result<List<MenuDto>>.Fail("No puede cargar el menu", "504"));
           
            }

        }
 
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            try
            {
                var DataSave = await RepForms.GetById(id);

                var mapperOut = _mapper.Map<FormDto>(DataSave);

                return Ok(Result<FormDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
            }
            catch (Exception e)
            {  
                Console.WriteLine(e);
                return Ok(Result<List<MenuDto>>.Fail("No puede cargar el formulario", "504"));
            }
           
        }
        

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepForms.GetById(id);

            Data.IsActive = false;

            await RepForms.Update(Data);

            var save = await RepForms.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<FormDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<FormDto>(Data);

            return Ok(Result<FormDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] FormDto _UpdateDto)
        {

            var mapper = _mapper.Map<Form>(_UpdateDto);
            
            mapper.IsActive = true;
            var result = await RepForms.Update(mapper);

            var DataSave = await RepForms.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<FormDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<FormDto>(result);

            return Ok(Result<FormDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
