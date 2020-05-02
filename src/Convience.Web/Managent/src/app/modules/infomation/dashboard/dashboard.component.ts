import { Component, OnInit, AfterViewInit } from '@angular/core';
import { StorageService } from 'src/app/core/services/storage.service';
import { Router } from '@angular/router';
import { Chart, registerShape } from '@antv/g2';
import { RangePoint } from '@antv/g2/lib/interface';
import { DashboardService } from 'src/app/services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, AfterViewInit {

  data = { userCount: 0, roleCount: 0, departmentCount: 0, positionCount: 0 };

  constructor(private dashboardService: DashboardService) {
  }

  ngOnInit() {
    this.dashboardService.get().subscribe((result: any) => this.data = result);
  }

  ngAfterViewInit(): void {
    this.initGraph1();
    this.initGraph2();
    this.initGraph3();
    this.initGraph4();
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
      padding: [20, 100, 50, 100],
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
        fontWeight: '300',
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
        fontWeight: '300',
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
        fontWeight: '300',
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
  }

  initGraph3() {
    const expectData = [
      { value: 100, name: '展现' },
      { value: 80, name: '点击' },
      { value: 60, name: '访问' },
      { value: 40, name: '咨询' },
      { value: 30, name: '订单' },
    ];
    const actualData = [
      { value: 80, name: '展现' },
      { value: 50, name: '点击' },
      { value: 30, name: '访问' },
      { value: 10, name: '咨询' },
      { value: 5, name: '订单' },
    ];
    const chart = new Chart({
      container: 'c3',
      autoFit: true,
      height: 500,
      padding: [20, 100, 40, 60],
    });
    chart
      .coordinate('rect')
      .transpose()
      .scale(1, -1);
    chart.axis(false);
    chart.legend(false);
    chart.tooltip({
      showTitle: false,
      showMarkers: false,
      shared: true,
      itemTpl: '<li class="g2-tooltip-list-item"><span style="background-color:{color};" class="g2-tooltip-marker"></span>{name}: {value}</li>',
    });

    const expectView = chart.createView();
    expectView.data(expectData);
    expectView
      .interval()
      .adjust('symmetric')
      .position('name*value')
      .color('name', ['#0050B3', '#1890FF', '#40A9FF', '#69C0FF', '#BAE7FF'])
      .shape('pyramid')
      .tooltip('name*value', (name, value) => {
        return {
          name: '预期' + name,
          value,
        };
      })
      .label('name', {
        offset: 35,
        labelLine: {
          style: {
            lineWidth: 1,
            stroke: 'rgba(0, 0, 0, 0.15)',
          },
        },
      })
      .animate({
        appear: {
          animation: 'fade-in'
        }
      });

    const actualView = chart.createView();
    actualView.data(actualData);
    actualView
      .interval()
      .adjust('symmetric')
      .position('name*value')
      .color('name', ['#0050B3', '#1890FF', '#40A9FF', '#69C0FF', '#BAE7FF'])
      .shape('pyramid')
      .tooltip('name*value', (name, value) => {
        return {
          name: '实际' + name,
          value,
        };
      })
      .style({
        lineWidth: 1,
        stroke: '#fff',
      })
      .animate({
        appear: {
          animation: 'fade-in'
        },
        update: {
          animation: 'fade-in'
        }
      });

    chart.interaction('element-active');

    chart.render();

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
      height: 500,
      padding: [0, 0, 30, 0],
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

  }
}
