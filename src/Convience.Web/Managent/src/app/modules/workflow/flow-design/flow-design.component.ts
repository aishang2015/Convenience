import { Component, OnInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import * as jp from 'jsplumb';

@Component({
  selector: 'app-flow-design',
  templateUrl: './flow-design.component.html',
  styleUrls: ['./flow-design.component.scss']
})
export class FlowDesignComponent implements OnInit {

  // 流程图
  @ViewChild('flowContainer', { static: true })
  private _flowContainer: ElementRef;

  @ViewChild('start', { static: true })
  private _startIcon: ElementRef;
  @ViewChild('work', { static: true })
  private _workIcon: ElementRef;
  @ViewChild('end', { static: true })
  private _endIcon: ElementRef;

  private _jsPlumb = jp.jsPlumb;
  private _jsPlumbInstance;
  private _endpointOption = {
    maxConnections: 5,
    reattachConnections: true,
    type: 'Dot',
    connector: 'Flowchart',
    isSource: true,
    isTarget: true,
    paintStyle: { fill: 'transparent', stroke: 'transparent', radius: 5, strokeWidth: 1 },
    hoverPaintStyle: { fill: 'rgba(95, 158, 160, 1)', stroke: 'rgba(95, 158, 160, 1)', strokeWidth: 2 },
    connectorStyle: { stroke: 'rgba(102, 96, 255, 0.9)', strokeWidth: 3 },
    connectorHoverStyle: { strokeWidth: 4, cursor: 'pointer' },
    connectorOverlays: [["PlainArrow", { location: 1 }]],
  };

  nodes = [
    { key: 'start', name: '开始节点', icon: 'play-circle' },
    { key: 'work', name: '工作节点', icon: 'check-circle' },
    { key: 'end', name: '结束节点', icon: 'stop' }
  ];

  private _nodeIndex: number = 1;

  private _draggedKey;

  constructor(
    private _renderer: Renderer2) { }

  ngOnInit(): void {
    this.initGraph();
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

  addStartNode(x, y) {
    this.addNode(x, y, 'node', "开始节点", this._startIcon);
  }

  addWorkNode(x, y) {
    this.addNode(x, y, 'node', "工作节点", this._workIcon);
  }

  addEndNode(x, y) {
    this.addNode(x, y, 'node', "结束节点", this._endIcon);
  }


  addNode(x, y, style, title, icon) {
    let id = `nodeIndex${this._nodeIndex++}`;

    // 节点
    let node = this._renderer.createElement('div');
    this._renderer.setStyle(node, 'top', `${y}px`);
    this._renderer.setStyle(node, 'left', `${x}px`);
    this._renderer.addClass(node, style);
    this._renderer.setAttribute(node, 'id', id);

    // 拼接元素
    this._renderer.appendChild(this._flowContainer.nativeElement, node);
    
    // 图标区域
    let iconArea = this._renderer.createElement('div');
    this._renderer.addClass(iconArea, 'connectable');
    // let newEl = icon.nativeElement.cloneNode(true);
    // this._renderer.setStyle(newEl, 'display', 'inline');
    // this._renderer.appendChild(iconArea, newEl);
    this._renderer.appendChild(node, iconArea);

    // 拖拽区域
    let draggableArea = this._renderer.createElement('div');
    this._renderer.addClass(draggableArea, 'draggable');
    // 拖拽区域文字
    let titleEl = this._renderer.createText(title);
    this._renderer.appendChild(draggableArea, titleEl);
    this._renderer.appendChild(node, draggableArea);

    // 配置拖拽
    this._jsPlumbInstance.draggable(node, {
      filter: '.draggable',
      filterExclude: false
    });

    // 配置源
    this._jsPlumbInstance.makeSource(id, {
      anchor: 'Continuous',
      allowLoopback: false,
      filter: (event, element) => {
        return event.target.classList.contains('connectable');
      }
    }, this._endpointOption);

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

  onDragStart(event, key) {
    //event.target.style.background = 'red';
    this._draggedKey = key;
  }

  onDragOver(event) {
    event.preventDefault();
  }

  dropZone(event) {
    event.preventDefault();
    switch (this._draggedKey) {
      case 'start':
        this.addStartNode(event.offsetX - 50, event.offsetY - 50);
        break;
      case 'work':
        this.addWorkNode(event.offsetX - 50, event.offsetY - 50);
        break;
      case 'end':
        this.addEndNode(event.offsetX - 50, event.offsetY - 50);
        break;
    }
  }

}
