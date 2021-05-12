import { Component, OnInit, Renderer2, ViewChild, ElementRef, Input } from '@angular/core';
import * as jp from 'jsplumb';
import { fromEvent } from 'rxjs';
import { WorkFlowFormControl, ControlTypeEnum } from '../model/workFlowFormControl';
import { ActivatedRoute } from '@angular/router';
import { WorkflowFormService } from 'src/app/business/workflow/workflow-form.service';
import { WorkFlowForm } from '../model/workflowForm';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-form-design',
  templateUrl: './form-design.component.html',
  styleUrls: ['./form-design.component.less']
})
export class FormDesignComponent implements OnInit {

  @ViewChild('formArea', { static: true })
  _formArea;

  @ViewChild('label', { static: true })
  _label;
  @ViewChild('input', { static: true })
  _input;
  @ViewChild('select', { static: true })
  _select;
  @ViewChild('numberInput', { static: true })
  _numberInput;
  @ViewChild('datePicker', { static: true })
  _datePicker;
  @ViewChild('timePicker', { static: true })
  _timePicker;
  @ViewChild('multiLineInput', { static: true })
  _multiLineInput;

  isloading = false;

  // 选中框
  @ViewChild('selectedBorder', { static: true })
  private _sborder: ElementRef;

  // 节点数据
  private _nodeDataList: WorkFlowFormControl[] = [];

  // 表单设计数据
  formData: WorkFlowForm = new WorkFlowForm();

  // 选中节点的数据
  checkedNodeData: WorkFlowFormControl = null;

  private _jsPlumb = jp.jsPlumb;
  private _jsPlumbInstance;

  // 拖拽的节点类型key
  private _draggedKey;

  // 点击选中的节点
  checkedNode = null;

  // 字号
  fontsize = [];

  // 编辑select的选项
  inputOption = null;

  // 工作流ID
  @Input()
  workflowId = null;

  @Input()
  workflowName = null;

  constructor(private _renderer: Renderer2,
    private _route: ActivatedRoute,
    private _formService: WorkflowFormService,
    private _messageService: NzMessageService) { }

  ngOnInit(): void {

    this.initGraph();
    this.initKeyboardListening();
    this.initFormAreaClick();
    this.initData();

    for (let i = 1; i <= 32; i++) {
      this.fontsize.push(i * 2);
    }
  }

  // 初始化流程图
  initGraph() {
    this._jsPlumbInstance = this._jsPlumb.getInstance({
      DragOptions: { cursor: 'move', zIndex: 2000 },
      Container: 'formArea'
    });
  }

  // 键盘事件
  initKeyboardListening() {
    fromEvent(window, 'keydown').subscribe((event: any) => {
      if (this.checkedNode) {
        if (event.key == 'ArrowDown') {
          event.preventDefault();
          let distance = Number.parseInt(this.checkedNode.style.top.replace('px', ''));
          this._renderer.setStyle(this.checkedNode, 'top', `${distance + 3}px`);
          this.checkedNodeData.top = distance + 3;
        } else if (event.key == 'ArrowUp') {
          event.preventDefault();
          let distance = Number.parseInt(this.checkedNode.style.top.substring(0, this.checkedNode.style.top.length - 2));
          this._renderer.setStyle(this.checkedNode, 'top', `${distance - 3}px`);
          this.checkedNodeData.top = distance - 3;
        } else if (event.key == 'ArrowLeft') {
          event.preventDefault();
          let distance = Number.parseInt(this.checkedNode.style.left.substring(0, this.checkedNode.style.left.length - 2));
          this._renderer.setStyle(this.checkedNode, 'left', `${distance - 3}px`);
          this.checkedNodeData.left = distance - 3;
        } else if (event.key == 'ArrowRight') {
          event.preventDefault();
          let distance = Number.parseInt(this.checkedNode.style.left.substring(0, this.checkedNode.style.left.length - 2));
          this._renderer.setStyle(this.checkedNode, 'left', `${distance + 3}px`);
          this.checkedNodeData.left = distance + 3;
        }
        if (event.key == 'Delete') {
          this._jsPlumbInstance.remove(this.checkedNode);
          this._nodeDataList = this._nodeDataList.filter(data => data.domId != this.checkedNode.id);
          this.checkedNode = null;
        }
      }

      if (event.ctrlKey && event.key == 's') {
        event.preventDefault();
        this.save();
      }

    });
  }

  // 初始化流程图区域点击
  initFormAreaClick() {
    this._renderer.listen(this._formArea.nativeElement, 'mousedown', event => {
      this.checkedNode = null;
      this.checkedNodeData = null;
      this._renderer.setStyle(this._sborder.nativeElement, 'display', 'none');
    });
  }

