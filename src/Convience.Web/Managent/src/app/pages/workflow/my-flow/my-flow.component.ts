import { Component, OnInit, ViewChild, Renderer2, ElementRef } from '@angular/core';
import { WorkflowInstance } from '../model/workflowInstance';
import { WorkflowGroupTreeComponent } from '../workflow-group-tree/workflow-group-tree.component';
import { WorkflowService } from 'src/app/business/workflow/workflow.service';
import { WorkFlow } from '../model/workflow';
import { WorkflowInstanceService } from 'src/app/business/workflow/workflow-instance.service';
import { WorkflowFormService } from 'src/app/business/workflow/workflow-form.service';
import { WorkFlowForm } from '../model/workflowForm';
import { WorkFlowFormControl } from '../model/workFlowFormControl';
import { WorkflowInstanceValue } from '../model/workflowInstanceValue';
import * as jp from 'jsplumb';
import { WorkflowFlowService } from 'src/app/business/workflow/workflow-flow.service';
import { WorkflowNode } from '../model/workflowNode';
import { WorkflowLink } from '../model/workflowLink';
import { WorkflowInstanceRoute } from '../model/workflowInstanceRoute';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-my-flow',
  templateUrl: './my-flow.component.html',
  styleUrls: ['./my-flow.component.less']
})
export class MyFlowComponent implements OnInit {

  @ViewChild('tree', { static: true })
  workflowGroupTree: WorkflowGroupTreeComponent;

  @ViewChild('contentTpl', { static: true })
  wfTypeTpl;

  @ViewChild('formTpl', { static: true })
  _formTpl;

  @ViewChild('flowTpl', { static: true })
  _flowTpl;

  @ViewChild('flowRouteTpl', { static: true })
  _flowRouteTpl;

  // 工作流实例数据
  data: WorkflowInstance[] = [];

  // 表单设计数据
  private _formData: WorkFlowForm = new WorkFlowForm();

  // 表单控件数据
  formControlList: WorkFlowFormControl[] = [];

  // 节点数据
  nodeDataList: WorkflowNode[] = [];
  linkDataList: WorkflowLink[] = [];

  // 节点处理数据
  routeDataList: WorkflowInstanceRoute[] = [];

  // 控件值
  controlValues: { [key: string]: string; } = {};

  page: number = 1;
  size: number = 10;
  total: number = 0;

  wfpage: number = 1;
  wfsize: number = 10;
  wftotal: number = 0;

  _nzModal: NzModalRef;

  checkedData: WorkflowInstance;

