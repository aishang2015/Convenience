import { Component, OnInit, ViewChild, Renderer2 } from '@angular/core';
import { NzModalRef, NzModalService, NzMessageService } from 'ng-zorro-antd';
import { WorkflowInstance } from '../model/workflowInstance';
import { WorkflowGroupTreeComponent } from '../workflow-group-tree/workflow-group-tree.component';
import { WorkflowService } from 'src/app/services/workflow.service';
import { WorkFlow } from '../model/workflow';
import { WorkflowInstanceService } from 'src/app/services/workflow-instance.service';
import { WorkflowFormService } from 'src/app/services/workflow-form.service';
import { WorkFlowForm } from '../model/workflowForm';
import { WorkFlowFormControl } from '../model/workFlowFormControl';
import { WorkflowInstanceValue } from '../model/workflowInstanceValue';

@Component({
  selector: 'app-my-flow',
  templateUrl: './my-flow.component.html',
  styleUrls: ['./my-flow.component.scss']
})
export class MyFlowComponent implements OnInit {

  @ViewChild('tree', { static: true })
  workflowGroupTree: WorkflowGroupTreeComponent;

  @ViewChild('contentTpl', { static: true })
  wfTypeTpl;

  @ViewChild('formTpl', { static: true })
  _formTpl;

  // 工作流实例数据
  data: WorkflowInstance[] = [];

  // 表单设计数据
  private _formData: WorkFlowForm = new WorkFlowForm();

  // 表单控件数据
  formControlList: WorkFlowFormControl[] = [];

  // 控件值
  controlValues: { [key: string]: string; } = {};

  page: number = 1;
  size: number = 10;
  total: number = 0;

  wfpage: number = 1;
  wfsize: number = 10;
  wftotal: number = 0;

  _nzModal: NzModalRef;

  _checkedData: WorkflowInstance;

  constructor(
    private _renderer: Renderer2,
    private _modalService: NzModalService,
    private _messageService: NzMessageService,
    private _workflowService: WorkflowService,
    private _formService: WorkflowFormService,
    private _workflowInstanceService: WorkflowInstanceService) { }

  ngOnInit(): void {
    this.refresh();
  }

  add() {
    this._nzModal = this._modalService.create({
      nzTitle: '选择工作流类型',
      nzContent: this.wfTypeTpl,
      nzFooter: null,
      nzMaskClosable: false,
      nzWidth: document.body.clientWidth * 0.8
    });
  }

  refresh() {
    this._workflowInstanceService.getInstances(this.page, this.size)
      .subscribe((result: any) => {
        this.data = result.data;
        this.total = result.count;
      });
  }

  // 查看内容
  viewForm(data) {
    this._checkedData = data;
    this._formService.get(data.workFlowId).subscribe((result: any) => {
      this._formData = result.formResult;
      this.formControlList = result.formControlResults;

      // 初始化表单区域状态
      //this._renderer.setStyle(this._formArea.nativeElement, 'height', `${this._formData.height}px`);
      //this._renderer.setStyle(this._formArea.nativeElement, 'width', `${this._formData.width}px`);
      //this._renderer.setStyle(this._formArea.nativeElement, 'background-color', this._formData.background);

      this.controlValues = {};
      this._workflowInstanceService.getControlValue(data.id).subscribe((result: any) => {
        result.forEach(element => {
          this.controlValues[element.formControlId] = element.value;
        });
      });

      this._nzModal = this._modalService.create({
        nzTitle: '编辑内容',
        nzContent: this._formTpl,
        nzFooter: null,
        nzMaskClosable: false,
        nzWidth: document.body.clientWidth * 0.8
      });

      let ele = document.getElementById('form-area');
      this._renderer.setStyle(ele, 'height', `${this._formData.height}px`);
      this._renderer.setStyle(ele, 'width', `${this._formData.width}px`);
      this._renderer.setStyle(ele, 'background-color', this._formData.background);
    });
  }

  // 查看流程
  viewflow(data) {
  }

  cancel() {
    this._nzModal.close();
  }

  pageChange() {
    this.refresh();
  }

  sizeChange() {
    this.page = 1;
    this.refresh();
  }

  checkedWfTypeId;

  wfdata: WorkFlow[] = [];

  nodeChecked(event) {
    if (event) {
      this.checkedWfTypeId = event;
      this.loadWf();
    }
  }

  loadWf() {
    if (this.checkedWfTypeId) {
      this._workflowService.getList(this.wfpage, this.wfsize, this.checkedWfTypeId).subscribe(
        (result: any) => {
          this.wfdata = result.data;
          this.wftotal = result.count;
        }
      )
    }
  }

  wfPageChange() {
    this.loadWf();
  }

  wfSizeChange() {
    this.page = 1;
    this.loadWf();
  }

  startFlow(workflowId) {
    this._workflowInstanceService.createInstance(workflowId).subscribe(result => {
      this._messageService.success('发起成功');
      this._nzModal.close();
      this.refresh();
    });
  }

  saveData() {
    let values: WorkflowInstanceValue[] = [];
    for (let key in this.controlValues) {
      values.push({
        formControlId: key,
        value: this.controlValues[key]
      });
    }
    this._workflowInstanceService.saveControlValues({
      workFlowInstanceId: this._checkedData.id,
      values: values
    }).subscribe(result => {
      this._messageService.success('保存成功');
      this.controlValues = {};
      this._nzModal.close();
    });
  }

  getPx(dis) {
    return `${dis}px`;
  }

  getState(state) {
    let result;
    switch (state) {
      case 1:
        result = '未提交';
        break;
      case 2:
        result = '流转中';
        break;
      case 3:
        result = '退回';
        break;
      default:
        result = '结束';
        break;
    }
    return result;
  }

}
