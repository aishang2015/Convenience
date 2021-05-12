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

  // 后端实体
  getBackEntity(entityName: string, properties: { type; property; }[]) {
    let propertyString = '';
    properties.forEach(element => {
      propertyString += propertyString ? `

        public ${this.getCsharpType(element.type)} ${this.upperFirstCharacter(element.property)} { get; set; }` :
        `public ${this.getCsharpType(element.type)} ${this.upperFirstCharacter(element.property)} { get; set; }`;
    });
    return `using Convience.EntityFrameWork.Infrastructure;

namespace Convience.Entity.Entity
{
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
      propertyString += propertyString ? `

        public ${this.getCsharpType(element.type)} ${this.upperFirstCharacter(element.property)} { get; set; }` :
        `public ${this.getCsharpType(element.type)} ${this.upperFirstCharacter(element.property)} { get; set; }`;
    });
    return `namespace Convience.Model.Models
{
    public class ${this.upperFirstCharacter(entityName)}ViewModel
    {
        ${propertyString}
    }
    
    public class ${this.upperFirstCharacter(entityName)}ResultModel
    {
        ${propertyString}
    }
    
    public class ${this.upperFirstCharacter(entityName)}QueryModel : PageSortQueryModel
    {
        ${propertyString}
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

  getBackService(entityName: string, properties: { type; property; }[]) {
    let camel = this.upperFirstCharacter(entityName);
    let lower = this.lowerFirstCharacter(entityName);

    var sortFieldDic = '';
    properties.forEach(p => {
      sortFieldDic += sortFieldDic ? `
            sortFieldDic["${this.lowerFirstCharacter(p.property)}"] = t => t.${this.upperFirstCharacter(p.property)};` :
        `sortFieldDic["${this.lowerFirstCharacter(p.property)}"] = t => t.${this.upperFirstCharacter(p.property)};`
    })


    return `using Convience.EntityFrameWork.Repositories;
using Convience.Util.Extension;
using Convience.Model.Models;
using Convience.Entity.Entity;
using Convience.Entity.Data;
using AutoMapper;

using System.Linq.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service
{
    public interface I${camel}Service : IBaseService
    {
        Task<${camel}ResultModel> GetByIdAsync(int id);

        PagingResultModel<${camel}ResultModel> Get${camel}s(${camel}QueryModel query);

        Task Add${camel}Async(${camel}ViewModel model);

        Task Update${camel}Async(${camel}ViewModel model);

        Task Delete${camel}Async(int id);
    }

    public class ${camel}Service : BaseService , I${camel}Service
    {
        private readonly IRepository<${camel}> _${lower}Repository;
        
        private readonly SystemIdentityDbUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ${camel}Service(IRepository<${camel}> ${lower}Repository,
          SystemIdentityDbUnitOfWork unitOfWork,
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
        
        public PagingResultModel<${camel}ResultModel> Get${camel}s(${camel}QueryModel queryModel)
        {
            // 构造查询
            var query = from ${lower} in _${lower}Repository.Get(false)
                        select ${lower};
            
            // 构造排序
            var sortFieldDic = new Dictionary<string, Expression<Func<${camel}, object>>>();
            ${sortFieldDic}            
            query = GetOrderQuery(query, queryModel.Sort, queryModel.Order, sortFieldDic);            
                        
            // 取得数据
            var skip = queryModel.Size * (queryModel.Page - 1);
            var result = query.Skip(skip).Take(queryModel.Size).ToList();
            return new PagingResultModel<${camel}ResultModel>
            {
                Count = query.Count(),
                Data = _mapper.Map<List<${camel}ResultModel>>(result)
            };
        }

        public async Task Add${camel}Async(${camel}ViewModel model)
        {
            var ${camel} = _mapper.Map<${camel}>(model);
            await _${lower}Repository.AddAsync(${camel});
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete${camel}Async(int id)
        {
            await _${lower}Repository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task Update${camel}Async(${camel}ViewModel model)
        {
            var entity = await _${lower}Repository.GetAsync(model.Id);
            _mapper.Map(model, entity);
            await _unitOfWork.SaveAsync();
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
using Convience.Service;
using Convience.Model.Models;

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
        public IActionResult Get([FromQuery]${camel}QueryModel ${lower}Query)
        {
            var result = _${lower}Service.Get${camel}s(${lower}Query);
            return Ok(result);
        }

        [HttpDelete]
        [Permission("${camel}Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _${lower}Service.Delete${camel}Async(id);
            return Ok();
        }

        [HttpPost]
        [Permission("${camel}Add")]
        public async Task<IActionResult> Add(${camel}ViewModel ${lower}ViewModel)
        {
            await _${lower}Service.Add${camel}Async(${lower}ViewModel);
            return Ok();
        }

        [HttpPatch]
        [Permission("${camel}Update")]
        public async Task<IActionResult> Update(${camel}ViewModel ${lower}ViewModel)
        {
            await _${lower}Service.Update${camel}Async(${lower}ViewModel);
            return Ok();
        }
    }
}`;
  }

  getFrontModel(entityName: string, properties: { type; property; }[]) {
    let propertyString = '';
    properties.forEach(element => {
      propertyString += propertyString ? `
    ${this.lowerFirstCharacter(element.property)}?: ${this.getTypeScriptType(element.type)};` :
        `${this.lowerFirstCharacter(element.property)}?: ${this.getTypeScriptType(element.type)};`;
    });
    return `export interface ${this.upperFirstCharacter(entityName)} {
    ${this.lowerFirstCharacter(propertyString)}
}`;
  }

  getFrontService(entityName: string, properties: { property; }[]) {
    let camel = this.upperFirstCharacter(entityName);
    let lower = this.lowerFirstCharacter(entityName);


    let tds = '';
    properties.forEach(element => {
      let property = this.lowerFirstCharacter(element.property);
      tds += `uri += searchObj.${property} ? \`&&${property}=\${searchObj.${property}}\` : '';
    `;
    });

    let result =
      `
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UriConstant } from '../core/configs/uri-constant';

@Injectable({
  providedIn: 'root'
})
export class ${camel}Service {

  constructor(
    private _httpClient: HttpClient,
    private _uriConstant: UriConstant) { }

  get(id) {
    return this._httpClient.get(\`\${this._uriConstant.${camel}Uri}?id=\${id}\`);
  }

  getList(page, size, sort, order, searchObj) {
    let uri = \`\${this._uriConstant.${camel}Uri}/list?page=\${page}&&size=\${size}&&sort=\${sort}&&order=\${order}\`;
    ${tds}
    return this._httpClient.get(uri);
  }

  delete(id) {
    return this._httpClient.delete(\`\${this._uriConstant.${camel}Uri}?id=\${id}\`);
  }

  update(${lower}) {
    return this._httpClient.patch(this._uriConstant.${camel}Uri, ${lower});
  }

  add(${lower}) {
    return this._httpClient.post(this._uriConstant.${camel}Uri, ${lower});
  }
}
`
    return result;
  }

  getFrontHtml(entityName: string, properties: { property; }[]) {
    let camel = this.upperFirstCharacter(entityName);

    let ths = '';
    properties.forEach(element => {
      ths += `<th nzAlign="center" nzColumnKey="${this.lowerFirstCharacter(element.property)}" [nzSortFn]="true" [nzSortPriority]="true">${element.property}</th>
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
<!--搜索表单-->
<nz-card [nzSize]="'small'">
    <form nz-form [nzLayout]="'inline'" [formGroup]="searchForm" (ngSubmit)="submitSearch()">    
        ${items}
        <nz-form-item>
            <nz-form-control>
                <button nz-button nzType="primary">搜索</button>
                <button nz-button type="reset" (click)="reset()">重置</button>
            </nz-form-control>
        </nz-form-item>
    </form>
</nz-card>

<!--数据表格-->
<nz-card [nzSize]="'small'">
    <div>
        <button nz-button class="mr-10" (click)="add()" *canOperate="'add${camel}Btn'">
            <i nz-icon nzType="plus"></i>添加</button>
        <button nz-button class="mr-10" (click)="initData()"><i nz-icon nzType="sync"></i>刷新</button>
    </div>
    <div class="mt-10">
        <nz-table #dataTable nzSize="middle" [nzData]="data" nzShowPagination="false" nzFrontPagination="false"
            nzBordered="true" (nzQueryParams)="onQueryParamsChange($event)" [nzScroll]="{ x: '2460px' }">
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

<!--编辑表单-->
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

    let setSearchObject = '';
    properties.forEach(element => {
      setSearchObject += `_searchObj.${this.lowerFirstCharacter(element.property)} = this.searchForm.values["${this.lowerFirstCharacter(element.property)}"];
      `;
    });


    let result = `
export class ${camel}Component implements OnInit {

    // 页面大小
    public size: number = 10;
    
    // 页码
    public page: number = 1;

    // 数据量
    public total: number = 0;

    // 搜索表单
    public searchForm: FormGroup = new FormGroup({});
    
    // 编辑表单
    public editForm: FormGroup = new FormGroup({});
 
    // 表格数据
    public data: ${camel}[] = [];  

    // 模态框
    public modal: NzModalRef;

    // 表格排序功能
    private _sortArray: string[] = [];
  
    // 表格排序功能
    private _orderArray: string[] = [];

    // 搜索参数
    private _searchObj : any = {};

    @ViewChild('contentTpl', { static: true })
    contentTpl;

    constructor(
        private _${lower}Service:${camel}Service,
        private _messageService: NzmessageService,
        private _modalService: NzModalService,
        private _formBuilder: FormBuilder) { }

    ngOnInit(): void {
      this.initSearchForm();
      this.initData();
    }

    // 初始化搜索表单
    initSearchForm(){
      this.searchForm = this._formBuilder.group({
        ${efs}
      });
    }

    // 点击搜索
    submitSearch() {    
      ${setSearchObject}
      this.initData();
    }

    // 初始化页面数据
    initData() { 
      this._${lower}Service.getList(this.page, this.size, this._sortArray, this._orderArray, this._searchObj)
        .subscribe(result => {
          this.data = result['data'];
          this.total = result['count'];
        });
    }

    // 创建新数据
    add(){
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

    // 编辑数据
    edit(id) {
        this._${lower}Service.get(id).subscribe((result: any) => {
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

    // 删除数据
    remove(id) {
      this._modalService.confirm({
        nzTitle: '是否删除该?',
        nzContent: null,
        nzOnOk: () => {
          this._${lower}Service.delete(id).subscribe(result => {
            this._messageService.success("删除成功！");
            this.initData();
          });
        },
      });
    }

    // 提交编辑
    submitEdit(){
      for (const i in this.editForm.controls) {
        this.editForm.controls[i].markAsDirty();
        this.editForm.controls[i].updateValueAndValidity();
      }
      if (this.editForm.valid) {
        let ${lower} = new ${camel}();
        ${fse}
        
        if (${lower}.id) {
          this._${lower}Service.update(${lower}).subscribe(result => {
            this._messageService.success("修改成功！");
            this.initData();
            this.modal.close();
          });
        } else {
          this._${lower}Service.add(${lower}).subscribe(result => {
            this._messageService.success("添加成功！");
            this.initData();
            this.modal.close();
          });
        }
      }
    }    

    pageChange() {
      this.initData();
    }
  
    sizeChange() {
      this.page = 1;
      this.initData();
    }

    cancel() {
      this.modal.close();
    }
   
    // 排序发生变化
    public onQueryParamsChange(params: NzTableQueryParams) {
      let currentSort = params.sort.filter(s => s.value != null);

      // 移除了排序字段
      if (this._sortArray.length > currentSort.length) {
        let removedField = this._sortArray.find(f => currentSort.find(c => c.key == f) == null);
        let removeIndex = this._sortArray.findIndex(f => f == removedField);

        // 移除元素
        this._sortArray.splice(removeIndex, 1);
        this._orderArray.splice(removeIndex, 1);
      } else if (this._sortArray.length < currentSort.length) {

        // 添加了排序字段
        let newField = currentSort.find(c => this._sortArray.find(f => f == c.key) == null);
        this._sortArray.push(newField.key);
        this._orderArray.push(newField.value);
      } else {
        for (let s of currentSort) {
          let index = this._sortArray.findIndex(f => f == s.key);
          this._orderArray[index] = s.value;
        }
      }
      this.initData();
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
      case 'double':
        result = 'double';
        break;
      case 'decimal':
        result = 'decimal';
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
      case 'double':
        result = 'number';
        break;
      case 'decimal':
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
