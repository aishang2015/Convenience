import { Component, OnInit, ViewChild } from '@angular/core';
import { NzModalRef, NzModalService, NzMessageService } from 'ng-zorro-antd';
import { WorkflowInstance } from '../model/workflowInstance';
import { WorkflowGroupTreeComponent } from '../workflow-group-tree/workflow-group-tree.component';
import { WorkflowService } from 'src/app/services/workflow.service';
import { WorkFlow } from '../model/workflow';
import { WorkflowInstanceService } from 'src/app/services/workflow-instance.service';

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

  data: WorkflowInstance[] = [];

  page: number = 1;
  size: number = 10;
  total: number = 0;

  wfpage: number = 1;
  wfsize: number = 10;
  wftotal: number = 0;

  _nzModal: NzModalRef;

  constructor(
    private _modalService: NzModalService,
    private _messageService: NzMessageService,
    private _workflowService: WorkflowService,
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
  viewForm(id) {
  }

  // 查看流程
  viewflow(id) {
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
