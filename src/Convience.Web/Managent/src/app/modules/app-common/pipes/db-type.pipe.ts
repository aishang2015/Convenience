import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dbType'
})
export class DbTypePipe implements PipeTransform {

  private _dbTypeMap: { [key: number]: string } = {
    0: 'SqlServer',
    1: 'Sqlite',
    2: 'MySQL',
    3: 'PostgreSQL'
  }

  transform(value: number, args?: any): any {
    return this._dbTypeMap[value];
  }

}
