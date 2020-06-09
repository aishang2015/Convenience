import { Component, OnInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import * as jp from 'jsplumb';
import { fromEvent } from 'rxjs';
import { WorkFlowFormControl, ControlTypeEnum } from '../model/workFlowFormControl';

@Component({
  selector: 'app-form-design',
  templateUrl: './form-design.component.html',
  styleUrls: ['./form-design.component.scss']
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

  // 选中框
  @ViewChild('selectedBorder', { static: true })
  private _sborder: ElementRef;

  // 节点数据
  private _nodeDataList: WorkFlowFormControl[] = [];

  // 选中节点的数据
  checkedNodeData = null;

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

  // 表单区域大小
  formHeight = 842;
  formWidth = 595;
  formBackground = 'black';

  constructor(private _renderer: Renderer2) { }

  ngOnInit(): void {
    this.initGraph();
    this.listenKeyboard();
    this.initFormAreaClick()

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

  // 选中元素的键盘事件
  listenKeyboard() {
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
          this.checkedNode = null;
        }
      }
    });
  }

  // 初始化点击
  initFormAreaClick() {
    this._renderer.listen(this._formArea.nativeElement, 'mousedown', event => {
      this.checkedNode = null;
      this.checkedNodeData = null;
      this._renderer.setStyle(this._sborder.nativeElement, 'display', 'none');
    });
  }

  onDragStart(event, key) {
    this._draggedKey = key;
  }

  onDragOver(event) {
    event.preventDefault();
  }

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
    x = x - eleRect.width / 2;
    y = y - eleRect.height / 2;

    this._renderer.setAttribute(newEle, 'id', id);
    this._renderer.setStyle(newEle, 'opacity', '1');
    this._renderer.setStyle(newEle, 'top', `${y}px`);
    this._renderer.setStyle(newEle, 'left', `${x}px`);

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
    this.formWidth = 595;
    this.formHeight = 842;
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
        break;
      case 'middle':
        let rect = this.checkedNode.getBoundingClientRect();
        this.leftChanged((this.formWidth - rect.width) / 2);
        break;
      case 'right':
        let rrect = this.checkedNode.getBoundingClientRect();
        this.leftChanged((this.formWidth - rrect.width));
        break;
    }
  }

  formatterPiex = (value: number) => {
    return `${value} px`
  };
  parserPiex = (value: string) => value.replace(' px', '');

  randomKey(): number {
    return Date.parse(new Date().toString()) + Math.floor(Math.random() * Math.floor(999));
  }
}
