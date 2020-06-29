import { Component, OnInit, ViewChild } from '@angular/core';
import { DicData } from '../model/dicdata';
import { DicType } from '../model/dictype';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzModalRef, NzModalService, NzMessageService } from 'ng-zorro-antd';
import { DicDataService } from 'src/app/business/dicdata.service';
import { DicTypeService } from 'src/app/business/dictype.service';

@Component({
  selector: 'app-dic-manage',
  templateUrl: './dic-manage.component.html',
  styleUrls: ['./dic-manage.component.scss']
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
    private formBuilder: FormBuilder,
    private modalService: NzModalService,
    private messageService: NzMessageService,
    private dicdataService: DicDataService,
    private dictypeService: DicTypeService
  ) { }

  ngOnInit(): void {
    this.initDataTypes();
  }

  initDataTypes() {
    this.dictypeService.getList().subscribe((result: any) => {
      this.dicTypes = result;
    });
  }

  addDicType() {
    this.currentDicTypeId = null;
    this.editDicTypeForm = this.formBuilder.group({
      code: [null, [Validators.required, Validators.maxLength(15)]],
      name: [null, [Validators.required, Validators.maxLength(15)]],
      sort: [null, [Validators.required]]
    });
    this.modal = this.modalService.create({
      nzTitle: '添加字典类型',
      nzContent: this.dicTypeTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  editDicType(id) {
    this.dictypeService.get(id).subscribe((result: any) => {
      this.currentDicTypeId = id;
      this.editDicTypeForm = this.formBuilder.group({
        code: [result.code, [Validators.required, Validators.maxLength(15)]],
        name: [result.name, [Validators.required, Validators.maxLength(15)]],
        sort: [result.sort, [Validators.required]]
      });
      this.modal = this.modalService.create({
        nzTitle: '添加字典类型',
        nzContent: this.dicTypeTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });

  }

  removeDicType(id) {
    this.modalService.confirm({
      nzTitle: '是否删除?',
      nzContent: null,
      nzOnOk: () => {
        this.dictypeService.delete(id).subscribe(result => {
          this.messageService.success("删除成功！");
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
    this.dicdataService.getList(item.id).subscribe((result: any) => {
      this.dicDatas = result;
      this.tableHeader = item.name;
      this.selectedDicType = item;
    });
  }

  refreshDicData() {
    this.dicdataService.getList(this.selectedDicType.id).subscribe((result: any) => {
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
        this.dictypeService.update(model).subscribe(result => {
          this.messageService.success("修改成功！");
          this.initDataTypes();
          this.modal.close();
        });
      } else {
        this.dictypeService.add(model).subscribe(result => {
          this.messageService.success("添加成功！");
          this.initDataTypes();
          this.modal.close();
        });
      }
    }
  }

  addDicData() {
    this.currentDicDataId = null;
    this.editDicDataForm = this.formBuilder.group({
      name: [null, [Validators.required, Validators.maxLength(15)]],
      sort: [null, [Validators.required]]
    });
    this.modal = this.modalService.create({
      nzTitle: '添加字典数据',
      nzContent: this.dicDataTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  editDicData(id) {
    this.dicdataService.get(id).subscribe((result: any) => {
      this.currentDicDataId = id;
      this.editDicDataForm = this.formBuilder.group({
        name: [result.name, [Validators.required, Validators.maxLength(15)]],
        sort: [result.sort, [Validators.required]]
      });
      this.modal = this.modalService.create({
        nzTitle: '编辑字典数据',
        nzContent: this.dicDataTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });
  }

  removeDicData(id) {
    this.modalService.confirm({
      nzTitle: '是否删除?',
      nzContent: null,
      nzOnOk: () => {
        this.dicdataService.delete(id).subscribe(result => {
          this.messageService.success("删除成功！");
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
        this.dicdataService.update(model).subscribe(result => {
          this.messageService.success("修改成功！");
          this.refreshDicData();
          this.modal.close();
        });
      } else {
        this.dicdataService.add(model).subscribe(result => {
          this.messageService.success("添加成功！");
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
