import { Component, OnInit, ViewChild } from '@angular/core';
import { WorkflowGroupTreeComponent } from '../workflow-group-tree/workflow-group-tree.component';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { WorkflowGroupService } from 'src/app/business/workflow/workflowgroup.service';
import { WorkFlowGroup } from '../model/workflowGroup';
import { WorkflowService } from 'src/app/business/workflow/workflow.service';
import { WorkFlow } from '../model/workflow';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-workflow-manage',
  templateUrl: './workflow-manage.component.html',
  styleUrls: ['./workflow-manage.component.less']
})
export class WorkflowManageComponent implements OnInit {

  @ViewChild('tree', { static: true })
  workflowGroupTree: WorkflowGroupTreeComponent;

  @ViewChild('workflowGroupEditTpl', { static: true })
  workflowGroupEditTpl;

  @ViewChild('workflowEditTpl', { static: true })
  workflowEditTpl;

  @ViewChild('formEditTpl', { static: true })
  formEditTpl;

  @ViewChild('flowEditTpl', { static: true })
  flowEditTpl;

  // 分页器
  size: number = 10;
  page: number = 1;
  total: number = 0;

  // 模态框
  nzModal: NzModalRef;

  // 工作流类别编辑表单
  workflowGroupEditForm: FormGroup = new FormGroup({});

  // 工作流编辑表单
  workflowEditForm: FormGroup = new FormGroup({});

  // 编辑中的工作流类别ID
  editingWorkflowGroupId;

  // 编辑中的工作流ID
  editingWorkflowId;

  // 选中的工作流类别id
  checkedWorkflowGroupId;

  // 工作流数据
  data: WorkFlow[] = [];

  // 编辑中的工作流id
  editWorkflowId;

  // 编辑中的工作流名称
  editWorkflowName;

  constructor(
    private _modalService: NzModalService,
    private _formBuilder: FormBuilder,
    private _messageService: NzMessageService,
    private _workflowGroupService: WorkflowGroupService,
    private _workflowService: WorkflowService,
    private _router: Router) { }

  ngOnInit(): void {
  }

  //#region workflow操作

  refresh() {
    if (this.checkedWorkflowGroupId) {
      this._workflowService.getList(this.page, this.size, this.checkedWorkflowGroupId).subscribe(
        (result: any) => {
          this.data = result.data;
          this.total = result.count;
        }
      )
    }
  }

