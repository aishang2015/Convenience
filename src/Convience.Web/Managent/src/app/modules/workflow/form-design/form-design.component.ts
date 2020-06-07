import { Component, OnInit, Renderer2, ViewChild } from '@angular/core';
import * as jp from 'jsplumb';

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

  private _jsPlumb = jp.jsPlumb;
  private _jsPlumbInstance;

  // 拖拽的节点类型key
  private _draggedKey;

  constructor(private _renderer: Renderer2) { }

  ngOnInit(): void {
    this.initGraph();
  }

  // 初始化流程图
  initGraph() {
    this._jsPlumbInstance = this._jsPlumb.getInstance({
      DragOptions: { cursor: 'move', zIndex: 2000 },
      Container: 'formArea'
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

    let ele;

    switch (this._draggedKey) {
      case 1:
        ele = this._label;
        break;
      case 2:
        ele = this._input;
        break;
      case 3:
        break;
      case 4:
        break;
      case 5:
        break;
      case 6:
        break;
      case 7:
        break;
    }

    let eleRect = ele.nativeElement.getBoundingClientRect();
    let newEle = ele.nativeElement.cloneNode(true);
    x = x - eleRect.width / 2;
    y = y - eleRect.height / 2;

    this._renderer.setStyle(newEle, 'opacity', '1');
    this._renderer.setStyle(newEle, 'top', `${y}px`);
    this._renderer.setStyle(newEle, 'left', `${x}px`);
    this._renderer.appendChild(this._formArea.nativeElement, newEle);
    this._jsPlumbInstance.draggable(newEle);
  }

}
