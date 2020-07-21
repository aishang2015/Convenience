import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CodeService {

  constructor() { }

  getFileNameList(entityName: string) {
    let result: string[] = [];
    let name = this.upperFirstCharacter(entityName);
    let lower = entityName.toLowerCase();
    result.push(`${name}.cs`);
    result.push(`${name}Configuration.cs`);
    result.push(`${name}Models.cs`);
    result.push(`${name}ViewModelValidator.cs`);
    result.push(`${name}QueryValidator.cs`);
    result.push(`${name}Service.cs`);
    result.push(`${name}Controller.cs`);

    result.push(`${lower}.ts`);
    result.push(`${lower}.service.ts`);
    result.push(`${lower}.components.html`);
    result.push(`${lower}.component.ts`);

    return result;
  }

  getBackEntity(entityName: string, dbcontext: string, properties: { type; property; }[]) {
    let propertyString = '';
    properties.forEach(element => {
      propertyString += `public ${this.getCsharpType(element.type)} ${this.upperFirstCharacter(element.property)} { get; set; }

        `;
    });
    return `using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity
{
    [Entity(DbContextType = typeof(${this.upperFirstCharacter(dbcontext)}))]
    public class ${this.upperFirstCharacter(entityName)}
    {
        ${propertyString}
    }
}`;
  }

  getBackEntityConfig(entityName: string, properties: { property; isRequired; length; }[]) {
    let camel = this.upperFirstCharacter(entityName);
    let builder = '';
    properties.forEach(element => {
      if (element.isRequired) {
        builder += `builder.Property(a => a.${element.property}).IsRequired();
            `;
      }
      if (element.length) {
        builder += `builder.Property(a => a.${element.property}).HasMaxLength(${element.length});
            `;
      }
    });
    return builder ? `using Convience.Entity.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
namespace Convience.Entity.Configurations
{
    public class ${camel}Configuration : IEntityTypeConfiguration<${camel}>
    {
        public void Configure(EntityTypeBuilder<${camel}> builder)
        {
            ${builder}
        }
    }
}`: '';
  }

  getBackModels(entityName: string, properties: { type; property; }[]) {
    let propertyString = '';
    properties.forEach(element => {
      propertyString += `public ${this.getCsharpType(element.type)} ${this.upperFirstCharacter(element.property)} {get;set;}

        `;
    });
    return `namespace Convience.Model.Models
{
    public class ${this.upperFirstCharacter(entityName)}ViewModel
    {
        ${propertyString}
    }
    
    public class ${this.upperFirstCharacter(entityName)}ResultModel : ${this.upperFirstCharacter(entityName)}ViewModel
    {
    }
    
    public class ${this.upperFirstCharacter(entityName)}QueryModel
    {
        public int Page { get; set; }

        public int Size { get; set; }
    }
}`;
  }

  getBackViewModelValidator(entityName: string, properties: { property; isRequired; length; }[]) {
    let camel = this.upperFirstCharacter(entityName);

    let builder = '';
    properties.forEach(element => {
      if (element.isRequired) {
        builder += `
          RuleFor(viewmodel => viewmodel.${element.property}).NotNull().NotEmpty()
            .WithMessage("不能为空!");
            `;
      }
      if (element.length) {
        builder += `
          RuleFor(viewmodel => viewmodel.${element.property}).MaximumLength(${element.length})
            .WithMessage("长度不能超过${element.length}!");
            `;
      }
    });

    return builder ? `using FluentValidation;
    
namespace Convience.Model.Validators
{
    public class ${camel}ViewModelValidator : AbstractValidator<${camel}ViewModel>
    {
        public ${camel}ViewModelValidator()
        {
          ${builder}
        }
    }
}`: '';
  }

  getBackQueryValidator(entityName: string) {
    let camel = this.upperFirstCharacter(entityName);
    return `using FluentValidation;

namespace Convience.Model.Validators
{
    public class ${camel}QueryModelValidator : AbstractValidator<${camel}QueryModel>
    {
        public ${camel}QueryModelValidator()
        {
            RuleFor(viewmodel => viewmodel.Size).Must(size => size == 10 || size == 20 || size == 30 || size == 40).WithMessage("错误的长度！");
        }
    }
}`;
  }

  getBackService(entityName: string, dbcontext: string) {
    let camel = this.upperFirstCharacter(entityName);
    let lower = this.lowerFirstCharacter(entityName);
    return `using Convience.EntityFrameWork.Repositories;
using Convience.Util.Extension;
using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service
{
    public interface I${camel}Service
    {
        Task<${camel}ResultModel> GetByIdAsync(int id);

        (IEnumerable<${camel}ResultModel>, int) Get${camel}s(${camel}QueryModel query);

        Task<bool> Add${camel}Async(${camel}ViewModel model);

        Task<bool> Update${camel}Async(${camel}ViewModel model);

        Task<bool> Delete${camel}Async(int id);
    }

    public class ${camel}Service : I${camel}Service
    {
        private readonly IRepository<${camel}> _${lower}Repository;
        
        private readonly IUnitOfWork<${this.upperFirstCharacter(dbcontext)}> _unitOfWork;

        private readonly IMapper _mapper;

        public ${camel}Service(IRepository<${camel}> ${lower}Repository,
          IUnitOfWork<${this.upperFirstCharacter(dbcontext)}> unitOfWork,
          IMapper mapper)
        {
            _${lower}Repository = ${lower}Repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<${camel}ResultModel> GetByIdAsync(int id)
        {
            var ${camel} = await _${lower}Repository.GetAsync(id);
            return _mapper.Map<${camel}ResultModel>(${camel});
        }
        
        public (IEnumerable<${camel}ResultModel>, int) Get${camel}s(${camel}QueryModel query)
        {
            var where = ExpressionExtension.TrueExpression<${camel}>();
            var ${lower}Query =  _${lower}Repository.Get().Where(@where);
            var skip = query.Size * (query.Page - 1);
            var result = _mapper.Map<IEnumerable<${camel}>,IEnumerable<${camel}ResultModel>>(${lower}Query.Skip(skip).Take(query.Size).ToList());
            return (result, ${lower}Query.Count());
        }

        public async Task<bool> Add${camel}Async(${camel}ViewModel model)
        {
            try
            {
                var ${camel} = _mapper.Map<${camel}>(model);
                await _${lower}Repository.AddAsync(${camel});
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete${camel}Async(int id)
        {
            try
            {
                await _${lower}Repository.RemoveAsync(id);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Update${camel}Async(${camel}ViewModel model)
        {
            try
            {
                var entity = await _${lower}Repository.GetAsync(model.Id);
                _mapper.Map(model, entity);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}`;
  }

  getBackIService(entityName: string) {
    return `using System.Threading.Tasks;
using System.Collections.Generic;

namespace Convience.Service
{

}`;
  }

  getBackController(entityName: string) {
    let camel = this.upperFirstCharacter(entityName);
    let lower = this.lowerFirstCharacter(entityName);
    return `using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Convience.ManagentApi.Infrastructure.Authorization;
using Convience.Fluentvalidation;

using System.Threading.Tasks;

namespace Convience.ManagentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ${camel}Controller : ControllerBase
    {       
        private readonly I${camel}Service _${lower}Service;

        public ${camel}Controller(I${camel}Service ${lower}Service)
        {
            _${lower}Service = ${lower}Service;
        }

        [HttpGet]
        [Permission("${camel}Get")]
        public async Task<IActionResult> GetById(int id)
        {
            var ${camel} = await _${lower}Service.GetByIdAsync(id);
            return Ok(${camel});
        }

        [HttpGet("list")]
        [Permission("${camel}List")]
        public IActionResult Get([FromQuery]${camel}QueryModel ${camel}Query)
        {
            var result = _${lower}Service.Get${camel}s(${camel}Query);
            return Ok(new
            {
                data = result.Item1,
                count = result.Item2
            });
        }

        [HttpDelete]
        [Permission("${camel}Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccess = await _${lower}Service.Delete${camel}Async(id);
            if (!isSuccess)
            {
                return this.BadRequestResult("删除失败!");
            }
            return Ok();
        }

        [HttpPost]
        [Permission("${camel}Add")]
        public async Task<IActionResult> Add(${camel}ViewModel ${camel}ViewModel)
        {
            var isSuccess = await _${lower}Service.Add${camel}Async(${camel}ViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("添加失败!");
            }
            return Ok();
        }

        [HttpPatch]
        [Permission("${camel}Update")]
        public async Task<IActionResult> Update(${camel}ViewModel ${camel}ViewModel)
        {
            var isSuccess = await _${lower}Service.Update${camel}Async(${camel}ViewModel);
            if (!isSuccess)
            {
                return this.BadRequestResult("更新失败!");
            }
            return Ok();
        }
    }
}`;
  }

  getFrontModel(entityName: string, properties: { type; property; }[]) {
    let propertyString = '';
    properties.forEach(element => {
      propertyString += `${this.lowerFirstCharacter(element.property)}?: ${this.getTypeScriptType(element.type)};
    `;
    });
    return `export class ${this.upperFirstCharacter(entityName)} {
    ${this.lowerFirstCharacter(propertyString)}
}`;
  }

  getFrontService(entityName: string) {
    let camel = this.upperFirstCharacter(entityName);
    let lower = this.lowerFirstCharacter(entityName);
    let result =
      `
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/configs/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class ${camel}Service {

  constructor(private httpClient: HttpClient,
    private uriConstant: UriConstant) { }

  get(id) {
    return this.httpClient.get(\`\${this.uriConstant.${camel}Uri}?id=\${id}\`);
  }

  getList(page, size) {
    let uri = \`\${this.uriConstant.${camel}Uri}/list?page=\${page}&&size=\${size}\`;
    return this.httpClient.get(uri);
  }

  delete(id) {
    return this.httpClient.delete(\`\${this.uriConstant.${camel}Uri}?id=\${id}\`);
  }

  update(${lower}) {
    return this.httpClient.patch(this.uriConstant.${camel}Uri, ${lower});
  }

  add(${lower}) {
    return this.httpClient.post(this.uriConstant.${camel}Uri, ${lower});
  }
}
`
    return result;
  }

  getFrontHtml(entityName: string, properties: { property; }[]) {
    let camel = this.upperFirstCharacter(entityName);

    let ths = '';
    properties.forEach(element => {
      ths += `<th nzAlign="center">${element.property}</th>
                    `;
    });
    let tds = '';
    properties.forEach(element => {
      tds += `<td nzAlign="center">{{ data.${this.lowerFirstCharacter(element.property)} }}</td>
                    `;
    });
    let items = '';
    properties.forEach(element => {
      items += `<nz-form-item>
            <nz-form-label [nzSm]="6" [nzXs]="24" [nzFor]="'edit_${this.lowerFirstCharacter(element.property)}'">${element.property}</nz-form-label>
            <nz-form-control [nzSm]="14" [nzXs]="24" nzErrorTip="">
                <input [attr.id]="'edit_${this.lowerFirstCharacter(element.property)}'" formControlName="${this.lowerFirstCharacter(element.property)}" nz-input placeholder="${element.property}"
                    autocomplete="off" />
            </nz-form-control>
        </nz-form-item>
                     `;

    });

    let result = `
<nz-card [nzSize]="'small'">
    <form nz-form [nzLayout]="'inline'" [formGroup]="searchForm" (ngSubmit)="submitSearch()">
        <nz-form-item>
            <nz-form-control>
                <button nz-button nzType="primary">搜索</button>
            </nz-form-control>
        </nz-form-item>
    </form>
</nz-card>

<nz-card [nzSize]="'small'">
    <div>
        <button nz-button class="mr-10" (click)="add()" *canOperate="'add${camel}Btn'">
            <i nz-icon nzType="plus"></i>添加</button>
        <button nz-button class="mr-10" (click)="refresh()"><i nz-icon nzType="sync"></i>刷新</button>
    </div>
    <div class="mt-10">
        <nz-table #dataTable nzSize="middle" [nzData]="data" nzShowPagination="false" nzFrontPagination="false"
            nzBordered="true">
            <thead>
                <tr>
                    <th nzAlign="center" nzWidth="50px">#</th>
                    ${ths}
                    <th nzAlign="center">操作</th>
                </tr>
            </thead>
            <tbody>
                <tr *ngFor="let data of dataTable.data;let i = index">
                    <td nzAlign="center">{{ i + 1 + (page - 1) * size }}</td>
                    ${tds}
                    <td nzAlign="center">
                        <button nz-button nzType="default" nzShape="circle" *canOperate="'update${camel}Btn'"
                            (click)="edit(data.id)" class="mr-10"><i nz-icon nzType="edit"></i></button>
                        <button nz-button nzType="default" nzShape="circle" *canOperate="'delete${camel}Btn'"
                            (click)="remove(data.id)" class="mr-10"><i nz-icon nzType="delete"></i></button>
                    </td>
                </tr>
            </tbody>
        </nz-table>
    </div>
    <div class="mt-10">
        <nz-pagination [(nzPageSize)]="size" [(nzPageIndex)]="page" [nzTotal]="total" nzShowSizeChanger
            nzShowQuickJumper (nzPageIndexChange)="pageChange()" (nzPageSizeChange)="sizeChange()"></nz-pagination>
    </div>
</nz-card>

<ng-template #contentTpl>
    <form nz-form [formGroup]="editForm" (ngSubmit)="submitEdit()">


        ${items}
        <nz-form-item>
            <nz-form-control [nzSpan]="14" [nzOffset]="6">
                <button nz-button nzType="primary" class="mr-10">提交</button>
                <button nz-button type="reset" (click)="cancel()">取消</button>
            </nz-form-control>
        </nz-form-item>
    </form>
</ng-template>

    `;
    return result;
  }

  getFrontTs(entityName: string, properties: { property; }[]) {
    let camel = this.upperFirstCharacter(entityName);
    let lower = this.lowerFirstCharacter(entityName);

    let fs = '';
    properties.forEach(element => {
      fs += `${this.lowerFirstCharacter(element.property)}: [result.${this.lowerFirstCharacter(element.property)}, []],
            `;
    });

    let efs = '';
    properties.forEach(element => {
      efs += `${this.lowerFirstCharacter(element.property)}: [null, []],
            `;
    });

    let fse = '';
    properties.forEach(element => {
      fse += `${lower}.${this.lowerFirstCharacter(element.property)} = this.editForm.values["${this.lowerFirstCharacter(element.property)}"];
            `;
    });


    let result = `
export class ${camel}Component implements OnInit {

    size: number = 10;
    page: number = 1;
    total: number = 0;

    searchForm: FormGroup = new FormGroup({});
    
    editForm: FormGroup = new FormGroup({});

    data: ${camel}[] = [];  

    currentId?: number = null;

    modal: NzModalRef;

    @ViewChild('contentTpl', { static: true })
    contentTpl;

    constructor(
        private _${lower}Service:${camel}Service,
        private _messageService: NzmessageService,
        private _modalService: NzModalService,
        private _formBuilder: FormBuilder) { }

    ngOnInit(): void {
      this.refresh();
    }

    add(){
      this.currentId = null;
      this.editForm = this._formBuilder.group({
        ${efs}
      });
      this.modal = this._modalService.create({
        nzTitle: '添加',
        nzContent: this.contentTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    }

    refresh() { 
      this._${lower}Service.getList(this.page, this.size)
        .subscribe(result => {
          this.data = result['data'];
          this.total = result['count'];
        });
    }

    submitSearch() {
      this.refresh();
    }

    edit(id) {
        this._${lower}Service.get(id).subscribe((result: any) => {
          this.currentId = result.id;
          this.editForm = this._formBuilder.group({
            ${fs}
          });
          this.modal = this._modalService.create({
            nzTitle: '编辑',
            nzContent: this.contentTpl,
            nzFooter: null,
            nzMaskClosable: false,
          });
        });
    }

    remove(id) {
      this._modalService.confirm({
        nzTitle: '是否删除该?',
        nzContent: null,
        nzOnOk: () => {
          this._${lower}Service.delete(id).subscribe(result => {
            this._messageService.success("删除成功！");
            this.refresh();
          });
        },
      });
    }

    submitEdit(){
      for (const i in this.editForm.controls) {
        this.editForm.controls[i].markAsDirty();
        this.editForm.controls[i].updateValueAndValidity();
      }
      if (this.editForm.valid) {
        var ${lower} = new ${camel}();
        ${fse}
        if (this.currentId) {
          ${lower}.id = this.currentId;
          this._${lower}Service.update(${lower}).subscribe(result => {
            this._messageService.success("修改成功！");
            this.refresh();
            this.modal.close();
          });
        } else {
          this._${lower}Service.add(${lower}).subscribe(result => {
            this._messageService.success("添加成功！");
            this.refresh();
            this.modal.close();
          });
        }
      }
    }
    

    pageChange() {
      this.refresh();
    }
  
    sizeChange() {
      this.page = 1;
      this.refresh();
    }

    cancel() {
      this.modal.close();
    }
}

    `;
    return result;
  }


  upperFirstCharacter(str: string) {
    if (str.length > 0) {
      return str[0].toUpperCase() + str.substring(1, str.length);
    }
  }

  lowerFirstCharacter(str: string) {
    if (str.length > 0) {
      return str[0].toLowerCase() + str.substring(1, str.length);
    }
  }

  getCsharpType(type: string) {
    let result = '';
    switch (type) {
      case 'guid':
        result = 'Guid';
        break;
      case 'int':
        result = 'int';
        break;
      case 'string':
        result = 'string';
        break;
      case 'bool':
        result = 'bool';
        break;
      case 'datetime':
        result = 'DateTime';
        break;
    }
    return result;
  }

  getTypeScriptType(type: string) {
    let result = '';
    switch (type) {
      case 'guid':
        result = 'string';
        break;
      case 'int':
        result = 'number';
        break;
      case 'string':
        result = 'string';
        break;
      case 'bool':
        result = 'boolean';
        break;
      case 'datetime':
        result = 'Date';
        break;
    }
    return result;
  }

}
