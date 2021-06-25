import { Component, OnInit, AfterViewInit, AfterViewChecked, ApplicationRef } from '@angular/core';
import { Chart, registerShape } from '@antv/g2';
import { DashboardService } from 'src/app/business/dashboard.service';
import { UriConfig } from 'src/app/configs/uri-config';

const signalR = require("@microsoft/signalr");

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less'],
  providers: [ApplicationRef]
})
export class DashboardComponent implements OnInit, AfterViewInit {

  data = { userCount: 0, roleCount: 0, departmentCount: 0, positionCount: 0 };

  chartArray = [];

  // 仪表盘图对象
  cpuChart;

  // 内存折线图
  memoryChart;

  // 内存折线图数据
  memoryChartData = [
  ];

  // 垃圾回收区域大小饼图
  garbageChart;

  // 垃圾回收区域大小饼图数据
  garbageChartData = [
    { item: '第0代', count: 0, percent: 0 },
    { item: '第1代', count: 0, percent: 0 },
    { item: '第2代', count: 0, percent: 0 },
  ];

  // 应用内存
  workingSet = 0;

  // 垃圾回收堆内存
  gcHeapSize = 0;

  // 垃圾回收代数
  gen0Count = 0;
  gen1Count = 0;
  gen2Count = 0;

  // 线程池相关
  handelThreadPoolThreadCount = 0;
  handelMonitorLockContentionCount = 0;
  threadPoolQueueLength = 0;
  threadPoolCompletedWorkItemCount = 0;

  // 垃圾回收代数空间
  gen0Size = 0;
  gen1Size = 0;
  gen2Size = 0;

  lohSize = 0;
  pohSize = 0;



  constructor(
    private _uriConstant: UriConfig,
    private _dashboardService: DashboardService,
    private appref: ApplicationRef) {
  }

