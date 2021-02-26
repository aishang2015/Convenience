import { Component, OnInit, AfterViewInit, AfterViewChecked, ApplicationRef } from '@angular/core';
import { Chart, registerShape } from '@antv/g2';
import { timeout } from 'rxjs/operators';
import { DashboardService } from 'src/app/business/dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less'],
  providers: [ApplicationRef]
})
export class DashboardComponent implements OnInit, AfterViewInit {

  data = { userCount: 0, roleCount: 0, departmentCount: 0, positionCount: 0 };

  chartArray = [];

  constructor(private _dashboardService: DashboardService,private appref: ApplicationRef) {
  }

  ngOnInit() {
    this._dashboardService.get().subscribe((result: any) => this.data = result);
  }

  ngAfterViewInit(): void {
    this.initGraph1();
    this.initGraph2();
    this.initGraph3();
    this.initGraph4();
    this.initGraph5();
    this.initGraph6();

    // 初始图像宽度会溢出，通过resize事件触发图标重绘
    setTimeout(() => {   
      var myEvent = new Event('resize');
      window.dispatchEvent(myEvent);
    }, 10);
  }

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

  initGraph2() {

    const data = [
      { item: '事例一', count: 40, percent: 0.4 },
      { item: '事例二', count: 21, percent: 0.21 },
      { item: '事例三', count: 17, percent: 0.17 },
      { item: '事例四', count: 13, percent: 0.13 },
      { item: '事例五', count: 9, percent: 0.09 },
    ];
    const chart = new Chart({
      container: 'c2',
      autoFit: true,
      height: 400,
    });
    chart.data(data);
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
        content: '主机',
        style: {
          fontSize: 14,
          fill: '#8c8c8c',
          textAlign: 'center',
        },
        offsetY: -20,
      })
      .text({
        position: ['50%', '50%'],
        content: '200',
        style: {
          fontSize: 20,
          fill: '#8c8c8c',
          textAlign: 'center',
        },
        offsetX: -10,
        offsetY: 20,
      })
      .text({
        position: ['50%', '50%'],
        content: '台',
        style: {
          fontSize: 14,
          fill: '#8c8c8c',
          textAlign: 'center',
        },
        offsetY: 20,
        offsetX: 20,
      });
    chart
      .interval().adjust('stack').position('percent').color('item')
      .label('percent', (percent) => {
        return {
          content: (data) => {
            return `${data.item}: ${percent * 100}%`;
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

  initGraph4() {
    function creatData() {
      const data = [];
      const val = (Math.random() * 6).toFixed(1);
      data.push({ value: +val });
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
      max: 6,
      tickInterval: 1,
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
        if (val < 2) {
          return color[0];
        } else if (val <= 4) {
          return color[1];
        } else if (val <= 6) {
          return color[2];
        }
      });

    draw(creatData());
    setInterval(function () {
      draw(creatData());
    }, 1000);

    function draw(data) {
      const val = data[0].value;
      const lineWidth = 25;
      chart.annotation().clear(true);
      // 绘制仪表盘背景
      chart.annotation().arc({
        top: false,
        start: [0, 1],
        end: [6, 1],
        style: {
          stroke: '#CBCBCB',
          lineWidth,
          lineDash: null,
        },
      });

      if (val >= 2) {
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

      if (val >= 4) {
        chart.annotation().arc({
          start: [2, 1],
          end: [4, 1],
          style: {
            stroke: color[1],
            lineWidth,
            lineDash: null,
          },
        });
      }

      if (val > 4 && val <= 6) {
        chart.annotation().arc({
          start: [4, 1],
          end: [val, 1],
          style: {
            stroke: color[2],
            lineWidth,
            lineDash: null,
          },
        });
      }

      if (val > 2 && val <= 4) {
        chart.annotation().arc({
          start: [2, 1],
          end: [val, 1],
          style: {
            stroke: color[1],
            lineWidth,
            lineDash: null,
          },
        });
      }

      if (val < 2) {
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

      // 绘制指标数字
      chart.annotation().text({
        position: ['50%', '85%'],
        content: '合格率',
        style: {
          fontSize: 20,
          fill: '#545454',
          textAlign: 'center',
        },
      });
      chart.annotation().text({
        position: ['50%', '90%'],
        content: `${data[0].value * 10} %`,
        style: {
          fontSize: 36,
          fill: '#545454',
          textAlign: 'center',
        },
        offsetY: 15,
      });
      chart.changeData(data);
    }
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

  initGraph6() {

    const data = [
      { month: 'Jan', city: 'Tokyo', temperature: 7 },
      { month: 'Jan', city: 'London', temperature: 3.9 },
      { month: 'Feb', city: 'Tokyo', temperature: 6.9 },
      { month: 'Feb', city: 'London', temperature: 4.2 },
      { month: 'Mar', city: 'Tokyo', temperature: 9.5 },
      { month: 'Mar', city: 'London', temperature: 5.7 },
      { month: 'Apr', city: 'Tokyo', temperature: 14.5 },
      { month: 'Apr', city: 'London', temperature: 8.5 },
      { month: 'May', city: 'Tokyo', temperature: 18.4 },
      { month: 'May', city: 'London', temperature: 11.9 },
      { month: 'Jun', city: 'Tokyo', temperature: 21.5 },
      { month: 'Jun', city: 'London', temperature: 15.2 },
      { month: 'Jul', city: 'Tokyo', temperature: 25.2 },
      { month: 'Jul', city: 'London', temperature: 17 },
      { month: 'Aug', city: 'Tokyo', temperature: 26.5 },
      { month: 'Aug', city: 'London', temperature: 16.6 },
      { month: 'Sep', city: 'Tokyo', temperature: 23.3 },
      { month: 'Sep', city: 'London', temperature: 14.2 },
      { month: 'Oct', city: 'Tokyo', temperature: 18.3 },
      { month: 'Oct', city: 'London', temperature: 10.3 },
      { month: 'Nov', city: 'Tokyo', temperature: 13.9 },
      { month: 'Nov', city: 'London', temperature: 6.6 },
      { month: 'Dec', city: 'Tokyo', temperature: 9.6 },
      { month: 'Dec', city: 'London', temperature: 4.8 },
    ];

    const chart = new Chart({
      container: 'c6',
      autoFit: true,
      height: 400,
    });

    chart.data(data);
    chart.scale({
      month: {
        range: [0, 1],
      },
      temperature: {
        nice: true,
      },
    });

    chart.tooltip({
      showCrosshairs: true,
      shared: true,
    });

    chart.axis('temperature', {
      label: {
        formatter: (val) => {
          return val + ' °C';
        },
      },
    });

    chart
      .line()
      .position('month*temperature')
      .color('city')
      .shape('smooth');

    chart
      .point()
      .position('month*temperature')
      .color('city')
      .shape('circle');

    chart.render();
    this.chartArray.push(chart);

  }
}