  initData() {
    this._formService.get(this.workflowId).subscribe((result: any) => {
      this.formData = result.formResult;
      this._nodeDataList = result.formControlResults;
      if (!this.formData) {
        this.formData = new WorkFlowForm();
        this.formData.width = 595;
        this.formData.height = 842;
        this.formData.background = 'black';
      }

      // 初始化表单区域状态
      this._renderer.setStyle(this._formArea.nativeElement, 'height', `${this.formData.height}px`);
      this._renderer.setStyle(this._formArea.nativeElement, 'width', `${this.formData.width}px`);
      this._renderer.setStyle(this._formArea.nativeElement, 'background-color', this.formData.background);

      if (!this._nodeDataList) {
        this._nodeDataList = new Array<WorkFlowFormControl>();
      }

      // 初始化各个元素状态
      this.checkedNode = null;
      for (let node of this._formArea.nativeElement.childNodes) {
        this._renderer.removeChild(this._formArea.nativeElement, node);
      }
      this._nodeDataList.forEach(nodeData => {
        this.initNode(nodeData);
      });
    });
  }

  // 根据数据绘制节点
  initNode(node: WorkFlowFormControl) {

    node.optionList = node.options ? node.options.split(',') : [];

    let id = node.domId;

    let ele;

    switch (node.controlType) {
      case 1:
        ele = this._label;
        break;
      case 2:
        ele = this._input;
        break;
      case 3:
        ele = this._select;
        break;
      case 4:
        ele = this._numberInput;
        break;
      case 5:
        ele = this._datePicker;
        break;
      case 6:
        ele = this._timePicker;
        break;
      case 7:
        ele = this._multiLineInput;
        this._renderer.setAttribute(ele.nativeElement.firstChild, 'rows', `${node.line}`);
        break;
    }

    let newEle = ele.nativeElement.cloneNode(true);

    this._renderer.setAttribute(newEle, 'id', id);
    this._renderer.setStyle(newEle, 'z-index', '0');
    this._renderer.setStyle(newEle, 'opacity', '1');
    this._renderer.setStyle(newEle, 'top', `${node.top}px`);
    this._renderer.setStyle(newEle, 'left', `${node.left}px`);
    if (node.controlType != 1) {
      this._renderer.setStyle(newEle.firstChild, 'width', `${node.width}px`);
      this._renderer.setStyle(newEle, 'width', `${node.width}px`);
    } else {
      newEle.firstChild.innerText = node.content;
    }
    this._renderer.setStyle(newEle, 'font-size', `${node.fontSize}px`);

    // 设置节点事件
    this._renderer.listen(newEle, 'mousedown', event => {
      this.checkedNode = newEle;

      this.checkedNodeData = this._nodeDataList.find(data => data.domId == newEle.id);

      let rect = newEle.getBoundingClientRect();
      this._renderer.setStyle(this._sborder.nativeElement, 'width', `${rect.width}px`);
      this._renderer.setStyle(this._sborder.nativeElement, 'height', `${rect.height}px`);
      this._renderer.setStyle(this._sborder.nativeElement, 'display', 'block');
      this._renderer.appendChild(newEle, this._sborder.nativeElement);
    });

    this._renderer.appendChild(this._formArea.nativeElement, newEle);
    this._jsPlumbInstance.draggable(newEle, {
      containment: true,
      drag: (event) => {
        this.checkedNodeData.top = Number.parseInt(event.el.style.top.replace('px', ''));
        this.checkedNodeData.left = Number.parseInt(event.el.style.left.replace('px', ''));
      }
    });
  }


  onDragStart(event, key) {
    this._draggedKey = key;
  }

  onDragOver(event) {
    event.preventDefault();
  }

  // drop事件
  dropZone(event) {
    event.preventDefault();
    let rect = event.currentTarget.getBoundingClientRect();
    let x = event.clientX - rect.left;
    let y = event.clientY - rect.top;
    let id = `control${this.randomKey()}`;

    let ele;
    let control = new WorkFlowFormControl();

    switch (this._draggedKey) {
      case 1:
        ele = this._label;
        control.content = '标签';
        break;
      case 2:
        ele = this._input;
        break;
      case 3:
        ele = this._select;
        control.optionList = [];
        break;
      case 4:
        ele = this._numberInput;
        break;
      case 5:
        ele = this._datePicker;
        break;
      case 6:
        ele = this._timePicker;
        break;
      case 7:
        ele = this._multiLineInput;
        control.line = 4;
        break;
    }

    let eleRect = ele.nativeElement.getBoundingClientRect();
    let newEle = ele.nativeElement.cloneNode(true);
    x = Math.floor(x - eleRect.width / 2);
    y = Math.floor(y - eleRect.height / 2);

    this._renderer.setAttribute(newEle, 'id', id);
    this._renderer.setStyle(newEle, 'z-index', '0');
    this._renderer.setStyle(newEle, 'opacity', '1');
    this._renderer.setStyle(newEle, 'top', `${y}px`);
    this._renderer.setStyle(newEle, 'left', `${x}px`);

    // 设置节点事件
    this._renderer.listen(newEle, 'mousedown', event => {
      this.checkedNode = newEle;

      // 把选项数据分割成list
      this.checkedNodeData = this._nodeDataList.find(data => data.domId == newEle.id);
      this.checkedNodeData.optionList = this.checkedNodeData.options ? this.checkedNodeData.options.split(',') : [];

      // 点击选中的效果
      let rect = newEle.getBoundingClientRect();
      this._renderer.setStyle(this._sborder.nativeElement, 'width', `${rect.width}px`);
      this._renderer.setStyle(this._sborder.nativeElement, 'height', `${rect.height}px`);
      this._renderer.setStyle(this._sborder.nativeElement, 'display', 'block');
      this._renderer.appendChild(newEle, this._sborder.nativeElement);
    });

    this._renderer.appendChild(this._formArea.nativeElement, newEle);
    this._jsPlumbInstance.draggable(newEle, {
      containment: true,
      drag: (event) => {
        this.checkedNodeData.top = Math.floor(event.el.style.top.replace('px', ''));
        this.checkedNodeData.left = Math.floor(event.el.style.left.replace('px', ''));
      }
    });

    control.domId = id;
    control.controlType = this._draggedKey;
    control.width = 200;
    control.fontSize = 14;
    control.left = x;
    control.top = y;
    this._nodeDataList.push(control);
  }

