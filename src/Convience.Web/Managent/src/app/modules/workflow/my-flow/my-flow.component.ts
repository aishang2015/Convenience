import { Component, OnInit, ViewChild } from '@angular/core';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { WorkflowInstance } from '../model/workflowInstance';
import { WorkflowGroupTreeComponent } from '../workflow-group-tree/workflow-group-tree.component';
import { WorkflowService } from 'src/app/services/workflow.service';
import { WorkFlow } from '../model/workflow';

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
    private _workflowService: WorkflowService) { }

  ngOnInit(): void {
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
  }

  edit(id) {
  }

  remove(id) {
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
    this._nzModal.close();
  }

}
