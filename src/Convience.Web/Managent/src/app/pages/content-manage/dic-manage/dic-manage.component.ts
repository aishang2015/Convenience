import { Component, OnInit, ViewChild } from '@angular/core';
import { DicData } from '../model/dicdata';
import { DicType } from '../model/dictype';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DicDataService } from 'src/app/business/dicdata.service';
import { DicTypeService } from 'src/app/business/dictype.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-dic-manage',
  templateUrl: './dic-manage.component.html',
  styleUrls: ['./dic-manage.component.less']
})
export class DicManageComponent implements OnInit {

  @ViewChild("dicTypeTpl", { static: true })
  dicTypeTpl;

  @ViewChild("dicDataTpl", { static: true })
  dicDataTpl;

  dicTypes: DicType[] = [];

  dicDatas: DicData[] = [];

  currentDicTypeId;

  currentDicDataId;

  editDicTypeForm: FormGroup = new FormGroup({});

  editDicDataForm: FormGroup = new FormGroup({});

  modal: NzModalRef;

  selectedDicType;

  tableHeader = '';

  constructor(
    private _formBuilder: FormBuilder,
    private _modalService: NzModalService,
    private _messageService: NzMessageService,
    private _dicdataService: DicDataService,
    private _dictypeService: DicTypeService
  ) { }

  ngOnInit(): void {
    this.initDataTypes();
  }

  initDataTypes() {
    this._dictypeService.getList().subscribe((result: any) => {
      this.dicTypes = result;
    });
  }

  addDicType() {
    this.currentDicTypeId = null;
    this.editDicTypeForm = this._formBuilder.group({
      code: [null, [Validators.required, Validators.maxLength(15)]],
      name: [null, [Validators.required, Validators.maxLength(15)]],
      sort: [null, [Validators.required]]
    });
    this.modal = this._modalService.create({
      nzTitle: '添加字典类型',
      nzContent: this.dicTypeTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  editDicType(id) {
    this._dictypeService.get(id).subscribe((result: any) => {
      this.currentDicTypeId = id;
      this.editDicTypeForm = this._formBuilder.group({
        code: [result.code, [Validators.required, Validators.maxLength(15)]],
        name: [result.name, [Validators.required, Validators.maxLength(15)]],
        sort: [result.sort, [Validators.required]]
      });
      this.modal = this._modalService.create({
        nzTitle: '添加字典类型',
        nzContent: this.dicTypeTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });

  }

  removeDicType(id) {
    this._modalService.confirm({
      nzTitle: '是否删除?',
      nzContent: null,
      nzOnOk: () => {
        this._dictypeService.delete(id).subscribe(result => {
          this._messageService.success("删除成功！");
          this.initDataTypes();
          if(id == this.selectedDicType?.id){
            this.dicDatas.splice(0,this.dicDatas.length);
            this.tableHeader = null;
            this.selectedDicType = null;
          }
        });
      },
    });
  }

  viewDicData(item) {
    this._dicdataService.getList(item.id).subscribe((result: any) => {
      this.dicDatas = result;
      this.tableHeader = item.name;
      this.selectedDicType = item;
    });
  }

  refreshDicData() {
    this._dicdataService.getList(this.selectedDicType.id).subscribe((result: any) => {
      this.dicDatas = result;
    });
  }

  submitDicTypeEdit() {
    for (const i in this.editDicTypeForm.controls) {
      this.editDicTypeForm.controls[i].markAsDirty();
      this.editDicTypeForm.controls[i].updateValueAndValidity();
    }
    if (this.editDicTypeForm.valid) {
      let model = new DicType();
      model.code = this.editDicTypeForm.value['code'];
      model.name = this.editDicTypeForm.value['name'];
      model.sort = this.editDicTypeForm.value['sort'];

      if (this.currentDicTypeId) {
        model.id = this.currentDicTypeId;
        this._dictypeService.update(model).subscribe(result => {
          this._messageService.success("修改成功！");
          this.initDataTypes();
          this.modal.close();
        });
      } else {
        this._dictypeService.add(model).subscribe(result => {
          this._messageService.success("添加成功！");
          this.initDataTypes();
          this.modal.close();
        });
      }
    }
  }

  addDicData() {
    this.currentDicDataId = null;
    this.editDicDataForm = this._formBuilder.group({
      name: [null, [Validators.required, Validators.maxLength(15)]],
      sort: [null, [Validators.required]]
    });
    this.modal = this._modalService.create({
      nzTitle: '添加字典数据',
      nzContent: this.dicDataTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  editDicData(id) {
    this._dicdataService.get(id).subscribe((result: any) => {
      this.currentDicDataId = id;
      this.editDicDataForm = this._formBuilder.group({
        name: [result.name, [Validators.required, Validators.maxLength(15)]],
        sort: [result.sort, [Validators.required]]
      });
      this.modal = this._modalService.create({
        nzTitle: '编辑字典数据',
        nzContent: this.dicDataTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });
  }

  removeDicData(id) {
    this._modalService.confirm({
      nzTitle: '是否删除?',
      nzContent: null,
      nzOnOk: () => {
        this._dicdataService.delete(id).subscribe(result => {
          this._messageService.success("删除成功！");
          this.refreshDicData();
        });
      },
    });
  }

  submitDicDataEdit() {
    for (const i in this.editDicDataForm.controls) {
      this.editDicDataForm.controls[i].markAsDirty();
      this.editDicDataForm.controls[i].updateValueAndValidity();
    }
    if (this.editDicDataForm.valid) {
      let model = new DicData();
      model.name = this.editDicDataForm.value['name'];
      model.sort = this.editDicDataForm.value['sort'];
      model.dicTypeId = this.selectedDicType.id;

      if (this.currentDicDataId) {
        model.id = this.currentDicDataId;
        this._dicdataService.update(model).subscribe(result => {
          this._messageService.success("修改成功！");
          this.refreshDicData();
          this.modal.close();
        });
      } else {
        this._dicdataService.add(model).subscribe(result => {
          this._messageService.success("添加成功！");
          this.refreshDicData();
          this.modal.close();
        });
      }
    }
  }

  cancel() {
    this.modal.close();
  }
}