  formHeightChanged(event) {
    this._renderer.setStyle(this._formArea.nativeElement, 'height', `${event}px`);
  }

  formWidthChanged(event) {
    this._renderer.setStyle(this._formArea.nativeElement, 'width', `${event}px`);
  }

  reloadFormSize(event) {
    this.formData.width = 595;
    this.formData.height = 842;
    this._renderer.setStyle(this._formArea.nativeElement, 'width', `${595}px`);
    this._renderer.setStyle(this._formArea.nativeElement, 'height', `${842}px`);
  }

  formBackGroundChanged(event) {
    this._renderer.setStyle(this._formArea.nativeElement, 'background-color', event);
  }

  fontSizeChange(event) {
    this._renderer.setStyle(this.checkedNode.firstChild, 'font-size', `${event}px`);
    setTimeout(() => {
      let rect = this.checkedNode.getBoundingClientRect();
      this._renderer.setStyle(this._sborder.nativeElement, 'height', `${rect.height}px`);
      this._renderer.setStyle(this._sborder.nativeElement, 'width', `${rect.width}px`);
    }, 300);
  }

  widthChanged(event) {
    this._renderer.setStyle(this.checkedNode.firstChild, 'width', `${event}px`);
    this._renderer.setStyle(this._sborder.nativeElement, 'width', `${event}px`);
  }

  topChanged(event) {
    this._renderer.setStyle(this.checkedNode, 'top', `${event}px`);
  }

  leftChanged(event) {
    this._renderer.setStyle(this.checkedNode, 'left', `${event}px`);
  }

  contentChanged(event) {
    this.checkedNode.firstChild.innerText = event;
    let rect = this.checkedNode.getBoundingClientRect();
    this._renderer.setStyle(this._sborder.nativeElement, 'width', `${rect.width}px`);
  }

  lineChanged(event) {
    this._renderer.setAttribute(this.checkedNode.firstChild, 'rows', event);
    let rect = this.checkedNode.getBoundingClientRect();
    this._renderer.setStyle(this._sborder.nativeElement, 'height', `${rect.height}px`);
  }

  optionEnter(event) {
    this.checkedNodeData.optionList.push(this.inputOption);
    this.inputOption = null;
  }
  removeOption(event) {
    this.checkedNodeData.optionList = this.checkedNodeData.optionList.filter(item => item !== event.currentTarget.innerText);
  }

  setVertialPosition(flg) {
    switch (flg) {
      case 'left':
        this.leftChanged(0);
        this.checkedNodeData.left = 0;
        break;
      case 'middle':
        let rect = this.checkedNode.getBoundingClientRect();
        let left = Math.round((this.formData.width - rect.width) / 2);
        this.leftChanged(left);
        this.checkedNodeData.left = left;
        break;
      case 'right':
        let rrect = this.checkedNode.getBoundingClientRect();
        this.leftChanged((this.formData.width - rrect.width));
        this.checkedNodeData.left = this.formData.width - rrect.width;
        break;
    }
  }

  back() {
    history.go(-1);
  }

  save() {

    this.isloading = true;

    // 把选项list拼接成string
    for (let data of this._nodeDataList) {
      data.options = data.optionList?.join(',');
    }

    this._formService.addOrUpdate({
      workFlowId: Number.parseInt(this.workflowId),
      formViewModel: this.formData,
      formControlViewModels: this._nodeDataList
    }).subscribe(result => {
      this._messageService.success('保存成功！');
      this.initData();
      this.isloading = false;
    }, error => {
      this.isloading = false;
    });
  }

  formatterPiex = (value: number) => {
    return `${value} px`
  };
  parserPiex = (value: string) => value.replace(' px', '');

  randomKey(): number {
    return Date.parse(new Date().toString()) + Math.floor(Math.random() * Math.floor(999));
  }
}
