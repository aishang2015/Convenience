import { Component, OnInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import * as jp from 'jsplumb';
import { fromEvent } from 'rxjs/internal/observable/fromEvent';
import { ActivatedRoute } from '@angular/router';
import { WorkflowNode } from '../model/workflowNode';
import { WorkflowLink } from '../model/workflowLink';
import { WorkflowFlowService } from 'src/app/services/workflow-flow.service';
import { NzMessageService, NzModalRef, NzModalService } from 'ng-zorro-antd';
import { ThrowStmt } from '@angular/compiler';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { debounce, debounceTime, switchMap } from 'rxjs/operators';
import { UserService } from 'src/app/services/user.service';
import { PositionService } from 'src/app/services/position.service';
import { DepartmentService } from 'src/app/services/department.service';

@Component({
  selector: 'app-flow-design',
  templateUrl: './flow-design.component.html',
  styleUrls: ['./flow-design.component.scss']
})
export class FlowDesignComponent implements OnInit {

  // 流程图
  @ViewChild('flowContainer', { static: true })
  private _flowContainer: ElementRef;

  @ViewChild('selectedBorder', { static: true })
  private _sborder: ElementRef;

  // 节点编辑
  @ViewChild('nodeEditTpl', { static: true })
  private _nodeEditTpl;

  // 编辑节点表单
  workflowNodeEditForm: FormGroup = new FormGroup({});
  connections = [];

  // 节点数据
  private _nodeDataList: WorkflowNode[] = [];
  private _linkDataList: WorkflowLink[] = [];

  private _jsPlumb = jp.jsPlumb;
  private _jsPlumbInstance;
  private _endpointOption: jp.EndpointOptions = {
    maxConnections: 10,
    reattachConnections: true,
    type: 'Dot',
    connector: 'Flowchart',
    isSource: true,
    isTarget: true,
    paintStyle: { fill: 'transparent', stroke: 'transparent', strokeWidth: 1 },
    hoverPaintStyle: { fill: 'rgba(95, 158, 160, 1)', stroke: 'rgba(95, 158, 160, 1)', strokeWidth: 2 },
    connectorStyle: { stroke: 'rgba(102, 96, 255, 0.9)', strokeWidth: 3 },
    connectorHoverStyle: { strokeWidth: 4 },
    connectorOverlays: [["PlainArrow", { location: 1 }]],
  };

  nodes = [
    { key: 'start', name: '开始节点', icon: 'play-circle' },
    { key: 'work', name: '工作节点', icon: 'check-circle' },
    { key: 'end', name: '结束节点', icon: 'stop' }
  ];

  // 拖拽的节点类型key
  private _draggedKey;

  // 点击选中的节点
  private _checkedNode = null;

  // 工作流ID
  private _workflowId = null;
  workflowName = null;

  // 加载中
  isLoading = false;

  // 用户
  userSearchChange$ = new BehaviorSubject('');
  userList = [];

  // 用户
  positionSearchChange$ = new BehaviorSubject('');
  positionList = [];

  // 用户
  departmentSearchChange$ = new BehaviorSubject('');
  departmentList = [];

  // 模态框
  nzModal: NzModalRef;

  constructor(
    private _renderer: Renderer2,
    private _route: ActivatedRoute,
    private _flowService: WorkflowFlowService,
    private _messageService: NzMessageService,
    private _formBuilder: FormBuilder,
    private _userService: UserService,
    private _positionService: PositionService,
    private _departmentService: DepartmentService,
    private _modalServce: NzModalService) { }

  ngOnInit(): void {

    this._workflowId = this._route.snapshot.paramMap.get('id')?.trim();
    this.workflowName = this._route.snapshot.paramMap.get('name')?.trim();

    this.listenKeyboard();
    this.initGraph();
    this.initData();

    // 初始化搜索事件
    this.userSearchChange$.asObservable().pipe(debounceTime(500)).subscribe(value => {
      this._userService.getUserDic(value).subscribe((result: any) => {
        this.userList = result;
        this.isLoading = false;
      });
    });
    this.positionSearchChange$.asObservable().pipe(debounceTime(500)).subscribe(value => {
      this._positionService.getDic(value).subscribe((result: any) => {
        this.positionList = result;
        this.isLoading = false;
      });
    });
    this.departmentSearchChange$.asObservable().pipe(debounceTime(500)).subscribe(value => {
      this._departmentService.getDic(value).subscribe((result: any) => {
        this.departmentList = result;
        this.isLoading = false;
      });
    });
  }

  // 初始化流程图
  initGraph() {

    // 创建实例
    this._jsPlumbInstance = this._jsPlumb.getInstance({
      DragOptions: { cursor: 'move', zIndex: 2000 },
      Container: 'flowContainer'
    });

    // 绑定点击
    this._jsPlumbInstance.bind('click', (conn, orignalEvent) => {
      this._jsPlumbInstance.deleteConnection(conn);
    });
  }

  // 监听键盘
  listenKeyboard() {
    fromEvent(window, 'keydown').subscribe((event: any) => {
      if (this._checkedNode) {
        if (event.key == 'Delete') {
          this._jsPlumbInstance.remove(this._checkedNode);
          this._nodeDataList = this._nodeDataList.filter(d => d.domId != this._checkedNode.id);
        }
      }
    });
  }

  // 初始化数据
  initData() {
    this._flowService.get(this._workflowId).subscribe((result: any) => {
      this._nodeDataList = result.workFlowNodeResults ? result.workFlowNodeResults : [];
      this._linkDataList = result.workFlowLinkResults ? result.workFlowLinkResults : [];

      this._nodeDataList.forEach(nodeData => {

        switch (nodeData.nodeType) {
          case 0:
            this.addStartNode(nodeData.domId, nodeData.left, nodeData.top, nodeData.name);
            break;
          case 1:
            this.addWorkNode(nodeData.domId, nodeData.left, nodeData.top, nodeData.name);
            break;
          case 99:
            this.addEndNode(nodeData.domId, nodeData.left, nodeData.top, nodeData.name);
            break;
        }

      });
      this._linkDataList.forEach(linkData => {
        this._jsPlumbInstance.connect({
          source: linkData.sourceId,
          target: linkData.targetId,
          anchor: 'Continuous'
        });
      });
    });
  }


  addStartNode(id, x, y, title = "开始节点") {

    this.addNode(id, x, y, title);

    this._endpointOption.maxConnections = 1;
    this._endpointOption.isSource = true;
    this._endpointOption.isTarget = false;

    // 配置源
    this._jsPlumbInstance.makeSource(id, {
      anchor: 'Continuous',
      allowLoopback: false,
      filter: (event, element) => {
        return event.target.classList.contains('connectable');
      }
    }, this._endpointOption);
  }

  addWorkNode(id, x, y, title = '工作节点') {

    this.addNode(id, x, y, title, true);

    this._endpointOption.maxConnections = 10;
    this._endpointOption.isSource = true;
    this._endpointOption.isTarget = true;

    // 配置源
    this._jsPlumbInstance.makeSource(id, {
      anchor: 'Continuous',
      allowLoopback: false,
      filter: (event, element) => {
        return event.target.classList.contains('connectable');
      }
    }, this._endpointOption);

    // 配置目标
    this._jsPlumbInstance.makeTarget(id, {
      anchor: 'Continuous',
      allowLoopback: false,
      filter: (event, element) => {
        return event.target.classList.contains('connectable');
      }
    }, this._endpointOption);
  }

  addEndNode(id, x, y, title = '结束节点') {

    this.addNode(id, x, y, title);

    this._endpointOption.maxConnections = 100;
    this._endpointOption.isSource = false;
    this._endpointOption.isTarget = true;

    // 配置源
    this._jsPlumbInstance.makeTarget(id, {
      anchor: 'Continuous',
      allowLoopback: false,
      filter: (event, element) => {
        return event.target.classList.contains('connectable');
      }
    }, this._endpointOption);
  }


  addNode(id, x, y, title, hasDblClickEvent = false) {

    // 节点
    let node = this._renderer.createElement('div');
    this._renderer.setStyle(node, 'top', `${y}px`);
    this._renderer.setStyle(node, 'left', `${x}px`);
    this._renderer.addClass(node, 'node');
    this._renderer.setAttribute(node, 'id', id);

    // 设置节点事件
    this._renderer.listen(node, 'mousedown', event => {
      this._checkedNode = node;

      // 绑定四个元素作为border的目的是为了以后修改为调整大小的瞄点
      this._renderer.setStyle(this._sborder.nativeElement, 'display', 'block');
      this._renderer.appendChild(node, this._sborder.nativeElement);
    });

    // 双击编辑节点事件
    if (hasDblClickEvent) {
      this._renderer.listen(node, 'dblclick', event => {
        let data = this._nodeDataList.find(d => d.domId == node.id);

        if (data) {
          let idArray = [];
          if (data.handlers) {
            this.userList = [];
            data.handlers?.split(',').forEach(id => {
              idArray.push(Number.parseInt(id));
            })
            this.initUserSelectData(idArray);
          }

          if (data.position) {
            this.positionList = [];
            this.initPositionSelectData(data.position);
          }

          if (data.department) {
            this.departmentList = [];
            this.initDepartmentSelectData(data.department);
          }

          this.connections = this._jsPlumbInstance.getAllConnections().filter(element => {
            return element.sourceId == node.id;
          });

          this.workflowNodeEditForm = this._formBuilder.group({
            id: data.domId,
            name: [data.name, [Validators.required, Validators.maxLength(15)]],
            handleMode: [data.handleMode, [Validators.required, Validators.pattern('^[12345]$')]],
            handlers: [idArray, [Validators.required]],
            position: [Number.parseInt(data.position), [Validators.required]],
            department: [Number.parseInt(data.department), [Validators.required]]
          });

          this.connections.forEach(connection => {
            this.workflowNodeEditForm.addControl(connection.targetId, new FormControl(null))
          });

          this.nzModal = this._modalServce.create({
            nzTitle: '编辑节点信息',
            nzContent: this._nodeEditTpl,
            nzFooter: null,
            nzMaskClosable: false,
            nzWidth: 800
          });
        }
      });
    }

    // 拼接节点到流程图
    this._renderer.appendChild(this._flowContainer.nativeElement, node);

    // 设置节点连线区域
    let iconArea = this._renderer.createElement('div');
    this._renderer.addClass(iconArea, 'connectable');
    // let newEl = icon.nativeElement.cloneNode(true);
    // this._renderer.setStyle(newEl, 'display', 'inline');
    // this._renderer.appendChild(iconArea, newEl);
    this._renderer.appendChild(node, iconArea);

    // 设置节点拖拽区域
    let draggableArea = this._renderer.createElement('div');
    this._renderer.addClass(draggableArea, 'draggable');
    let titleEl = this._renderer.createText(title);
    this._renderer.appendChild(draggableArea, titleEl);
    this._renderer.appendChild(node, draggableArea);

    // 设施元素在流程图中可拖拽
    this._jsPlumbInstance.draggable(node, {
      filter: '.draggable',
      filterExclude: false,
      drag: (event) => {
        let data = this._nodeDataList.find(d => d.domId == this._checkedNode.id);
        data.top = Number.parseInt(event.el.style.top);
        data.left = Number.parseInt(event.el.style.left);
      }
    });
  }

  // cdk的drag和drop
  // drop(event) {
  //   console.log(event);
  //   console.log(event.previousContainer === event.container);
  //   if (event.isPointerOverContainer) {
  //     this.addNode(event.distance.x, event.distance.y);
  //   }
  // }

  // // 禁止任何元素进入
  //noReturnPredicate() {
  //  return false;
  //}

  back() {
    history.go(-1);
  }

  save() {
    this._linkDataList = [];
    this._jsPlumbInstance.getAllConnections().forEach(element => {
      this._linkDataList.push({
        sourceId: element.sourceId,
        targetId: element.targetId,
      })
    });

    this._flowService.addOrUpdate({
      workFlowId: Number.parseInt(this._workflowId),
      workFlowLinkViewModels: this._linkDataList,
      workFlowNodeViewModels: this._nodeDataList
    }).subscribe(result => {
      this._messageService.success('修改成功！');
    })

  }

  onDragStart(event, key) {
    //event.target.style.background = 'red';
    this._draggedKey = key;
  }

  onDragOver(event) {
    event.preventDefault();
  }

  dropZone(event) {
    event.preventDefault();

    // 取得offsetx时，如果有子元素会冒泡，将子元素的offset设置
    let rect = event.currentTarget.getBoundingClientRect();
    let x = event.clientX - rect.left - 100;
    let y = event.clientY - rect.top - 25;
    let id = `node${this.randomKey()}`;

    // 保存节点数据
    let nodeData = new WorkflowNode();

    switch (this._draggedKey) {
      case 'start':
        if (this._nodeDataList.find(d => d.nodeType == 0)) {
          this._messageService.error('已经有一个开始节点了！');
        } else {
          this.addStartNode(id, x, y);
          nodeData.name = '开始节点';
          nodeData.nodeType = 0;
        }
        break;
      case 'work':
        this.addWorkNode(id, x, y);
        nodeData.name = '工作节点';
        nodeData.nodeType = 1;
        break;
      case 'end':
        if (this._nodeDataList.find(d => d.nodeType == 99)) {
          this._messageService.error('已经有一个开始节点了！');
        } else {
          this.addEndNode(id, x, y);
          nodeData.name = '结束节点';
          nodeData.nodeType = 99;
        }
        break;
    }

    nodeData.domId = id;
    nodeData.top = y;
    nodeData.left = x;
    this._nodeDataList.push(nodeData);
  }

  // 键入用户关键字搜索
  onSearchUser(value: string): void {
    if (value) {
      this.isLoading = true;
      this.userSearchChange$.next(value);
    }
  }

  // 键入职位关键字搜索
  onSearchPosition(value: string): void {
    if (value) {
      this.isLoading = true;
      this.positionSearchChange$.next(value);
    }
  }

  // 键入职位关键字搜索
  onSearchDepartment(value: string): void {
    if (value) {
      this.isLoading = true;
      this.departmentSearchChange$.next(value);
    }
  }

  // 提交节点编辑结果
  submitWorkflowNodeEdit() {

    this.workflowNodeEditForm.controls['name'].markAsDirty();
    this.workflowNodeEditForm.controls['name'].updateValueAndValidity();

    this.workflowNodeEditForm.controls['handleMode'].markAsDirty();
    this.workflowNodeEditForm.controls['handleMode'].updateValueAndValidity();

    let validResult = false;
    if (this.workflowNodeEditForm.value['handleMode'] == 1) {
      this.workflowNodeEditForm.controls['handlers'].markAsDirty();
      this.workflowNodeEditForm.controls['handlers'].updateValueAndValidity();
      validResult = this.workflowNodeEditForm.controls['name'].valid &&
        this.workflowNodeEditForm.controls['handleMode'].valid &&
        this.workflowNodeEditForm.controls['handlers'].valid;
    } else if (this.workflowNodeEditForm.value['handleMode'] == 2) {
      this.workflowNodeEditForm.controls['position'].markAsDirty();
      this.workflowNodeEditForm.controls['position'].updateValueAndValidity();
      validResult = this.workflowNodeEditForm.controls['name'].valid &&
        this.workflowNodeEditForm.controls['handleMode'].valid &&
        this.workflowNodeEditForm.controls['position'].valid;
    } else if (this.workflowNodeEditForm.value['handleMode'] == 3) {
      this.workflowNodeEditForm.controls['department'].markAsDirty();
      this.workflowNodeEditForm.controls['department'].updateValueAndValidity();
      validResult = this.workflowNodeEditForm.controls['name'].valid &&
        this.workflowNodeEditForm.controls['handleMode'].valid &&
        this.workflowNodeEditForm.controls['department'].valid;
    } else if (this.workflowNodeEditForm.value['handleMode'] == 4) {
      validResult = this.workflowNodeEditForm.controls['name'].valid &&
        this.workflowNodeEditForm.controls['handleMode'].valid
    }

    if (validResult) {
      let data = this._nodeDataList.find(d => d.domId == this.workflowNodeEditForm.value['id']);
      data.name = this.workflowNodeEditForm.value['name'];
      data.handleMode = this.workflowNodeEditForm.value['handleMode'];
      data.handlers = this.workflowNodeEditForm.value['handlers']?.join(',');
      data.position = this.workflowNodeEditForm.value['position'];
      data.department = this.workflowNodeEditForm.value['department'];
      this._checkedNode.childNodes[1].innerText = data.name;
      this.nzModal.close();
    }
  }

  initUserSelectData(handlers) {
    handlers.forEach(handler => {
      this._userService.getUser(handler).subscribe((result: any) => {

        this.userList.push({
          key: result.id,
          value: result.name
        });
      });
    });
  }

  initPositionSelectData(id) {
    this._positionService.getPosition(id).subscribe((result: any) => {
      this.positionList.push({
        key: result.id,
        value: result.name
      });
    });
  }

  initDepartmentSelectData(id) {
    this._departmentService.get(id).subscribe((result: any) => {
      this.departmentList.push({
        key: result.id,
        value: result.name
      });
    });
  }



  cancel() {
    this.nzModal.close();
  }

  randomKey(): number {
    return Date.parse(new Date().toString()) + Math.floor(Math.random() * Math.floor(999));
  }


}