  ngOnInit() {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${this._uriConstant._baseUri}/appState`)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    async function start() {
      try {
        await connection.start();
        console.log("SignalR Connected.");
      } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
      }
    };

    connection.onclose(start);

    connection.on("HandleCpuUsage", data => {
      if (this.cpuChart) {
        const data = [];
        data.push({ value: +Number(data) });
        this.drawCpuChart(this.cpuChart, data);
      }
    });

    connection.on("HandleWorkingSet", data => {
      this.workingSet = data;
      if (this.memoryChart) {
        if (this.memoryChartData.length > 10) {
          this.memoryChartData.shift();
        }
        var myDate = new Date();
        let hh = myDate.getHours()
        let mf = myDate.getMinutes() < 10 ? '0' + myDate.getMinutes()
          : myDate.getMinutes()
        let ss = myDate.getSeconds() < 10 ? '0' + myDate.getSeconds()
          : myDate.getSeconds()
        this.memoryChartData.push({ 'time': `${hh}:${mf}:${ss}`, type: '内存', size: Number(data) });
        this.memoryChart.render();
      }
    });

    connection.on("HandleGCHeapSize", data => {
      this.gcHeapSize = data;
    });

    connection.on("HandelGCCount", (gen, count) => {
      switch (gen) {
        case 0:
          this.gen0Count = count;
          break;
        case 1:
          this.gen1Count = count;
          break;
        case 2:
          this.gen2Count = count;
          break;
      }
    });

    connection.on("HandelThreadPoolThreadCount", data => {
      this.handelThreadPoolThreadCount = data;
    });

    connection.on("HandelMonitorLockContentionCount", data => {
      this.handelMonitorLockContentionCount = data;
    });

    connection.on("ThreadPoolQueueLength", data => {
      this.threadPoolQueueLength = data;
    });

    connection.on("ThreadPoolCompletedWorkItemCount", data => {
      this.threadPoolCompletedWorkItemCount = data;
    });

    connection.on("HandelGcSize", (gen, count) => {
      switch (gen) {
        case 0:
          this.gen0Size = count;
          this.garbageChartData[0].count = count;
          break;
        case 1:
          this.gen1Size = count;
          this.garbageChartData[1].count = count;
          break;
        case 2:
          this.gen2Size = count;
          this.garbageChartData[2].count = count;
          break;
      }

      let fun = (num: number) => {
        let numStr = num.toString()
        let index = numStr.indexOf('.')
        let result = numStr.slice(0, index + 3);
        return Number(result);
      }

      let total = this.garbageChartData[0].count + this.garbageChartData[1].count + this.garbageChartData[2].count;
      this.garbageChartData[0].percent = fun(this.garbageChartData[0].count / total);
      this.garbageChartData[1].percent = fun(this.garbageChartData[1].count / total);
      this.garbageChartData[2].percent = fun(this.garbageChartData[2].count / total);
      this.garbageChart.changeData(this.garbageChartData);
    });

    connection.on("HandeLohSize", data => {
      this.lohSize = data;
    });

    connection.on("HandelPohSize", data => {
      this.pohSize = data;
    });

    // Start the connection.
    start();

    this._dashboardService.get().subscribe((result: any) => this.data = result);
  }

  ngAfterViewInit(): void {
    this.initGarbageChart();
    this.initCpuChart();
    this.initMemoryChart();

    this.initGraph1();
    this.initGraph3();
    this.initGraph5();

    // 初始图像宽度会溢出，通过resize事件触发图标重绘
    setTimeout(() => {
      var myEvent = new Event('resize');
      window.dispatchEvent(myEvent);
    }, 10);
  }

  //#region cpu使用率

  initCpuChart() {
    function creatData() {
      const data = [];
      data.push({ value: +0 });
      return data;
    }

    // 自定义Shape 部分
    registerShape('point', 'pointer', {
      draw(cfg, container) {
        const group = container.addGroup({});
        // 获取极坐标系下画布中心点
        const center = this.parsePoint({ x: 0, y: 0 });
        // 绘制指针
        group.addShape('line', {
          attrs: {
            x1: center.x,
            y1: center.y,
            x2: cfg.x,
            y2: cfg.y,
            stroke: cfg.color,
            lineWidth: 5,
            lineCap: 'round',
          },
        });
        group.addShape('circle', {
          attrs: {
            x: center.x,
            y: center.y,
            r: 9.75,
            stroke: cfg.color,
            lineWidth: 4.5,
            fill: '#fff',
          },
        });
        return group;
      },
    });

    const color = ['#0086FA', '#FFBF00', '#F5222D'];
    const chart = new Chart({
      container: 'c4',
      autoFit: true,
      height: 400,
    });
    chart.data(creatData());
    chart.animate(false);

    chart.coordinate('polar', {
      startAngle: (-9 / 8) * Math.PI,
      endAngle: (1 / 8) * Math.PI,
      radius: 0.75,
    });
    chart.scale('value', {
      min: 0,
      max: 100,
      tickInterval: 5,
    });

    chart.axis('1', false);
    chart.axis('value', {
      line: null,
      label: {
        offset: -40,
        style: {
          fontSize: 18,
          fill: '#CBCBCB',
          textAlign: 'center',
          textBaseline: 'middle',
        },
      },
      tickLine: {
        length: -24,
      },
      grid: null,
    });
    chart.legend(false);
    chart.tooltip(false);
    chart
      .point()
      .position('value*1')
      .shape('pointer')
      .color('value', (val) => {
        if (val < 40) {
          return color[0];
        } else if (val <= 80) {
          return color[1];
        } else if (val <= 100) {
          return color[2];
        }
      });

    this.drawCpuChart(chart, creatData());
    this.chartArray.push(chart);
    this.cpuChart = chart;
  }

  drawCpuChart(chart, data) {

    const color = ['#0086FA', '#FFBF00', '#F5222D'];
    const val = data[0].value;
    const lineWidth = 25;
    chart.annotation().clear(true);
    // 绘制仪表盘背景
    chart.annotation().arc({
      top: false,
      start: [0, 1],
      end: [100, 1],
      style: {
        stroke: '#CBCBCB',
        lineWidth,
        lineDash: null,
      },
    });

    if (val < 40) {
      chart.annotation().arc({
        start: [0, 1],
        end: [val, 1],
        style: {
          stroke: color[0],
          lineWidth,
          lineDash: null,
        },
      });
    }

    if (val > 40 && val <= 80) {
      chart.annotation().arc({
        start: [0, 1],
        end: [40, 1],
        style: {
          stroke: color[0],
          lineWidth,
          lineDash: null,
        },
      });
      chart.annotation().arc({
        start: [40, 1],
        end: [val, 1],
        style: {
          stroke: color[1],
          lineWidth,
          lineDash: null,
        },
      });
    }

    if (val >= 80) {
      chart.annotation().arc({
        start: [0, 1],
        end: [40, 1],
        style: {
          stroke: color[0],
          lineWidth,
          lineDash: null,
        },
      });
      chart.annotation().arc({
        start: [40, 1],
        end: [80, 1],
        style: {
          stroke: color[1],
          lineWidth,
          lineDash: null,
        },
      });
      chart.annotation().arc({
        start: [80, 1],
        end: [val, 1],
        style: {
          stroke: color[2],
          lineWidth,
          lineDash: null,
        },
      });
    }

    // 绘制指标数字
    chart.annotation().text({
      position: ['50%', '85%'],
      content: '应用CPU使用率',
      style: {
        fontSize: 20,
        fill: '#545454',
        textAlign: 'center',
      },
    });
    chart.annotation().text({
      position: ['50%', '90%'],
      content: `${data[0].value} %`,
      style: {
        fontSize: 36,
        fill: '#545454',
        textAlign: 'center',
      },
      offsetY: 15,
    });
    chart.changeData(data);
  }

  //#endregion

  //#region  内存图表
  initMemoryChart() {

    const chart = new Chart({
      container: 'c6',
      autoFit: true,
      height: 400,
    });

    chart.data(this.memoryChartData);
    chart.scale({
      time: {
        range: [0, 1],
      },
      size: {
        nice: true,
        min: 0,
      },
    });

    chart.tooltip({
      showCrosshairs: true,
      shared: true,
    });

    chart.axis('size', {
      label: {
        formatter: (val) => {
          return val + ' MB';
        },
      },
    });

    // 线
    chart.line().position('time*size').color('type').shape('smooth');

    // 点
    chart.point().position('time*size').color('type').shape('circle');

    chart.render();
    this.chartArray.push(chart);
    this.memoryChart = chart;
  }
  //#endregion

  //#region 垃圾回收区域饼图
  initGarbageChart() {

    const chart = new Chart({
      container: 'c2',
      autoFit: true,
      height: 400,
    });
    chart.data(this.garbageChartData);
    chart.scale('percent', {
      formatter: (val) => {
        val = val * 100 + '%';
        return val;
      },
    });
    chart.coordinate('theta', {
      radius: 0.75,
      innerRadius: 0.6,
    });
    chart.tooltip({
      showTitle: false,
      showMarkers: false,
      itemTpl: '<li class="g2-tooltip-list-item"><span style="background-color:{color};" class="g2-tooltip-marker"></span>{name}: {value}</li>',
    });

    // 辅助文本
    chart
      .annotation()
      .text({
        position: ['50%', '50%'],
        content: '存储',
        style: {
          fontSize: 14,
          fill: '#8c8c8c',
          textAlign: 'center',
        },
        offsetY: -20,
      });
    chart
      .interval().adjust('stack').position(['percent']).color('item')
      .label('percent', (percent) => {
        return {
          content: (data) => {
            return `${data.item}: ${(percent * 100).toFixed(2)}% ${data.count}B`;
          },
        };
      })
      .tooltip('item*percent', (item, percent) => {
        percent = percent * 100 + '%';
        return {
          name: item,
          value: percent,
        };
      });

    chart.interaction('element-active');

    chart.render();
    this.chartArray.push(chart);
    this.garbageChart = chart;
  }
  //#endregion


  initGraph1() {

    const data = [
      { type: '收纳', value: 340, cat: '办公用品' },
      { type: '笔', value: 20760, cat: '办公用品' },
      { type: '纸张', value: 28750, cat: '办公用品' },
      { type: '配件', value: 4090, cat: '技术' },
      { type: '电话', value: 9880, cat: '技术' },
      { type: '复印机', value: 40988, cat: '技术' },
      { type: '桌子', value: 14870, cat: '家具' },
      { type: '椅子', value: 37098, cat: '家具' },
      { type: '书架', value: 49099, cat: '家具' },
    ];
    const chart = new Chart({
      container: 'c1',
      autoFit: true,
      height: 400,
      syncViewPadding: true,
    });
    chart.data(data);
    chart.scale({
      value: {
        max: 55000,
        min: 0,
        alias: '金额（元）',
      },
    });
    chart.axis('type', {
      tickLine: null,
      line: null,
    });
    chart.axis('value', {
      label: null,
      title: {
        offset: 30,
        style: {
          fontWeight: 300,
        },
      },
      grid: null,
    });
    chart.legend(false);
    chart.coordinate('rect').transpose();
    chart
      .interval()
      .position('type*value')
      .color('cat', ['#face1d', '#37c461', '#2194ff'])
      .size(26)
      .label('value', {
        style: {
          fill: '#8d8d8d',
        },
        offset: 10,
        content: (originData) => {
          return (originData.value + '').replace(/(\d)(?=(?:\d{3})+$)/g, '$1,');
        },
      });

    chart.annotation().text({
      top: true,
      position: ['椅子', 'min'],
      content: '家具',
      style: {
        fill: '#c0c0c0',
        fontSize: 12,
        fontWeight: 300,
        textAlign: 'center',
      },
      offsetX: -70,
      rotate: Math.PI * -0.5
    });
    chart.annotation().text({
      top: true,
      position: ['电话', 'min'],
      content: '技术',
      style: {
        fill: '#c0c0c0',
        fontSize: 12,
        fontWeight: 300,
        textAlign: 'center',
      },
      offsetX: -70,
      rotate: Math.PI * -0.5
    });
    chart.annotation().text({
      top: true,
      position: ['笔', 'min'],
      content: '办公用品',
      style: {
        fill: '#c0c0c0',
        fontSize: 12,
        fontWeight: 300,
        textAlign: 'center',
      },
      offsetX: -70,
      rotate: Math.PI * -0.5
    });
    chart.annotation().line({
      start: ['-20%', '33.2%'],
      end: ['100%', '33.2%'],
      style: {
        stroke: '#c0c0c0',
        lineDash: [2, 2],
      },
    });
    chart.annotation().line({
      start: ['-20%', '66.8%'],
      end: ['100%', '66.8%'],
      style: {
        stroke: '#c0c0c0',
        lineDash: [2, 2],
      },
    });
    chart.interaction('element-active');
    chart.theme('dark');
    chart.render();
  }

  initGraph3() {

    function getFillAttrs(cfg) {
      return {
        ...cfg.defaultStyle,
        ...cfg.style,
        fill: cfg.color,
        fillOpacity: cfg.opacity,
      };
    }
    function getRectPath(points) {
      const path = [];
      for (let i = 0; i < points.length; i++) {
        const point = points[i];
        if (point) {
          const action = i === 0 ? 'M' : 'L';
          path.push([action, point.x, point.y]);
        }
      }
      const first = points[0];
      path.push(['L', first.x, first.y]);
      path.push(['z']);
      return path;
    }

    // 顶边带圆角
    registerShape('interval', 'top', {
      draw(cfg, container) {
        const attrs = getFillAttrs(cfg);
        let path = getRectPath(cfg.points);
        path = this.parsePath(path); // 将 0 - 1 的值转换为画布坐标
        const radius = (path[2][1] - path[1][1]) / 2;
        const temp = [];
        temp.push(['M', path[0][1], path[0][2]]);
        temp.push(['L', path[1][1], path[1][2] + radius]);
        temp.push(['A', radius, radius, 90, 0, 1, path[1][1] + radius, path[1][2]]);
        temp.push(['L', path[2][1] - radius, path[2][2]]);
        temp.push(['A', radius, radius, 90, 0, 1, path[2][1], path[2][2] + radius]);
        temp.push(['L', path[3][1], path[3][2]]);
        temp.push(['Z']);

        const group = container.addGroup();
        group.addShape('path', {
          attrs: {
            ...attrs,
            path: temp,
          },
        });

        return group;
      },
    });

    // 底边带圆角
    registerShape('interval', 'bottom', {
      draw(cfg, container) {
        const attrs = getFillAttrs(cfg);
        let path = getRectPath(cfg.points);
        path = this.parsePath(path);
        const radius = (path[2][1] - path[1][1]) / 2;
        const temp = [];
        temp.push(['M', path[0][1] + radius, path[0][2]]);
        temp.push(['A', radius, radius, 90, 0, 1, path[0][1], path[0][2] - radius]);
        temp.push(['L', path[1][1], path[1][2]]);
        temp.push(['L', path[2][1], path[2][2]]);
        temp.push(['L', path[3][1], path[3][2] - radius]);
        temp.push(['A', radius, radius, 90, 0, 1, path[3][1] - radius, path[3][2]]);
        temp.push(['Z']);

        const group = container.addGroup();
        group.addShape('path', {
          attrs: {
            ...attrs,
            path: temp,
          },
        });

        return group;
      },
    });

    const data = [
      { year: '2014', type: 'Sales', sales: 1000 },
      { year: '2015', type: 'Sales', sales: 1170 },
      { year: '2016', type: 'Sales', sales: 660 },
      { year: '2017', type: 'Sales', sales: 1030 },
      { year: '2014', type: 'Expenses', sales: 400 },
      { year: '2015', type: 'Expenses', sales: 460 },
      { year: '2016', type: 'Expenses', sales: 1120 },
      { year: '2017', type: 'Expenses', sales: 540 },
      { year: '2014', type: 'Profit', sales: 300 },
      { year: '2015', type: 'Profit', sales: 300 },
      { year: '2016', type: 'Profit', sales: 300 },
      { year: '2017', type: 'Profit', sales: 350 },
    ];

    const chart = new Chart({
      container: 'c3',
      autoFit: true,
      height: 400,
    });

    chart.data(data);
    chart.scale({
      sales: {
        max: 2400,
        tickInterval: 600,
        nice: true,
      },
    });

    const axisCfg = {
      title: null,
      label: {
        style: {
          fontFamily: 'Monospace',
          fontWeight: 700,
          fontSize: 14,
          fill: '#545454',
        },
      },
      grid: {
        line: {
          style: {
            lineDash: null,
            stroke: '#545454',
          },
        },
      },
      line: {
        style: {
          lineDash: null,
          stroke: '#545454',
        },
      },
    };

    chart.axis('year', axisCfg);
    chart.axis('sales', { ...axisCfg, line: null });

    chart.tooltip({
      showMarkers: false
    });

    chart
      .interval()
      .position('year*sales')
      .color('type')
      .size(35)
      .shape('type', (val) => {
        if (val === 'Profit') {
          // 顶部圆角
          return 'bottom';
        } else if (val === 'Sales') {
          // 底部圆角
          return 'top';
        }
      })
      .style({
        stroke: '#545454',
        lineWidth: 2,
      })
      .adjust('stack');

    chart.interaction('element-highlight-by-color');

    chart.render();

    this.chartArray.push(chart);
  }

  initGraph5() {

    const data = [
      [0, 0, 10],
      [0, 1, 19],
      [0, 2, 8],
      [0, 3, 24],
      [0, 4, 67],
      [1, 0, 92],
      [1, 1, 58],
      [1, 2, 78],
      [1, 3, 117],
      [1, 4, 48],
      [2, 0, 35],
      [2, 1, 15],
      [2, 2, 123],
      [2, 3, 64],
      [2, 4, 52],
      [3, 0, 72],
      [3, 1, 132],
      [3, 2, 114],
      [3, 3, 19],
      [3, 4, 16],
      [4, 0, 38],
      [4, 1, 5],
      [4, 2, 8],
      [4, 3, 117],
      [4, 4, 115],
      [5, 0, 88],
      [5, 1, 32],
      [5, 2, 12],
      [5, 3, 6],
      [5, 4, 120],
      [6, 0, 13],
      [6, 1, 44],
      [6, 2, 88],
      [6, 3, 98],
      [6, 4, 96],
      [7, 0, 31],
      [7, 1, 1],
      [7, 2, 82],
      [7, 3, 32],
      [7, 4, 30],
      [8, 0, 85],
      [8, 1, 97],
      [8, 2, 123],
      [8, 3, 64],
      [8, 4, 84],
      [9, 0, 47],
      [9, 1, 114],
      [9, 2, 31],
      [9, 3, 48],
      [9, 4, 91],
    ];

    const source = data.map((arr) => {
      return {
        name: arr[0],
        day: arr[1],
        sales: arr[2],
      };
    });
    const chart = new Chart({
      container: 'c5',
      autoFit: true,
      height: 400,
    });

    chart.data(source);

    chart.scale('name', {
      type: 'cat',
      values: ['Alexander', 'Marie', 'Maximilian', 'Sophia', 'Lukas', 'Maria', 'Leon', 'Anna', 'Tim', 'Laura'],
    });
    chart.scale('day', {
      type: 'cat',
      values: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'],
    });
    chart.scale('sales', {
      nice: true,
    });

    chart.axis('name', {
      tickLine: null,
      grid: {
        alignTick: false,
        line: {
          style: {
            lineWidth: 1,
            lineDash: null,
            stroke: '#f0f0f0',
          },
        },
      },
    });

    chart.axis('day', {
      title: null,
      grid: {
        alignTick: false,
        line: {
          style: {
            lineWidth: 1,
            lineDash: null,
            stroke: '#f0f0f0',
          },
        },
      },
    });

    chart.tooltip(false);
    chart.interaction('brush-visible');

    chart
      .polygon()
      .position('name*day')
      .color('sales', '#BAE7FF-#1890FF-#0050B3')
      .label('sales', {
        offset: -2,
        style: {
          fill: '#fff',
          shadowBlur: 2,
          shadowColor: 'rgba(0, 0, 0, .45)',
        },
      })
      .style({
        lineWidth: 1,
        stroke: '#fff',
      }).state({
        active: {
          style: {
            fillOpacity: 0.9
          },
        },
        inactive: {
          style: {
            fillOpacity: 0.4
          },
        }
      });;

    chart.render();
    this.chartArray.push(chart);

  }
}
