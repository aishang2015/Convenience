import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'gender'
})
export class GenderPipe implements PipeTransform {

  private _genderMap: { [key: number]: string } = {
    0: '未知',
    1: '男',
    2: '女'
  }

  transform(value: number, args?: any): any {
    return this._genderMap[value];
  }

}