  add() {
    this.workflowEditForm = this._formBuilder.group({
      workFlowGroup: [{ value: this.workflowGroupTree.selectedNode?.title, disabled: true }],
      name: [null, [Validators.required, Validators.maxLength(15)]],
      describe: [null, [Validators.required, Validators.maxLength(30)]]
    });
    this.nzModal = this._modalService.create({
      nzTitle: '创建工作流',
      nzContent: this.workflowEditTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  edit(id) {
    this._workflowService.get(id).subscribe((result: any) => {
      this.editingWorkflowId = id;
      this.workflowEditForm = this._formBuilder.group({
        workFlowGroup: [{ value: this.workflowGroupTree.selectedNode?.title, disabled: true }],
        name: [result.name, [Validators.required, Validators.maxLength(15)]],
        describe: [result.describe, [Validators.required, Validators.maxLength(30)]]
      });
      this.nzModal = this._modalService.create({
        nzTitle: '创建工作流',
        nzContent: this.workflowEditTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });
  }

  remove(id) {
    this._modalService.confirm({
      nzTitle: '是否删除该工作流?',
      nzContent: null,
      nzOnOk: () => {
        this._workflowService.delete(id).subscribe(result => {
          this._messageService.success('删除成功！');
          this.refresh();
        });
      }
    })
  }

  submitWorkflowEdit() {
    for (const i in this.workflowGroupEditForm.controls) {
      this.workflowGroupEditForm.controls[i].markAsDirty();
      this.workflowGroupEditForm.controls[i].updateValueAndValidity();
    }
    if (this.workflowEditForm.valid) {
      let d = new WorkFlow();
      d.name = this.workflowEditForm.value['name'];
      d.describe = this.workflowEditForm.value['describe'];
      d.workFlowGroupId = Number.parseInt(this.checkedWorkflowGroupId);
      if (this.editingWorkflowId) {
        d.id = this.editingWorkflowId;
        this._workflowService.update(d).subscribe(reuslt => {
          this._messageService.success("修改成功！");
          this.refresh();
          this.nzModal.close();
        });
      } else {
        this._workflowService.add(d).subscribe(reuslt => {
          this._messageService.success("添加成功！");
          this.refresh();
          this.nzModal.close();
        });
      }
    }
  }

  isPublishChange(data) {
    this._workflowService.publish(data.id, !data.isPublish).subscribe((result: any) => {
      data.isPublish = !data.isPublish;
    });
  }

  editFlow(id, name) {
    this.editWorkflowId = id;
    this.editWorkflowName = name;
    this._modalService.create({
      nzTitle: "编辑表单",
      nzContent: this.flowEditTpl,
      nzFooter: null,
      nzWidth: "90%"
    });
  }

  editForm(id, name) {
    this.editWorkflowId = id;
    this.editWorkflowName = name;
    this._modalService.create({
      nzTitle: "编辑表单",
      nzContent: this.formEditTpl,
      nzFooter: null,
      nzWidth: "90%"
    });
  }

  pageChange() {
    this.refresh();
  }

  sizeChange() {
    this.page = 1;
    this.refresh();
  }

  //#endregion


  //#region  workflowgroup 操作

  nodeChecked(event) {
    this.checkedWorkflowGroupId = event;
    if (event) {
      this.refresh();
    }
  }

  addWorkFlowGroup() {
    this.workflowGroupEditForm = this._formBuilder.group({
      upWorkFlowGroup: [this.workflowGroupTree.selectedNode?.key],
      name: [null, [Validators.required, Validators.maxLength(15)]],
      sort: [null, Validators.required]
    });
    this.nzModal = this._modalService.create({
      nzTitle: '添加分类',
      nzContent: this.workflowGroupEditTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  updateWorkFlowGroup() {
    let id = this.workflowGroupTree.selectedNode?.key?.toString();
    this._workflowGroupService.get(id).subscribe((result: any) => {

      this.editingWorkflowGroupId = id;

      let upId = this.workflowGroupTree.data.find(d => d.id == result.id)?.upId;
      this.workflowGroupEditForm = this._formBuilder.group({
        upWorkFlowGroup: [{ value: Number(upId), disabled: true }],
        name: [result.name, [Validators.required, Validators.maxLength(15)]],
        sort: [result.sort, Validators.required]
      });

      this.nzModal = this._modalService.create({
        nzTitle: '修改分类',
        nzContent: this.workflowGroupEditTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });
  }

  deleteWorkFlowGroup() {
    this._modalService.confirm({
      nzTitle: '是否删除该分类?',
      nzContent: null,
      nzOnOk: () => {
        let id = this.workflowGroupTree.selectedNode?.key?.toString();
        this._workflowGroupService.delete(id).subscribe(result => {
          this._messageService.success('成功删除');
          this.workflowGroupTree.initNodes();
        });
      }
    })
  }

  submitWorkflowGroupEdit() {
    for (const i in this.workflowGroupEditForm.controls) {
      this.workflowGroupEditForm.controls[i].markAsDirty();
      this.workflowGroupEditForm.controls[i].updateValueAndValidity();
    }
    if (this.workflowGroupEditForm.valid) {
      let d = new WorkFlowGroup();
      d.upId = this.workflowGroupEditForm.value['upWorkFlowGroup'];
      d.name = this.workflowGroupEditForm.value['name'];
      d.sort = this.workflowGroupEditForm.value['sort'];
      if (this.editingWorkflowGroupId) {
        d.id = this.editingWorkflowGroupId;
        this._workflowGroupService.update(d).subscribe(reuslt => {
          this._messageService.success("修改成功！");
          this.workflowGroupTree.initNodes();
          this.nzModal.close();
        });
      } else {
        this._workflowGroupService.add(d).subscribe(reuslt => {
          this._messageService.success("添加成功！");
          this.workflowGroupTree.initNodes();
          this.nzModal.close();
        });
      }
    }
  }

  //#endregion

  cancel() {
    this.nzModal.close();
  }

}