  constructor(
    private _renderer: Renderer2,
    private _modalService: NzModalService,
    private _messageService: NzMessageService,
    private _workflowService: WorkflowService,
    private _formService: WorkflowFormService,
    private _flowService: WorkflowFlowService,
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
    this.checkedData = data;
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
          this.controlValues[element.formControlDomId] = element.value;
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

    this.checkedData = data;
    this._flowService.get(data.workFlowId).subscribe((result: any) => {
      this.nodeDataList = result.workFlowNodeResults ? result.workFlowNodeResults : [];
      this.linkDataList = result.workFlowLinkResults ? result.workFlowLinkResults : [];

      this._modalService.create({
        nzTitle: '查看流程',
        nzContent: this._flowTpl,
        nzFooter: null,
        nzMaskClosable: false,
        nzWidth: document.body.clientWidth * 0.8
      });
      let ele = document.getElementById('flowContainer');
      let jsPlumbInstance: any = jp.jsPlumb.getInstance({
        DragOptions: { cursor: 'move', zIndex: 2000 },
        Container: 'flowContainer'
      });
      let endpointOption: jp.EndpointOptions = {
        maxConnections: 100,
        reattachConnections: true,
        type: 'Dot',
        connector: 'Flowchart',
        isSource: true,
        isTarget: true,
        paintStyle: { fill: 'transparent', stroke: 'transparent', strokeWidth: 1 },
        connectorStyle: { stroke: 'rgba(102, 96, 255, 0.9)', strokeWidth: 3 },
        connectorOverlays: [["PlainArrow", { location: 1 }]],
      };
      // this.nodeDataList.forEach(node => {
      //   jsPlumbInstance.addEndpoint(node.domId, endpointOption);
      //   jsPlumbInstance.makeTarget(node.domId, {});
      //   jsPlumbInstance.makeSource(node.domId, {});
      // });

      setTimeout(() => {
        this.nodeDataList.forEach(node => {

          jsPlumbInstance.makeSource(node.domId, {
            anchor: 'Continuous',
            allowLoopback: false,
            filter: (event, element) => {
              return false;
            }
          }, endpointOption);
          jsPlumbInstance.makeTarget(node.domId, {
            anchor: 'Continuous',
            allowLoopback: false,
            filter: (event, element) => {
              return false;
            }
          }, endpointOption);
        });
        this.linkDataList.forEach(linkData => {
          jsPlumbInstance.connect({
            source: document.getElementById(linkData.sourceId),
            target: document.getElementById(linkData.targetId),
          });
        });
      }, 400);

    });

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
      this._workflowService.getList(this.wfpage, this.wfsize, this.checkedWfTypeId, true).subscribe(
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

  delete(data) {
    this._modalService.confirm({
      nzTitle: '是否删除？',
      nzOnOk: () => {
        this._workflowInstanceService.deleteInstance(data.id).subscribe(result => {
          this._messageService.success('删除成功');
          this.refresh();
        });
      }
    });
  }


  saveData() {
    let values: WorkflowInstanceValue[] = [];
    for (let key in this.controlValues) {
      values.push({
        formControlDomId: key,
        value: this.controlValues[key]
      });
    }
    this._workflowInstanceService.saveControlValues({
      workFlowInstanceId: this.checkedData.id,
      values: values
    }).subscribe(result => {
      this._messageService.success('保存成功');
      this.controlValues = {};
      this._nzModal.close();
    });
  }

  // 提交审批
  submitApprove() {
    this._modalService.confirm({
      nzTitle: '确认内容无误，是否提交？',
      nzOnOk: () => {

        let values: WorkflowInstanceValue[] = [];
        for (let key in this.controlValues) {
          values.push({
            formControlDomId: key,
            value: this.controlValues[key]
          });
        }
        this._workflowInstanceService.saveControlValues({
          workFlowInstanceId: this.checkedData.id,
          values: values
        }).subscribe(result => {
          this._workflowInstanceService.submitInstance({
            workFlowInstanceId: this.checkedData.id,
            isPass: true,
          }).subscribe(result => {
            this._messageService.success('提交成功');
            this.controlValues = {};
            this.refresh();
            this._nzModal.close();
          });
        });
      }
    })
  }

  // 查看流程
  viewRoutes() {
    this._workflowInstanceService.getInstanceRoute(this.checkedData.id).subscribe((result: any) => {
      this.routeDataList = result;
      this._modalService.create({
        nzTitle: '处理过程',
        nzContent: this._flowRouteTpl,
        nzFooter: null,
        nzMaskClosable: false,
        nzWidth: document.body.clientWidth * 0.8
      });
    });
  }

  // 取消流程
  cancelWf() {
    this._modalService.confirm({
      nzTitle: '确认取消流程？',
      nzOnOk: () => {
        this._workflowInstanceService.cancelInstance({
          workFlowInstanceId: this.checkedData.id
        }).subscribe(result => {
          this._messageService.success('操作成功！');
          this.refresh();
          this._nzModal.close();
        });
      }
    })
  }

  print() {

    const printContent = document.getElementById("print-area");
    const WindowPrt = window.open('', '', '');
    WindowPrt.document.write(printContent.innerHTML);
    WindowPrt.document.close();

    WindowPrt.focus();
    WindowPrt.print();
    WindowPrt.close();
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
        result = '已拒绝';
        break;
      case 4:
        result = '已结束';
        break;
      case 5:
        result = '无法进行';
        break;
      case 6:
        result = '已取消';
        break;
    }
    return result;
  }

  getHandleState(state) {
    let result;
    switch (state) {
      case 1:
        result = '未处理';
        break;
      case 2:
        result = '通过';
        break;
      case 3:
        result = '拒绝';
        break;
    }
    return result;

  }

}
