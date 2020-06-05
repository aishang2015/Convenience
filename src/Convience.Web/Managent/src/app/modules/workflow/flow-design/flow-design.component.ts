import { Component, OnInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import * as jp from 'jsplumb';
import { fromEvent } from 'rxjs/internal/observable/fromEvent';

@Component({
  selector: 'app-flow-design',
  templateUrl: './flow-design.component.html',
  styleUrls: ['./flow-design.component.scss']
})
export class FlowDesignComponent implements OnInit {

  // 流程图
  @ViewChild('flowContainer', { static: true })
  private _flowContainer: ElementRef;

  @ViewChild('lborder', { static: true })
  private _lborder: ElementRef;
  @ViewChild('tborder', { static: true })
  private _tborder: ElementRef;
  @ViewChild('rborder', { static: true })
  private _rborder: ElementRef;
  @ViewChild('bborder', { static: true })
  private _bborder: ElementRef;
  @ViewChild('selectedBorder', { static: true })
  private _sborder: ElementRef;

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

  // 拖拽的节点类型key
  private _draggedKey;

  // 点击选中的节点
  private _checkedNodeId = null;

  constructor(
    private _renderer: Renderer2) { }

  ngOnInit(): void {
    this.listenKeyboard();
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
    this.addNode(x, y, "开始节点");
  }

  addWorkNode(x, y) {
    this.addNode(x, y, "工作节点");
  }

  addEndNode(x, y) {
    this.addNode(x, y, "结束节点");
  }


  addNode(x, y, title) {
    let id = `nodeIndex${this.randomKey()}`;

    // 节点
    let node = this._renderer.createElement('div');
    this._renderer.setStyle(node, 'top', `${y}px`);
    this._renderer.setStyle(node, 'left', `${x}px`);
    this._renderer.addClass(node, 'node');
    this._renderer.setAttribute(node, 'id', id);

    // 设置节点事件
    this._renderer.listen(node, 'click', event => {

      if (this._checkedNodeId != node.id) {
        this._checkedNodeId = node.id

        // 绑定四个元素作为border的目的是为了以后修改为调整大小的瞄点
        this._renderer.setStyle(this._sborder.nativeElement, 'display', 'block');
        this._renderer.appendChild(node, this._sborder.nativeElement);
      } else {
        this._checkedNodeId = null;
        this._renderer.setStyle(this._sborder.nativeElement, 'display', 'none');
      }
    });

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
    let x = event.offsetX - 100;
    let y = event.offsetY - 25;
    switch (this._draggedKey) {
      case 'start':
        this.addStartNode(x, y);
        break;
      case 'work':
        this.addWorkNode(x, y);
        break;
      case 'end':
        this.addEndNode(x, y);
        break;
    }
  }

  randomKey(): number {
    return Date.parse(new Date().toString()) + Math.floor(Math.random() * Math.floor(999));
  }

  listenKeyboard() {
    fromEvent(window, 'keydown').subscribe((event: any) => {
      event.preventDefault();
      if (this._checkedNodeId) {
        let element = this._renderer.selectRootElement(`#${this._checkedNodeId}`);
        if (event.key == 'ArrowDown') {
          element.style.top = element.style.top - 1;
        }
      }
    });
  }

}
