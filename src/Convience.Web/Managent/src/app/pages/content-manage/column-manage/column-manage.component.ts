import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzModalRef, NzMessageService, NzModalService, NzFormatEmitEvent, NzTreeNode, NzTreeNodeOptions } from 'ng-zorro-antd';
import { Column } from '../model/column';
import { ColumnService } from 'src/app/business/column.service';

@Component({
  selector: 'app-column-manage',
  templateUrl: './column-manage.component.html',
  styleUrls: ['./column-manage.component.scss']
})
export class ColumnManageComponent implements OnInit {

  nodes: NzTreeNodeOptions[] = [];

  data: Column[] = [];

  currentId?: number = null;

  @ViewChild('contentTpl', { static: true })
  contentTpl;

  editForm: FormGroup = new FormGroup({});

  selectedNode: NzTreeNode;

  displayData: Column[] = [];

  modal: NzModalRef;

  constructor(
    private messageService: NzMessageService,
    private modalService: NzModalService,
    private formBuilder: FormBuilder,
    private columnService: ColumnService) { }

  ngOnInit(): void {
    this.initNodes();
  }

  add() {
    this.currentId = null;
    this.editForm = this.formBuilder.group({
      upColumn: [{ value: this.getUpperSelectColumn(), disabled: true }],
      name: [null, [Validators.required, Validators.maxLength(15)]],
      sort: [null, [Validators.required]]
    });
    this.modal = this.modalService.create({
      nzTitle: '添加栏目',
      nzContent: this.contentTpl,
      nzFooter: null,
      nzMaskClosable: false,
    });
  }

  initNodes() {
    let nodes: NzTreeNodeOptions[] = [{ title: '文章栏目管理', key: null, icon: 'database', expanded: true, children: [] }];
    this.columnService.getAll().subscribe((result: any) => {
      this.data = result;
      this.makeNodes(null, nodes[0], this.data);
      this.nodes = nodes;
      this.displayData = this.data.filter(column => column.upId == this.selectedNode?.key);
    });
  }

  makeNodes(upId, node, columns: Column[]) {
    var cs = columns.filter(column => column.upId == upId);
    cs.forEach(column => {
      let data = { title: column.name, key: column.id, icon: 'database', children: [] };
      this.makeNodes(column.id, data, columns);
      node.children.push(data);
    });
  }

  edit(id) {
    this.columnService.get(id).subscribe((result: Column) => {
      this.currentId = result.id;
      this.editForm = this.formBuilder.group({
        upColumn: [{ value: this.getUpperColumnName(result.upId), disabled: true }],
        name: [result.name, [Validators.required, Validators.maxLength(15)]],
        sort: [result.sort, Validators.required]
      });

      this.modal = this.modalService.create({
        nzTitle: '编辑栏目',
        nzContent: this.contentTpl,
        nzFooter: null,
        nzMaskClosable: false,
      });
    });
  }

  remove(id) {
    this.modalService.confirm({
      nzTitle: '是否删除该栏目?',
      nzContent: null,
      nzOnOk: () => {
        this.columnService.delete(id).subscribe(result => {
          this.initNodes();
          this.messageService.success("删除成功！");
        });
      },
    });
  }

  submitEdit() {
    for (const i in this.editForm.controls) {
      this.editForm.controls[i].markAsDirty();
      this.editForm.controls[i].updateValueAndValidity();
    }
    if (this.editForm.valid) {
      let column = new Column();
      column.name = this.editForm.value["name"];
      column.sort = this.editForm.value["sort"];
      column.upId = this.selectedNode?.key?.toString();

      if (this.currentId) {
        column.id = this.currentId;
        this.columnService.update(column).subscribe(result => {
          this.messageService.success("修改成功！");
          this.initNodes();
          this.modal.close();
        });
      } else {
        this.columnService.add(column).subscribe(result => {
          this.messageService.success("添加成功！");
          this.initNodes();
          this.modal.close();
        });
      }
    }
  }

  cancel() {
    this.modal.close();
  }

  treeClick(event: NzFormatEmitEvent) {
    this.selectedNode = event.keys.length > 0 ? event.node : null;
    this.displayData = this.data.filter(column => column.upId == this.selectedNode?.key);
  }

  getUpperSelectColumn() {
    var key = this.selectedNode?.key;
    var selectedColumn = this.data.find(column => column.id.toString() == key);
    return selectedColumn?.name;
  }

  getUpperColumnName(id) {
    return this.data.find(d => d.id == id)?.name;
  }

}
